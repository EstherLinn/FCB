using Sitecore.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.FundDetail
{
    public class FundViewModel : FundDetailsRenderingModel
    {
        /// <summary>
        ///基金基本資料
        /// </summary>
        public FundBase FundBaseData { get; set; }

        /// 近一年淨值
        /// </summary>
        public List<FundCloseYearNetValue> FundCloseYearsNetValue { get; set; }

        /// <summary>
        /// 同類型排名
        /// </summary>
        public List<FundTypeRank> FundTypeRanks { get; set; }

        /// <summary>
        /// 近30天淨值
        /// </summary>
        public List<FundThiryDays> FundThiryDaysNetValue { get; set; }

        /// <summary>
        /// 報酬率
        /// </summary>
        public FundRateOfReturn FundRateOfReturn { get; set; }

        /// <summary>
        /// 近五年報酬率
        /// </summary>
        public List<FundAnnunalRateOfReturn> FundAnnunalRateOfReturn { get; set; }

        /// <summary>
        /// 累積報酬率
        /// </summary>
        public FundAccumulationRateOfReturn FundAccumulationRateOfReturn { get; set; }

        /// <summary>
        /// 道瓊指數
        /// </summary>
        public List<FundDowJonesIndex> FundDowJonesIndexs { get; set; }

        /// <summary>
        /// 產業持股狀況
        /// </summary>
        public List<FundStockHolding> FundAccordingStockHoldings { get; set; }

        /// <summary>
        /// 產業持股狀況
        /// </summary>
        public List<FundStockHolding> FundStockHoldings { get; set; }

        /// <summary>
        /// 區域持股狀況
        /// </summary>
        public List<FundStockHolding> FundStockAreaHoldings { get; set; }

        /// <summary>
        /// 前10大持股
        /// </summary>
        public List<FundStockHolding> FundTopTenStockHolding { get; set; }

        /// <summary>
        /// 風險指標
        /// </summary>
        public FundRiskindicators FundRiskindicators { get; set; }

        /// <summary>
        /// 同類型報酬率比較
        /// </summary>
        public FundReturnCompare FundReturnCompare { get; set; }

        /// <summary>
        /// 年報酬率比較
        /// </summary>
        public List<FundYearRateOfReturn> FundYearRateOfReturn { get; set; }

        /// <summary>
        /// 配息紀錄
        /// </summary>
        public List<FundDividendRecord> FundDividendRecords { get; set; }

        /// <summary>
        /// 規模變動
        /// </summary>
        public List<FundScaleRecord> FundScaleRecords { get; set; }

        /// <summary>
        /// 標籤
        /// </summary>
        public Dictionary<FundTagEnum, List<string>> TagsDic { get; set; }

        /// <summary>
        /// 公開說明書
        /// </summary>
        public string OpenDoc { get; set; }

        /// <summary>
        /// 財務報告書
        /// </summary>
        public string FinancialReportDoc { get; set; }

        /// <summary>
        /// 簡式公開說明書
        /// </summary>
        public string EasyOpenDoc { get; set; }

        /// <summary>
        /// 基金月報
        /// </summary>
        public string MonthReportDoc { get; set; }

        /// <summary>
        /// 投資人須知(專屬資訊)
        /// </summary>
        public string InvestExclusiveDoc { get; set; }

        /// <summary>
        /// 投資人須知(一般資訊)
        /// </summary>
        public string InvestNomnalDoc { get; set; }
    }

    public class FundDetailTemp
    {
        public string FundID { get; set; }
        public string Data { get; set; }
        public DateTime DataDate { get; set; }
    }
}
