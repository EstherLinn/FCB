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
        [Index(0)]
        public string ProductName { get; set; }

        /// <summary>
        /// 商品代碼
        /// </summary>
        [Index(1)]
        [NullValues("", "NULL", null)]
        public string ProductCode { get; set; }

        /// <summary>
        /// 風險等級
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string RiskLevel { get; set; }

        /// <summary>
        /// 商品種類
        /// </summary>
        [Index(3)]
        [NullValues("", "NULL", null)]
        public string ProductType { get; set; }

        /// <summary>
        /// 計價幣別
        /// </summary>
        [Index(4)]
        [NullValues("", "NULL", null)]
        public string Currency { get; set; }

        /// <summary>
        /// 定期定額
        /// </summary>
        [Index(5)]
        [NullValues("", "NULL", null)]
        public string RegularInvestmentPlan { get; set; }

        /// <summary>
        /// 連結標的
        /// </summary>
        [Index(6)]
        [NullValues("", "NULL", null)]
        public string UnderlyingAsset { get; set; }

        /// <summary>
        /// 網站連結
        /// </summary>
        [Index(7)]
        [NullValues("", "NULL", null)]
        public string WebsiteLink { get; set; }

        /// <summary>
        /// 期間
        /// </summary>
        [Index(8)]
        [NullValues("", "NULL", null)]
        public string Duration { get; set; }

        /// <summary>
        /// 目標客群
        /// </summary>
        [Index(9)]
        [NullValues("", "NULL", null)]
        public string TargetAudience { get; set; }

        /// <summary>
        /// 產品特色
        /// </summary>
        [Index(10)]
        [NullValues("", "NULL", null)]
        public string ProductFeatures { get; set; }

        /// <summary>
        /// 投資期間
        /// </summary>
        [Index(11)]
        [NullValues("", "NULL", null)]
        public string InvestmentPeriod { get; set; }

        /// <summary>
        /// 出版日期
        /// </summary>
        [Index(12)]
        [NullValues("", "NULL", null)]
        public string PublicationDate { get; set; }

        /// <summary>
        /// 商品形態
        /// </summary>
        [Index(13)]
        [NullValues("", "NULL", null)]
        public string ProductForm { get; set; }

        /// <summary>
        /// 配息頻率
        /// </summary>
        [Index(14)]
        [NullValues("", "NULL", null)]
        public string DividendDistributionFrequency { get; set; }

        /// <summary>
        /// 收益攤還
        /// </summary>
        [Index(15)]
        [NullValues("", "NULL", null)]
        public string IncomeAllocation { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        [Index(16)]
        [NullValues("", "NULL", null)]
        public string Amount { get; set; }

        /// <summary>
        /// 機構
        /// </summary>
        [Index(17)]
        [NullValues("", "NULL", null)]
        public string Institution { get; set; }

        /// <summary>
        /// 方案內容
        /// </summary>
        [Index(18)]
        [NullValues("", "NULL", null)]
        public string SchemeDetails { get; set; }

        /// <summary>
        /// 擔保
        /// </summary>
        [Index(19)]
        [NullValues("", "NULL", null)]
        public string Collateral { get; set; }

        /// <summary>
        /// ISIN Code
        /// </summary>
        [Index(20)]
        [NullValues("", "NULL", null)]
        public string ISINCode { get; set; }

        /// <summary>
        /// 已撤銷核備
        /// </summary>
        [Index(21)]
        [NullValues("", "NULL", null)]
        public string WithdrawnApproval { get; set; }

        /// <summary>
        /// 是否上架
        /// </summary>
        [Index(22)]
        [NullValues("", "NULL", null)]
        public string AvailabilityStatus { get; set; }

        /// <summary>
        /// 上架日期
        /// </summary>
        [Index(23)]
        [NullValues("", "NULL", null)]
        public string ListingDate { get; set; }

        /// <summary>
        /// 下架日期
        /// </summary>
        [Index(24)]
        [NullValues("", "NULL", null)]
        public string DelistingDate { get; set; }

        /// <summary>
        /// 最低單筆投資金額
        /// </summary>
        [Index(25)]
        [NullValues("", "NULL", null)]
        public string MinimumSingleInvestmentAmount { get; set; }

        /// <summary>
        /// 最低定期定額投資金額
        /// </summary>
        [Index(26)]
        [NullValues("", "NULL", null)]
        public string MinimumRegularInvestmentAmount { get; set; }

        /// <summary>
        /// 最低不定期定額投資金額
        /// </summary>
        [Index(27)]
        [NullValues("", "NULL", null)]
        public string MinimumIrregularInvestmentAmount { get; set; }

        /// <summary>
        /// 申購費用
        /// </summary>
        [Index(28)]
        [NullValues("", "NULL", null)]
        public string SubscriptionFee { get; set; }

        /// <summary>
        /// 赎回費用
        /// </summary>
        [Index(29)]
        [NullValues("", "NULL", null)]
        public string RedemptionFee { get; set; }

        /// <summary>
        /// 銀行相關說明
        /// </summary>
        [Index(30)]
        [NullValues("", "NULL", null)]
        public string BankRelatedInstructions { get; set; }

        /// <summary>
        /// 商品識別碼
        /// </summary>
        [Index(31)]
        [NullValues("", "NULL", null)]
        public string ProductIdentifier { get; set; }

        /// <summary>
        /// 是否可於網路申購
        /// </summary>
        [Index(32)]
        [NullValues("", "NULL", null)]
        public string OnlineSubscriptionAvailability { get; set; }

        /// <summary>
        /// 百元標的
        /// </summary>
        [Index(33)]
        [NullValues("", "NULL", null)]
        public string TargetName { get; set; }

        /// <summary>
        /// 申購淨值日
        /// </summary>
        [Index(34)]
        [NullValues("", "NULL", null)]
        public string SubscriptionNAVDate { get; set; }

        /// <summary>
        /// 赎回淨值日
        /// </summary>
        [Index(35)]
        [NullValues("", "NULL", null)]
        public string RedemptionNAVDate { get; set; }

        /// <summary>
        /// 贖回入款日
        /// </summary>
        [Index(36)]
        [NullValues("", "NULL", null)]
        public string RedemptionDepositDate { get; set; }

        /// <summary>
        /// 商品到期日
        /// </summary>
        [Index(37)]
        [NullValues("", "NULL", null)]
        public string ProductMaturityDate { get; set; }

        /// <summary>
        /// 發行機構提前買回日
        /// </summary>
        [Index(38)]
        [NullValues("", "NULL", null)]
        public string IssuerEarlyRedemptionDate { get; set; }

        /// <summary>
        /// 發行機構
        /// </summary>
        [Index(39)]
        [NullValues("", "NULL", null)]
        public string IssuingInstitution { get; set; }


        /// <summary>
        /// 幣別代碼
        /// </summary>
        [Index(40)]
        [NullValues("", "NULL", null)]
        public string CurrencyCode { get; set; }
    }
}