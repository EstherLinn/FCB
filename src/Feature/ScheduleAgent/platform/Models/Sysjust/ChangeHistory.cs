using System;
using System.ComponentModel;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    public class ChangeHistory
    {
        public string FileName { get; set; }
        public DateTime ModificationDate { get; set; }
        public string ModificationType { get; set; }
        public string DataTable { get; set; }
        public int ModificationLine { get; set; }
        public double TotalSeconds { get; set; }
        public string Success { get; set; }
        public string ModificationID { get; set; }
        public int? TableCount { get; set; }
        public string TableCountConver { get; set; }
        public string ScheduleName { get; set; }
    }

    public enum ModificationID
    {
        [Description("最新資料")]
        最新資料 = 100,

        [Description("資料差異更新")]
        資料差異更新 = 101,

        [Description("最舊日期的那筆")]
        最舊日期的那筆 = 103,

        [Description("倉儲")]
        OdbcDone = 102,

        [Description("完成")]
        Done = 200,

        [Description("Error")]
        Error = 404
    }

    public enum ScheduleName
    {
        [Description("ETF 發行公司列表")]
        InsertCompanyEtf,

        [Description("境內基金公司列表")]
        InsertCompanyFund1,

        [Description("境外基金公司列表(國內總代理) ")]
        InsertCompanyFund2,

        [Description("境外基金發行公司與總代理公司資訊")]
        InsertCompanyFund3,

        [Description("境內基金公司-經營團隊")]
        InsertCompanyFund4,

        [Description("美元匯率資訊")]
        InsertExchangeRate,

        [Description("全球指數-區間報酬率")]
        InsertGlobalIndexRoi,

        [Description("股東背景 1 (境內基金)")]
        InsertShareholderFund1,

        [Description("股東背景2 (境內基金)")]
        InsertShareholderFund2,

        [Description("列表報酬資訊 ")]
        InsertUSStockList,

        [Description("排程執行紀錄表")]
        UpdateChangeHistory,

        [Description("ETF - 基本資料")]
        InsertBasicEtf,

        [Description("ETF - 基本資料 2")]
        InsertBasicEtf2,

        [Description("基金 – 基本資料 ")]
        InsertBasicFund,

        [Description("基金 – 基本資料 2")]
        InsertBasicFund2,

        [Description("配息資訊")]
        InsertDividendEtf,

        [Description("境內、外基金配息")]
        InsertDividendFund,

        [Description("規模變動(境外)")]
        InsertFundsizeEtf,

        [Description("規模變動(境外)")]
        InsertFundsizeFund1,

        [Description("規模變動(境內)")]
        InsertFundsizeFund2,

        [Description("全球指數列表")]
        InsertGlobalIndex,

        [Description("ETF歷史淨值")]
        InsertEtfNavHIS,

        [Description("基金歷史淨值 ")]
        InsertFundNavHIS,
        
        [Description("持股 - 依產業")]
        InsertHoldingEtf1,

        [Description("持股 - 依照區域 ")]
        InsertHoldingEtf2,

        [Description("持股-持股明細")]
        InsertHoldingEtf3,

        [Description("持股 - 投資比例彙總表")]
        InsertHoldingEtf4,

        [Description("境內基金  依持股類別 ")]
        InsertHoldingFund1,

        [Description("國內外基金-持股資料-依照區域 ")]
        InsertHoldingFund2,

        [Description("國內外基金-持股資料-依照產業")]
        InsertHoldingFund3,

        [Description("境外基金-持股資料-依照持股明細")]
        InsertHoldingFund4,

        [Description("境內基金-持股資料-依照持股明細 ")]
        InsertHoldingFund5,

        [Description("ETF – 淨值")]
        InsertNavEtf,

        [Description("ETF歷史淨值")]
        InsertNavEtfToHis,

        [Description("基金歷史淨值 ")]
        InsertNavFundToHis,

        [Description("基金 – 淨值")]
        InsertNavFund,

        [Description("各區間(市價/淨值)報酬率")]
        InsertReturnEtf,

        [Description("基準指數 /市價 (近五年)報酬")]
        InsertReturnEtf2,

        [Description("近一年各月(市價/指數漲跌幅)報酬")]
        InsertReturnEtf3,

        [Description("國內外基金 最新區間績效")]
        InsertReturnFund,

        [Description("近五年報酬率 ")]
        InsertReturnFund2,

        [Description("近一年各月報酬率")]
        InsertReturnFund3,

        [Description("年報酬率比較表 ")]
        InsertRiskEtf,

        [Description("ETF各期別績效評比")]
        InsertRiskEtf2,

        [Description("基金各期別績效評比 ")]
        InsertRiskFund,

        [Description("風險/報酬率比較表(境內)")]
        InsertRiskFund2Domestic,

        [Description("風險/報酬率比較表(境外) ")]
        InsertRiskFund2Oversea,

        [Description("年報酬率比較表(境內外)")]
        InsertRiskFund3,

        [Description("境內、外基金十年標準差(分布圖使用)")]
        InsertRiskFund4,

        [Description("債券歷史價格檔")]
        InsertBondHistoryPrice,

        [Description("債券商品檔")]
        InsertBondList,

        [Description("債券價格檔")]
        InsertBondNav,

        [Description("分行資料")]
        InsertBranchData,

        [Description("主機配息檔")]
        InsertEFND,

        [Description("ETF淨值及報酬資料檔")]
        InsertETF_NAV_TFJENAV,

        [Description("基金淨值及報酬資料檔")]
        InsertFUND_NAV_TFJSNAV,

        [Description("基金基本資料檔")]
        InsertFundBSC,

        [Description("產品淨值資料檔")]
        InsertFundEtf,

        [Description("十大主題貼標")]
        InsertFundList,

        [Description("基金規模資料檔")]
        InsertFundSize,

        [Description("基金通路報酬揭露資料檔")]
        InsertFundTrb,

        [Description("一銀人事資料檔")]
        InsertHRIS,

        [Description("國內基金商品資料檔")]
        InsertWms,

        [Description("專屬推薦-同血型")]
        InsertWmsAge,

        [Description("基金查無資料時顯示的推薦資料來源")]
        InsertWmsFocus,

        [Description("專屬推薦-同星座")]
        InsertWmsZodiac,

        [Description("一銀假日檔")]
        InsertImvpHoliday,

        [Description("好評基金")]
        InsertFundHighRated,

        [Description("查詢客戶六碼與身分證ID對應")]
        InsertCfmbsel,

        [Description("神機妙算投資法、金速配投資法")]
        InsertTFTFU_STG,

        [Description("查詢客戶的基本資料(風險屬性、理專AO代號)")]
        InsertCif,
    }
}