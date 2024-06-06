using Sitecore.Data;
using System.Collections.Generic;
using System.ComponentModel;

namespace Feature.Wealth.Component.Models.ETF.Detail
{
    public class EtfDetailModel : EtfDetailBase
    {
        /// <summary>
        /// ETF 基本資訊
        /// </summary>
        public EtfDetail BasicEtf { get; set; }

        /// <summary>
        /// ETF 地區
        /// </summary>
        public RegionType RegionType { get; set; }

        /// <summary>
        /// 瀏覽次數
        /// </summary>
        public string VisitCount { get; set; }

        /// <summary>
        /// 優惠標籤
        /// </summary>
        public string[] DiscountTags { get; set; }

        /// <summary>
        /// 分類標籤
        /// </summary>
        public string[] CategoryTags { get; set; }

        /// <summary>
        /// 近一年市價表現
        /// </summary>
        public List<EtfPriceHistory> ETFMarketPriceOverPastYear { get; set; }

        /// <summary>
        /// 近一年淨值表現
        /// </summary>
        public List<EtfPriceHistory> ETFNetWorthOverPastYear { get; set; }

        /// <summary>
        /// 同類ETF排行
        /// </summary>
        public List<EtfTypeRanking> ETFTypeRanks { get; set; }

        /// <summary>
        /// 近30日市價/淨值
        /// </summary>
        public List<EtfNav> ETFThiryDaysNav { get; set; }

        /// <summary>
        /// 近五年報酬率
        /// </summary>
        public List<EtfReferenceIndexAnnualReturn> ETFNetWorthAnnunalReturn { get; set; }

        /// <summary>
        /// 近一年各月報酬率
        /// </summary>
        public List<EtfReferenceIndexMonthlyReturn> ETFNetWorthMonthlyReturn { get; set; }

        /// <summary>
        /// 一銀買賣價
        /// </summary>
        public EtfTradingPrice ETFTradingPrice { get; set; }

        /// <summary>
        /// 近30日報價
        /// </summary>
        public List<EtfTradingPrice> ETFThiryDaysTradingPrice { get; set; }

        /// <summary>
        /// 前十大持股明細
        /// </summary>
        public List<EtfStrockHolding> ETFStockHoldings { get; set; }

        /// <summary>
        /// 風險指標
        /// </summary>
        public EtfRiskIndicator ETFRiskIndicator { get; set; }

        /// <summary>
        /// 年報酬率比較
        /// </summary>
        public List<EtfYearReturnCompare> ETFYearReturns { get; set; }

        /// <summary>
        /// 配息紀錄
        /// </summary>
        public Dictionary<int?, List<EtfDividendRecord>> ETFDividendRecords { get; set; }

        /// <summary>
        /// 規模變動
        /// </summary>
        public List<EtfScaleRecord> ETFScaleRecords { get; set; }
    }

    public enum RegionType
    {
        [Description("None")]
        None,

        /// <summary>
        /// 國內
        /// </summary>
        [Description("國內")]
        Domestic,

        /// <summary>
        /// 境外
        /// </summary>
        [Description("境外")]
        Overseas,

        /// <summary>
        /// 舊境外
        /// </summary>
        [Description("舊境外")]
        OverseasOld
    }

    public struct Templates
    {
        public struct EtfDetailDatasource
        {
            public static readonly ID Id = new ID("{436D4A29-5F41-40C6-84D0-03788EB8A742}");

            public struct Fields
            {
                public static readonly ID AccordionContent = new ID("{9DDAC5CF-9261-4E4D-8B55-736DD22467EB}");
                public static readonly ID RiskIntro = new ID("{8E2D292E-E116-4558-B879-E95ADCE8C875}");
                public static readonly ID RiskQuadrantChartDescription = new ID("{CD811ADC-043F-4506-8A8B-817096C0B7A6}");
                public static readonly ID DividendRecordDescription = new ID("{CDD6E76C-31B3-4F51-9C49-E5AB8D87143C}");
            }
        }
    }
}