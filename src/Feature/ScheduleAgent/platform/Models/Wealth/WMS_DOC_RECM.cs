using CsvHelper.Configuration.Attributes;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    /// <summary>
    /// 國內基金商品資料檔，檔案名稱：WMS_DOC_RECM.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    public class WmsDocRecm
    {
        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品代碼
        /// </summary>
        [NullValues("", "NULL", null)]
        public string ProductCode { get; set; }

        /// <summary>
        /// 風險等級
        /// </summary>
        [NullValues("", "NULL", null)]
        public string RiskLevel { get; set; }

        /// <summary>
        /// 商品種類
        /// </summary>
        [NullValues("", "NULL", null)]
        public string ProductType { get; set; }

        /// <summary>
        /// 計價幣別
        /// </summary>
        [NullValues("", "NULL", null)]
        public string Currency { get; set; }

        /// <summary>
        /// 定期定額
        /// </summary>
        [NullValues("", "NULL", null)]
        public string RegularInvestmentPlan { get; set; }

        /// <summary>
        /// 連結標的
        /// </summary>
        [NullValues("", "NULL", null)]
        public string UnderlyingAsset { get; set; }

        /// <summary>
        /// 網站連結
        /// </summary>
        [NullValues("", "NULL", null)]
        public string WebsiteLink { get; set; }

        /// <summary>
        /// 期間
        /// </summary>
        [NullValues("", "NULL", null)]
        public string Duration { get; set; }

        /// <summary>
        /// 目標客群
        /// </summary>
        [NullValues("", "NULL", null)]
        public string TargetAudience { get; set; }

        /// <summary>
        /// 產品特色
        /// </summary>
        [NullValues("", "NULL", null)]
        public string ProductFeatures { get; set; }

        /// <summary>
        /// 投資期間
        /// </summary>
        [NullValues("", "NULL", null)]
        public string InvestmentPeriod { get; set; }

        /// <summary>
        /// 出版日期
        /// </summary>
        [NullValues("", "NULL", null)]
        public string PublicationDate { get; set; }

        /// <summary>
        /// 商品形態
        /// </summary>
        [NullValues("", "NULL", null)]
        public string ProductForm { get; set; }

        /// <summary>
        /// 配息頻率
        /// </summary>
        [NullValues("", "NULL", null)]
        public string DividendDistributionFrequency { get; set; }

        /// <summary>
        /// 收益攤還
        /// </summary>
        [NullValues("", "NULL", null)]
        public string IncomeAllocation { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        [NullValues("", "NULL", null)]
        public string Amount { get; set; }

        /// <summary>
        /// 機構
        /// </summary>
        [NullValues("", "NULL", null)]
        public string Institution { get; set; }

        /// <summary>
        /// 方案內容
        /// </summary>
        [NullValues("", "NULL", null)]
        public string SchemeDetails { get; set; }

        /// <summary>
        /// 擔保
        /// </summary>
        [NullValues("", "NULL", null)]
        public string Collateral { get; set; }

        /// <summary>
        /// ISIN Code
        /// </summary>
        [NullValues("", "NULL", null)]
        public string ISINCode { get; set; }

        /// <summary>
        /// 已撤銷核備
        /// </summary>
        [NullValues("", "NULL", null)]
        public string WithdrawnApproval { get; set; }

        /// <summary>
        /// 是否上架
        /// </summary>
        [NullValues("", "NULL", null)]
        public string AvailabilityStatus { get; set; }

        /// <summary>
        /// 上架日期
        /// </summary>
        [NullValues("", "NULL", null)]
        public string ListingDate { get; set; }

        /// <summary>
        /// 下架日期
        /// </summary>
        [NullValues("", "NULL", null)]
        public string DelistingDate { get; set; }

        /// <summary>
        /// 最低單筆投資金額
        /// </summary>
        [NullValues("", "NULL", null)]
        public string MinimumSingleInvestmentAmount { get; set; }

        /// <summary>
        /// 最低定期定額投資金額
        /// </summary>
        [NullValues("", "NULL", null)]
        public string MinimumRegularInvestmentAmount { get; set; }

        /// <summary>
        /// 最低不定期定額投資金額
        /// </summary>
        [NullValues("", "NULL", null)]
        public string MinimumIrregularInvestmentAmount { get; set; }

        /// <summary>
        /// 申購費用
        /// </summary>
        [NullValues("", "NULL", null)]
        public string SubscriptionFee { get; set; }

        /// <summary>
        /// 赎回費用
        /// </summary>
        [NullValues("", "NULL", null)]
        public string RedemptionFee { get; set; }

        /// <summary>
        /// 銀行相關說明
        /// </summary>
        [NullValues("", "NULL", null)]
        public string BankRelatedInstructions { get; set; }

        /// <summary>
        /// 商品識別碼
        /// </summary>
        [NullValues("", "NULL", null)]
        public string ProductIdentifier { get; set; }

        /// <summary>
        /// 是否可於網路申購
        /// </summary>
        [NullValues("", "NULL", null)]
        public string OnlineSubscriptionAvailability { get; set; }

        /// <summary>
        /// 百元標的
        /// </summary>
        [NullValues("", "NULL", null)]
        public string TargetName { get; set; }

        /// <summary>
        /// 申購淨值日
        /// </summary>
        [NullValues("", "NULL", null)]
        public string SubscriptionNAVDate { get; set; }

        /// <summary>
        /// 赎回淨值日
        /// </summary>
        [NullValues("", "NULL", null)]
        public string RedemptionNAVDate { get; set; }

        /// <summary>
        /// 贖回入款日
        /// </summary>
        [NullValues("", "NULL", null)]
        public string RedemptionDepositDate { get; set; }

        /// <summary>
        /// 商品到期日
        /// </summary>
        [NullValues("", "NULL", null)]
        public string ProductMaturityDate { get; set; }

        /// <summary>
        /// 發行機構提前買回日
        /// </summary>
        [NullValues("", "NULL", null)]
        public string IssuerEarlyRedemptionDate { get; set; }

        /// <summary>
        /// 發行機構
        /// </summary>
        [NullValues("", "NULL", null)]
        public string IssuingInstitution { get; set; }


        /// <summary>
        /// 幣別代碼
        /// </summary>
        [NullValues("", "NULL", null)]
        public string CurrencyCode { get; set; }
    }
}