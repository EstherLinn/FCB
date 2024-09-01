using Dapper;
using Feature.Wealth.Component.Models.Invest;
using Feature.Wealth.Component.Models.Bond;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Manager;
using Sitecore.Data.Items;
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
                           ,A.[PaymentFrequency]
                           ,A.[RiskLevel]
                           ,A.[SalesTarget]
                           ,A.[MinSubscriptionForeign]
                           ,A.[MinSubscriptionNTD]
                           ,A.[MinIncrementAmount]
                           ,A.[MaturityDate]
                           ,A.[StopSubscriptionDate]
                           ,A.[RedemptionDateByIssuer]
                           ,A.[Issuer]
                           ,A.[OpenToPublic]
                           ,A.[Listed]
                           ,A.[ListingDate]
                           ,A.[DelistingDate]
                           ,B.[RedemptionFee]
                           ,B.[SubscriptionFee]
                           ,B.[Date]
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
                           ,A.[PaymentFrequency]
                           ,A.[RiskLevel]
                           ,A.[SalesTarget]
                           ,A.[MinSubscriptionForeign]
                           ,A.[MinSubscriptionNTD]
                           ,A.[MinIncrementAmount]
                           ,A.[MaturityDate]
                           ,A.[StopSubscriptionDate]
                           ,A.[RedemptionDateByIssuer]
                           ,A.[Issuer]
                           ,A.[OpenToPublic]
                           ,A.[Listed]
                           ,A.[ListingDate]
                           ,A.[DelistingDate]
                           ,B.[RedemptionFee]
                           ,B.[SubscriptionFee]
                           ,B.[Date]
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

            return bond;
        }

        public IList<BondHistoryPrice> GetBondHistoryPrice(string bondCode)
        {
            string sql = @"[BondCode]
                           ,[Currency]
                           ,[RedemptionFee]
                           ,[SubscriptionFee]
                           ,[Date]
                           ,[BondName]
                           FROM [BondHistoryPrice] WITH (NOLOCK)
                           WHERE SUBSTRING(BondCode, 1, 4) = @BondCode
                           ORDER BY Date ASC";

            var bondHistoryPrices = this._dbConnection.Query<BondHistoryPrice>(sql)?.ToList() ?? new List<BondHistoryPrice>();

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
