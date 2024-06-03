using CsvHelper.Configuration.Attributes;

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
        public string ProductCode { get; set; }

        /// <summary>
        /// 風險等級
        /// </summary>
        public string RiskLevel { get; set; }

        /// <summary>
        /// 商品種類
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        /// 計價幣別
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 定期定額
        /// </summary>
        public string RegularInvestmentPlan { get; set; }

        /// <summary>
        /// 連結標的
        /// </summary>
        public string UnderlyingAsset { get; set; }

        /// <summary>
        /// 網站連結
        /// </summary>
        public string WebsiteLink { get; set; }

        /// <summary>
        /// 期間
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        /// 目標客群
        /// </summary>
        public string TargetAudience { get; set; }

        /// <summary>
        /// 產品特色
        /// </summary>
        public string ProductFeatures { get; set; }

        /// <summary>
        /// 投資期間
        /// </summary>
        public string InvestmentPeriod { get; set; }

        /// <summary>
        /// 出版日期
        /// </summary>
        public string PublicationDate { get; set; }

        /// <summary>
        /// 商品形態
        /// </summary>
        public string ProductForm { get; set; }

        /// <summary>
        /// 配息頻率
        /// </summary>
        public string DividendDistributionFrequency { get; set; }

        /// <summary>
        /// 收益攤還
        /// </summary>
        public string IncomeAllocation { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 機構
        /// </summary>
        public string Institution { get; set; }

        /// <summary>
        /// 方案內容
        /// </summary>
        public string SchemeDetails { get; set; }

        /// <summary>
        /// 擔保
        /// </summary>
        public string Collateral { get; set; }

        /// <summary>
        /// ISIN Code
        /// </summary>
        public string ISINCode { get; set; }

        /// <summary>
        /// 已撤銷核備
        /// </summary>
        public string WithdrawnApproval { get; set; }

        /// <summary>
        /// 是否上架
        /// </summary>
        public string AvailabilityStatus { get; set; }

        /// <summary>
        /// 上架日期
        /// </summary>
        public string ListingDate { get; set; }

        /// <summary>
        /// 下架日期
        /// </summary>
        public string DelistingDate { get; set; }

        /// <summary>
        /// 最低單筆投資金額
        /// </summary>
        public string MinimumSingleInvestmentAmount { get; set; }

        /// <summary>
        /// 最低定期定額投資金額
        /// </summary>
        public string MinimumRegularInvestmentAmount { get; set; }

        /// <summary>
        /// 最低不定期定額投資金額
        /// </summary>
        public string MinimumIrregularInvestmentAmount { get; set; }

        /// <summary>
        /// 申購費用
        /// </summary>
        public string SubscriptionFee { get; set; }

        /// <summary>
        /// 赎回費用
        /// </summary>
        public string RedemptionFee { get; set; }

        /// <summary>
        /// 銀行相關說明
        /// </summary>
        public string BankRelatedInstructions { get; set; }

        /// <summary>
        /// 商品識別碼
        /// </summary>
        public string ProductIdentifier { get; set; }

        /// <summary>
        /// 是否可於網路申購
        /// </summary>
        public string OnlineSubscriptionAvailability { get; set; }

        /// <summary>
        /// 百元標的
        /// </summary>
        public string TargetName { get; set; }

        /// <summary>
        /// 申購淨值日
        /// </summary>
        public string SubscriptionNAVDate { get; set; }

        /// <summary>
        /// 赎回淨值日
        /// </summary>
        public string RedemptionNAVDate { get; set; }

        /// <summary>
        /// 贖回入款日
        /// </summary>
        public string RedemptionDepositDate { get; set; }

        /// <summary>
        /// 商品到期日
        /// </summary>
        public string ProductMaturityDate { get; set; }

        /// <summary>
        /// 發行機構提前買回日
        /// </summary>
        public string IssuerEarlyRedemptionDate { get; set; }

        /// <summary>
        /// 發行機構
        /// </summary>
        public string IssuingInstitution { get; set; }

        /// <summary>
        /// 幣別代碼
        /// </summary>
        public string CurrencyCode { get; set; }
    }
}