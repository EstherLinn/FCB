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
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using Templates = Feature.Wealth.Component.Models.Bond.Template;

namespace Feature.Wealth.Component.Repositories
{
    public class BondRepository
    {
        private readonly IDbConnection _dbConnection = DbManager.Custom.DbConnection();
        private IList<BondHistoryPrice> _bondHistoryPrices;

        public IList<Bond> GetBondList()
        {
            string sql = @"SELECT
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
                           ,B.[RedemptionFee]
                           ,B.[SubscriptionFee]
                           ,SUBSTRING(B.[Date],1,4)+'/'+SUBSTRING(B.[Date],5,2)+'/'+SUBSTRING(B.[Date],7,2) AS [Date]
                           ,B.[ReservedColumn]
                           ,B.[Note]
                           ,B.[PreviousInterest]
                           ,B.[SPCreditRating]
                           ,B.[MoodyCreditRating]
                           ,B.[FitchCreditRating]
                           ,B.[YieldRateYTM]
                           FROM [BondList] AS A WITH (NOLOCK)
                           LEFT JOIN [BondNav] AS B WITH (NOLOCK) ON A.BondCode = SUBSTRING(B.BondCode, 1, 4)
                           LEFT JOIN [Currency] AS C WITH (NOLOCK) ON A.CurrencyCode = C.CurrencyCode
                           ORDER BY A.BondCode";

            var bonds = this._dbConnection.Query<Bond>(sql)?.ToList() ?? new List<Bond>();

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
                bonds[i].InterestRate = Round4(bonds[i].InterestRate);
                bonds[i].RedemptionFee = Round2(bonds[i].RedemptionFee);
                bonds[i].SubscriptionFee = Round2(bonds[i].SubscriptionFee);
                bonds[i].PreviousInterest = Round4(bonds[i].PreviousInterest);
                bonds[i].YieldRateYTM = Round2(bonds[i].YieldRateYTM);
                bonds[i].UpsAndDownsMonth = Round2(bonds[i].UpsAndDownsMonth);
                bonds[i].UpsAndDownsMonth = Round2(bonds[i].UpsAndDownsMonth);

                bonds[i].DetailLink = bonds[i].DetailLink + "?id=" + bonds[i].BondCode;
                bonds[i] = GetButtonHtml(bonds[i], true);
                bonds[i] = SetTags(bonds[i]);

                if (DateTime.TryParse(bonds[i].MaturityDate, out var d))
                {
                    var diff = d.Subtract(now).TotalDays;
                    if (diff > 0)
                    {
                        bonds[i].MaturityYear = decimal.Parse((diff / 365).ToString());
                    }
                    else
                    {
                        bonds[i].MaturityYear = 0;
                    }
                }
                else
                {
                    bonds[i].MaturityYear = 0;
                }

                bonds[i].MaturityYear = Round2(bonds[i].MaturityYear);

                if (int.TryParse(bonds[i].MinIncrementAmount, out var min))
                {
                    bonds[i].MinIncrementAmountNumber = min;
                }
                else
                {
                    bonds[i].MinIncrementAmountNumber = 0;
                }

                if (DateTime.TryParse(bonds[i].Date, out var oneMonthAgo))
                {
                    oneMonthAgo = oneMonthAgo.AddMonths(-1);
                    bonds[i].UpsAndDownsMonth = GetUpsAndDowns(bonds[i].BondCode, bonds[i].RedemptionFee, oneMonthAgo.ToString("yyyyMMdd"), false);
                }

                if (DateTime.TryParse(bonds[i].Date, out var threeMonthAgo))
                {
                    threeMonthAgo = threeMonthAgo.AddMonths(-3);
                    bonds[i].UpsAndDownsSeason = GetUpsAndDowns(bonds[i].BondCode, bonds[i].RedemptionFee, threeMonthAgo.ToString("yyyyMMdd"), false);
                }
            }

            return bonds;
        }

        public Bond GetBond(string bondCode)
        {
            string sql = @"SELECT
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
                           ,B.[RedemptionFee]
                           ,B.[SubscriptionFee]
                           ,SUBSTRING(B.[Date],1,4)+'/'+SUBSTRING(B.[Date],5,2)+'/'+SUBSTRING(B.[Date],7,2) AS [Date]
                           ,B.[ReservedColumn]
                           ,B.[Note]
                           ,B.[PreviousInterest]
                           ,B.[SPCreditRating]
                           ,B.[MoodyCreditRating]
                           ,B.[FitchCreditRating]
                           ,B.[YieldRateYTM]
                           FROM [BondList] AS A WITH (NOLOCK)
                           LEFT JOIN [BondNav] AS B WITH (NOLOCK) ON A.BondCode = SUBSTRING(B.BondCode, 1, 4)
                           LEFT JOIN [Currency] AS C WITH (NOLOCK) ON A.CurrencyCode = C.CurrencyCode
                           WHERE A.BondCode = @BondCode";

            var bond = this._dbConnection.Query<Bond>(sql, new { BondCode = bondCode })?.FirstOrDefault() ?? new Bond();

            var now = DateTime.Now;

            bond.InterestRate = Round4(bond.InterestRate);
            bond.RedemptionFee = Round2(bond.RedemptionFee);
            bond.SubscriptionFee = Round2(bond.SubscriptionFee);
            bond.PreviousInterest = Round4(bond.PreviousInterest);
            bond.YieldRateYTM = Round2(bond.YieldRateYTM);
            bond.UpsAndDownsMonth = Round2(bond.UpsAndDownsMonth);
            bond.UpsAndDownsMonth = Round2(bond.UpsAndDownsMonth);

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
                bond.UpsAndDownsMonth = GetUpsAndDowns(bond.BondCode, bond.RedemptionFee, oneMonthAgo.ToString("yyyyMMdd"), true);
            }

            if (DateTime.TryParse(bond.Date, out var threeMonthAgo))
            {
                threeMonthAgo = threeMonthAgo.AddMonths(-3);
                bond.UpsAndDownsSeason = GetUpsAndDowns(bond.BondCode, bond.RedemptionFee, threeMonthAgo.ToString("yyyyMMdd"), true);
            }

            return bond;
        }

        private decimal? GetUpsAndDowns(string bondCode, decimal? redemptionFee, string date, bool single)
        {
            BondHistoryPrice bondHistoryPrice = null;

            if (single)
            {
                bondHistoryPrice = GetBondHistoryPrice(bondCode, date);
            }
            else if (this._bondHistoryPrices != null && this._bondHistoryPrices.Count > 0)
            {
                bondHistoryPrice = this._bondHistoryPrices.Where(b => b.BondCode == bondCode && int.Parse(b.Date) <= int.Parse(date)).FirstOrDefault();
            }

            if (redemptionFee.HasValue && bondHistoryPrice != null && bondHistoryPrice.RedemptionFee.HasValue)
            {
                return (redemptionFee - bondHistoryPrice.RedemptionFee) / bondHistoryPrice.RedemptionFee * 100;
            }
            else
            {
                return null;
            }
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
                           ,[RedemptionFee]
                           ,[SubscriptionFee]
                           ,SUBSTRING([Date],1,4)+'/'+SUBSTRING([Date],5,2)+'/'+SUBSTRING([Date],7,2) AS [Date]
                           FROM [BondHistoryPrice] WITH (NOLOCK)
                           WHERE SUBSTRING(BondCode, 1, 4) = @BondCode
                           ORDER BY Date ASC";

            var bondHistoryPrices = this._dbConnection.Query<BondHistoryPrice>(sql, new { BondCode = bondCode })?.ToList() ?? new List<BondHistoryPrice>();

            for (int i = 0; i < bondHistoryPrices.Count; i++)
            {
                bondHistoryPrices[i].RedemptionFee = Round2(bondHistoryPrices[i].RedemptionFee);
                bondHistoryPrices[i].SubscriptionFee = Round2(bondHistoryPrices[i].SubscriptionFee);
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
                           ,[RedemptionFee]
                           ,[SubscriptionFee]
                           ,SUBSTRING([Date],1,4)+'/'+SUBSTRING([Date],5,2)+'/'+SUBSTRING([Date],7,2) AS [Date]
                           FROM [BondHistoryPrice] WITH (NOLOCK)
                           WHERE SUBSTRING(BondCode, 1, 4) = @BondCode AND [Date] <= @date
                           ORDER BY Date DESC";

            var bondHistoryPrice = this._dbConnection.Query<BondHistoryPrice>(sql, new { BondCode = bondCode, date = date })?.FirstOrDefault() ?? new BondHistoryPrice();

            bondHistoryPrice.RedemptionFee = Round2(bondHistoryPrice.RedemptionFee);
            bondHistoryPrice.SubscriptionFee = Round2(bondHistoryPrice.SubscriptionFee);

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
                           ,[RedemptionFee]
                           ,[SubscriptionFee]
                           ,[Date]
                           FROM [BondHistoryPrice] WITH (NOLOCK)
                           WHERE [Date] >= @date
                           ORDER BY Date ASC";

            var bondHistoryPrices = this._dbConnection.Query<BondHistoryPrice>(sql, new { date = date })?.ToList() ?? new List<BondHistoryPrice>();

            for (int i = 0; i < bondHistoryPrices.Count; i++)
            {
                bondHistoryPrices[i].RedemptionFee = Round2(bondHistoryPrices[i].RedemptionFee);
                bondHistoryPrices[i].SubscriptionFee = Round2(bondHistoryPrices[i].SubscriptionFee);
            }

            return bondHistoryPrices;
        }

        public decimal? Round2(decimal? value)
        {
            if (value != null)
            {
                value = decimal.Round((decimal)value, 2);
            }
            return value;
        }

        public decimal? Round4(decimal? value)
        {
            if (value != null)
            {
                value = decimal.Round((decimal)value, 4);
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
