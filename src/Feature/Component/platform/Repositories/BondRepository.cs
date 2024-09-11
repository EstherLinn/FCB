using Dapper;
using Feature.Wealth.Component.Models.Invest;
using Feature.Wealth.Component.Models.Bond;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Manager;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Caching;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using Templates = Feature.Wealth.Component.Models.Bond.Template;
using Foundation.Wealth.Models;

namespace Feature.Wealth.Component.Repositories
{
    public class BondRepository
    {
        private readonly IDbConnection _dbConnection = DbManager.Custom.DbConnection();
        private readonly DjMoneyApiRespository _djMoneyApiRespository = new DjMoneyApiRespository();

        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly string BondClassCacheKey = $"Fcb_BondClassCache";

        private IList<BondHistoryPrice> _bondHistoryPrices;
        private List<BondClass> _bondClasses = new List<BondClass>();

        public IList<Bond> GetBondList()
        {
            string BondList = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.BondList);
            string BondNav = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.BondNav);
            string sql = @$"SELECT
                           A.[BondCode] + ' ' + A.[BondName] AS [FullName]
                           ,A.[BondCode] + ' ' + A.[BondName] AS [value]
                           ,A.[BondCode]
                           ,A.[ISINCode]
                           ,A.[BondName]
                           ,A.[Currency]
                           ,A.[CurrencyCode]
                           ,C.[CurrencyName]
                           ,A.[InterestRate]
                           ,[PaymentFrequency]
                           ,CASE 
                           WHEN A.[PaymentFrequency] = 0
                           THEN '零息'
                           WHEN A.[PaymentFrequency] = 1
                           THEN '月配'
                           WHEN A.[PaymentFrequency] = 2
                           THEN '季配'
                           WHEN A.[PaymentFrequency] = 3
                           THEN '半年配'
                           WHEN A.[PaymentFrequency] = 4
                           THEN '年配'
                           ELSE ''
                           END AS [PaymentFrequencyName]
                           ,A.[RiskLevel]
                           ,A.[SalesTarget]
                           ,A.[MinSubscriptionForeign]
                           ,A.[MinSubscriptionNTD]
                           ,A.[MinIncrementAmount]
                           ,SUBSTRING(A.[MaturityDate],1,4)+'/'+SUBSTRING(A.[MaturityDate],5,2)+'/'+SUBSTRING(A.[MaturityDate],7,2) AS [MaturityDate]
                           ,CASE
                           WHEN A.[StopSubscriptionDate] = '29109999'
                           THEN NULL
                           ELSE SUBSTRING(A.[StopSubscriptionDate],1,4)+'/'+SUBSTRING(A.[StopSubscriptionDate],5,2)+'/'+SUBSTRING(A.[StopSubscriptionDate],7,2)
                           END AS [StopSubscriptionDate]
                           ,A.[RedemptionDateByIssuer]
                           ,A.[Issuer]
                           ,A.[OpenToPublic]
                           ,A.[Listed]
                           ,SUBSTRING(A.[ListingDate],1,4)+'/'+SUBSTRING(A.[ListingDate],5,2)+'/'+SUBSTRING(A.[ListingDate],7,2) AS [ListingDate]
                           ,SUBSTRING(A.[DelistingDate],1,4)+'/'+SUBSTRING(A.[DelistingDate],5,2)+'/'+SUBSTRING(A.[DelistingDate],7,2) AS [DelistingDate]
                           ,B.[SubscriptionFee]
                           ,B.[RedemptionFee]
                           ,SUBSTRING(B.[Date],1,4)+'/'+SUBSTRING(B.[Date],5,2)+'/'+SUBSTRING(B.[Date],7,2) AS [Date]
                           ,B.[ReservedColumn]
                           ,B.[Note]
                           ,B.[PreviousInterest]
                           ,B.[SPCreditRating]
                           ,B.[MoodyCreditRating]
                           ,B.[FitchCreditRating]
                           ,B.[YieldRateYTM]
                           FROM {BondList} AS A WITH (NOLOCK)
                           LEFT JOIN {BondNav} AS B WITH (NOLOCK) ON A.BondCode = SUBSTRING(B.BondCode, 1, 4)
                           LEFT JOIN [Currency] AS C WITH (NOLOCK) ON A.CurrencyCode = C.CurrencyCode
                           ORDER BY A.BondCode";

            var bonds = this._dbConnection.Query<Bond>(sql)?.ToList() ?? new List<Bond>();

            GetBondClass();

            var minDate = bonds
                .Where(b => string.IsNullOrEmpty(b.Date) == false)
                .OrderBy(b => b.Date)
                .Select(b => b.Date).FirstOrDefault();


            if (DateTime.TryParse(minDate, out var fourMonthAgo))
            {
                fourMonthAgo = fourMonthAgo.AddMonths(-4);
                this._bondHistoryPrices = GetBondHistoryPriceByDate(fourMonthAgo.ToString("yyyyMMdd"));
            }

            var now = DateTime.Now;

            for (int i = 0; i < bonds.Count; i++)
            {
                bonds[i] = MoreInfo(bonds[i], false);
            }

            return bonds;
        }

        public Bond GetBond(string bondCode)
        {
            string BondList = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.BondList);
            string BondNav = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.BondNav);
            string sql = @$"SELECT
                           A.[BondCode] + ' ' + A.[BondName] AS [FullName]
                           ,A.[BondCode] + ' ' + A.[BondName] AS [value]
                           ,A.[BondCode]
                           ,A.[ISINCode]
                           ,A.[BondName]
                           ,A.[Currency]
                           ,A.[CurrencyCode]
                           ,C.[CurrencyName]
                           ,A.[InterestRate]
                           ,[PaymentFrequency]
                           ,CASE 
                           WHEN A.[PaymentFrequency] = 0
                           THEN '零息'
                           WHEN A.[PaymentFrequency] = 1
                           THEN '月配'
                           WHEN A.[PaymentFrequency] = 2
                           THEN '季配'
                           WHEN A.[PaymentFrequency] = 3
                           THEN '半年配'
                           WHEN A.[PaymentFrequency] = 4
                           THEN '年配'
                           ELSE ''
                           END AS [PaymentFrequencyName]
                           ,A.[RiskLevel]
                           ,A.[SalesTarget]
                           ,A.[MinSubscriptionForeign]
                           ,A.[MinSubscriptionNTD]
                           ,A.[MinIncrementAmount]
                           ,SUBSTRING(A.[MaturityDate],1,4)+'/'+SUBSTRING(A.[MaturityDate],5,2)+'/'+SUBSTRING(A.[MaturityDate],7,2) AS [MaturityDate]
                           ,CASE
                           WHEN A.[StopSubscriptionDate] = '29109999'
                           THEN NULL
                           ELSE SUBSTRING(A.[StopSubscriptionDate],1,4)+'/'+SUBSTRING(A.[StopSubscriptionDate],5,2)+'/'+SUBSTRING(A.[StopSubscriptionDate],7,2)
                           END AS [StopSubscriptionDate]
                           ,A.[RedemptionDateByIssuer]
                           ,A.[Issuer]
                           ,A.[OpenToPublic]
                           ,A.[Listed]
                           ,SUBSTRING(A.[ListingDate],1,4)+'/'+SUBSTRING(A.[ListingDate],5,2)+'/'+SUBSTRING(A.[ListingDate],7,2) AS [ListingDate]
                           ,SUBSTRING(A.[DelistingDate],1,4)+'/'+SUBSTRING(A.[DelistingDate],5,2)+'/'+SUBSTRING(A.[DelistingDate],7,2) AS [DelistingDate]
                           ,B.[SubscriptionFee]
                           ,B.[RedemptionFee]
                           ,SUBSTRING(B.[Date],1,4)+'/'+SUBSTRING(B.[Date],5,2)+'/'+SUBSTRING(B.[Date],7,2) AS [Date]
                           ,B.[ReservedColumn]
                           ,B.[Note]
                           ,B.[PreviousInterest]
                           ,B.[SPCreditRating]
                           ,B.[MoodyCreditRating]
                           ,B.[FitchCreditRating]
                           ,B.[YieldRateYTM]
                           FROM {BondList} AS A WITH (NOLOCK)
                           LEFT JOIN {BondNav} AS B WITH (NOLOCK) ON A.BondCode = SUBSTRING(B.BondCode, 1, 4)
                           LEFT JOIN [Currency] AS C WITH (NOLOCK) ON A.CurrencyCode = C.CurrencyCode
                           WHERE A.BondCode = @BondCode";

            var bond = this._dbConnection.Query<Bond>(sql, new { BondCode = bondCode })?.FirstOrDefault();

            if (bond == null)
            {
                GetBondClass();
                bond = MoreInfo(bond, true);
            }

            return bond;
        }

        private void GetBondClass()
        {
            this._bondClasses = (List<BondClass>)this._cache.Get(BondClassCacheKey) ?? new List<BondClass>();

            if (this._bondClasses.Any() == false)
            {
                var result = this._djMoneyApiRespository.GetBondClass();

                if (result != null
                    && result.ContainsKey("resultSet")
                    && result["resultSet"]["result"] != null
                    && result["resultSet"]["result"].Any())
                {
                    foreach (var data in result["resultSet"]["result"])
                    {
                        this._bondClasses.Add(new BondClass
                        {
                            ISINCode = data["v1"].ToString(),
                            BondName = data["v2"].ToString(),
                            Class = data["v9"].ToString(),
                            BondCode = data["v33"].ToString(),
                        });
                    }
                }

                this._cache.Set(BondClassCacheKey, this._bondClasses, DateTimeOffset.Now.AddMinutes(600));
            }
        }

        private Bond MoreInfo(Bond bond, bool single)
        {
            var now = DateTime.Now;

            bond.InterestRate = Round4(bond.InterestRate);
            bond.SubscriptionFee = Round2(bond.SubscriptionFee);
            bond.RedemptionFee = Round2(bond.RedemptionFee);
            bond.PreviousInterest = Round4(bond.PreviousInterest);
            bond.YieldRateYTM = Round2(bond.YieldRateYTM);

            bond.DetailLink = bond.DetailLink + "?id=" + bond.BondCode;
            bond = GetButtonHtml(bond, true);
            bond = SetTags(bond);

            if (DateTime.TryParse(bond.MaturityDate, out var d))
            {
                var diff = d.Subtract(now).TotalDays;
                if (diff > 0)
                {
                    bond.MaturityYear = decimal.Parse((diff / 365).ToString());
                }
                else
                {
                    bond.MaturityYear = 0;
                }
            }
            else
            {
                bond.MaturityYear = 0;
            }

            bond.MaturityYear = Round2(bond.MaturityYear);

            if (int.TryParse(bond.MinIncrementAmount, out var min))
            {
                bond.MinIncrementAmountNumber = min;
            }
            else
            {
                bond.MinIncrementAmountNumber = 0;
            }

            if (DateTime.TryParse(bond.Date, out var oneMonthAgo))
            {
                oneMonthAgo = oneMonthAgo.AddMonths(-1);
                bond.UpsAndDownsMonth = GetUpsAndDowns(bond, oneMonthAgo.ToString("yyyyMMdd"), single);
            }

            if (DateTime.TryParse(bond.Date, out var threeMonthAgo))
            {
                threeMonthAgo = threeMonthAgo.AddMonths(-3);
                bond.UpsAndDownsSeason = GetUpsAndDowns(bond, threeMonthAgo.ToString("yyyyMMdd"), single);
            }

            bond.UpsAndDownsMonth = Round2(bond.UpsAndDownsMonth);
            bond.UpsAndDownsSeason = Round2(bond.UpsAndDownsSeason);

            bond.BondClass = this._bondClasses.Where(c => c.BondCode == bond.BondCode).Select(c => c.Class).FirstOrDefault() ?? null;

            return bond;
        }

        private decimal? GetUpsAndDowns(Bond bond, string date, bool single)
        {
            BondHistoryPrice bondHistoryPrice = null;

            if (single)
            {
                bondHistoryPrice = GetBondHistoryPrice(bond.BondCode, date);
            }
            else if (this._bondHistoryPrices != null && this._bondHistoryPrices.Count > 0)
            {
                bondHistoryPrice = this._bondHistoryPrices.Where(b => b.BondCode == bond.BondCode && int.Parse(b.Date) <= int.Parse(date)).FirstOrDefault();
            }

            if (bondHistoryPrice != null)
            {
                if (bond.SubscriptionFee.HasValue && bond.SubscriptionFee != 0 && bondHistoryPrice.SubscriptionFee.HasValue && bondHistoryPrice.SubscriptionFee != 0)
                {
                    return (bond.SubscriptionFee - bondHistoryPrice.SubscriptionFee) / bondHistoryPrice.SubscriptionFee * 100;
                }
                else if (bond.RedemptionFee.HasValue && bond.RedemptionFee != 0 && bondHistoryPrice.RedemptionFee.HasValue && bondHistoryPrice.RedemptionFee != 0)
                {
                    return (bond.RedemptionFee - bondHistoryPrice.RedemptionFee) / bondHistoryPrice.RedemptionFee * 100;
                }
            }

            return null;
        }

        /// <summary>
        /// 取得某債券所有歷史價格
        /// </summary>
        /// <param name="bondCode">債券代碼</param>
        /// <returns></returns>
        public IList<BondHistoryPrice> GetBondHistoryPrice(string bondCode)
        {
            string sql = @"SELECT
                           SUBSTRING([BondCode], 1, 4) AS [BondCode]
                           ,[Currency]
                           ,[SubscriptionFee]
                           ,[RedemptionFee]
                           ,SUBSTRING([Date],1,4)+'/'+SUBSTRING([Date],5,2)+'/'+SUBSTRING([Date],7,2) AS [Date]
                           FROM [BondHistoryPrice] WITH (NOLOCK)
                           WHERE SUBSTRING(BondCode, 1, 4) = @BondCode
                           ORDER BY Date ASC";

            var bondHistoryPrices = this._dbConnection.Query<BondHistoryPrice>(sql, new { BondCode = bondCode })?.ToList() ?? new List<BondHistoryPrice>();

            for (int i = 0; i < bondHistoryPrices.Count; i++)
            {
                bondHistoryPrices[i].SubscriptionFee = Round2(bondHistoryPrices[i].SubscriptionFee);
                bondHistoryPrices[i].RedemptionFee = Round2(bondHistoryPrices[i].RedemptionFee);
            }

            return bondHistoryPrices;
        }

        /// <summary>
        /// 取得某債券某一日前最近價格
        /// </summary>
        /// <param name="bondCode">債券債券代碼</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public BondHistoryPrice GetBondHistoryPrice(string bondCode, string date)
        {
            string sql = @"SELECT TOP (1)
                           SUBSTRING([BondCode], 1, 4) AS [BondCode]
                           ,[Currency]
                           ,[SubscriptionFee]
                           ,[RedemptionFee]
                           ,SUBSTRING([Date],1,4)+'/'+SUBSTRING([Date],5,2)+'/'+SUBSTRING([Date],7,2) AS [Date]
                           FROM [BondHistoryPrice] WITH (NOLOCK)
                           WHERE SUBSTRING(BondCode, 1, 4) = @BondCode AND [Date] <= @date
                           ORDER BY Date DESC";

            var bondHistoryPrice = this._dbConnection.Query<BondHistoryPrice>(sql, new { BondCode = bondCode, date = date })?.FirstOrDefault() ?? new BondHistoryPrice();

            bondHistoryPrice.SubscriptionFee = Round2(bondHistoryPrice.SubscriptionFee);
            bondHistoryPrice.RedemptionFee = Round2(bondHistoryPrice.RedemptionFee);

            return bondHistoryPrice;
        }

        /// <summary>
        /// 取得一定時間內債券價格資料
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public IList<BondHistoryPrice> GetBondHistoryPriceByDate(string date)
        {
            string sql = @"SELECT
                           SUBSTRING([BondCode], 1, 4) AS [BondCode]
                           ,[Currency]
                           ,[SubscriptionFee]
                           ,[RedemptionFee]
                           ,[Date]
                           FROM [BondHistoryPrice] WITH (NOLOCK)
                           WHERE [Date] >= @date
                           ORDER BY Date DESC";

            var bondHistoryPrices = this._dbConnection.Query<BondHistoryPrice>(sql, new { date = date })?.ToList() ?? new List<BondHistoryPrice>();

            for (int i = 0; i < bondHistoryPrices.Count; i++)
            {
                bondHistoryPrices[i].SubscriptionFee = Round2(bondHistoryPrices[i].SubscriptionFee);
                bondHistoryPrices[i].RedemptionFee = Round2(bondHistoryPrices[i].RedemptionFee);
            }

            return bondHistoryPrices;
        }

        public decimal? Round2(decimal? value)
        {
            if (value != null)
            {
                value = decimal.Round((decimal)value, 2, MidpointRounding.AwayFromZero);
            }
            return value;
        }

        public decimal? Round4(decimal? value)
        {
            if (value != null)
            {
                value = decimal.Round((decimal)value, 4, MidpointRounding.AwayFromZero);
            }
            return value;
        }

        internal Bond GetButtonHtml(Bond item, bool isListButton)
        {
            item.FocusButton = PublicHelpers.FocusButton(null, null, item.BondCode, item.FullName, InvestTypeEnum.ForeignBonds, isListButton);
            item.FocusButtonHtml = item.FocusButton.ToString();
            item.SubscribeButton = PublicHelpers.SubscriptionButton(null, null, item.BondCode, InvestTypeEnum.ForeignBonds, isListButton);
            item.SubscribeButtonHtml = item.SubscribeButton.ToString();
            item.AutoFocusButtonHtml = PublicHelpers.FocusTag(null, null, item.BondCode, item.FullName, InvestTypeEnum.ForeignBonds).ToString();
            item.AutoSubscribeButtonHtml = PublicHelpers.SubscriptionTag(null, null, item.BondCode, item.FullName, InvestTypeEnum.ForeignBonds).ToString();

            return item;
        }

        private List<Item> _hotKeywordTags = new List<Item>();
        private List<Item> _hotProductTags = new List<Item>();
        private List<Item> _discounts = new List<Item>();

        internal Bond SetTags(Bond bond)
        {
            if (!this._hotKeywordTags.Any() || !this._hotProductTags.Any() || !this._discounts.Any())
            {
                var tagFolder = ItemUtils.GetContentItem(Templates.BondTagFolder.Id);
                var children = ItemUtils.GetChildren(tagFolder, Templates.TagFolder.Id);

                if (children != null && children.Any())
                {
                    foreach (var child in children)
                    {
                        var tagsType = ItemUtils.GetFieldValue(child, Templates.TagFolder.Fields.TagType);

                        switch (tagsType)
                        {
                            case "HotKeywordTag":
                                this._hotKeywordTags.AddRange(ItemUtils.GetChildren(child, Templates.BondTag.Id));
                                break;
                            case "HotProductTag":
                                this._hotProductTags.AddRange(ItemUtils.GetChildren(child, Templates.BondTag.Id));
                                break;
                            case "Discount":
                                this._discounts.AddRange(ItemUtils.GetChildren(child, Templates.BondTag.Id));
                                break;
                        }
                    }
                }
            }

            foreach (var f in this._hotKeywordTags)
            {
                string tagName = ItemUtils.GetFieldValue(f, Templates.BondTag.Fields.TagName);
                string productCodeList = ItemUtils.GetFieldValue(f, Templates.BondTag.Fields.ProductCodeList);

                if (productCodeList.Contains(bond.BondCode) && !bond.HotKeywordTags.Contains(tagName))
                {
                    bond.HotKeywordTags.Add(tagName);
                }
            }

            foreach (var f in this._hotProductTags)
            {
                string tagName = ItemUtils.GetFieldValue(f, Templates.BondTag.Fields.TagName);
                string productCodeList = ItemUtils.GetFieldValue(f, Templates.BondTag.Fields.ProductCodeList);

                if (productCodeList.Contains(bond.BondCode) && !bond.HotProductTags.Contains(tagName))
                {
                    bond.HotProductTags.Add(tagName);
                }
            }

            foreach (var f in this._discounts)
            {
                string tagName = ItemUtils.GetFieldValue(f, Templates.BondTag.Fields.TagName);
                string productCodeList = ItemUtils.GetFieldValue(f, Templates.BondTag.Fields.ProductCodeList);

                if (productCodeList.Contains(bond.BondCode) && !bond.Discount.Contains(tagName))
                {
                    bond.Discount.Add(tagName);
                }
            }

            return bond;
        }
    }
}
