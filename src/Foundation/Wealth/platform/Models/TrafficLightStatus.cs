using System;
using System.ComponentModel;

namespace Foundation.Wealth.Models
{
    public class SignalStatus
    {
        public int Number { get; set; }
        public string SignalName { get; set; }
        public TrafficLightStatus Status { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    public enum TrafficLightStatus
    {
        Red = 0,
        Green = 1
    }


    public enum NameofTrafficLight
    {
        [Description("Sysjust_Basic_ETF")]
        Sysjust_Basic_ETF = 1,

        [Description("Sysjust_Basic_ETF_2")]
        Sysjust_Basic_ETF_2 = 2,

        [Description("Sysjust_Basic_Fund")]
        Sysjust_Basic_Fund = 3,

        [Description("Sysjust_Basic_Fund_2")]
        Sysjust_Basic_Fund_2 = 4,

        [Description("Sysjust_Company_ETF")]
        Sysjust_Company_ETF = 5,

        [Description("Sysjust_Company_fund_1")]
        Sysjust_Company_fund_1 = 6,

        [Description("Sysjust_Company_fund_2")]
        Sysjust_Company_fund_2 = 7,

        [Description("Sysjust_Company_fund_3")]
        Sysjust_Company_fund_3 = 8,

        [Description("Sysjust_Company_fund_4")]
        Sysjust_Company_fund_4 = 9,

        [Description("Sysjust_Dividend_ETF")]
        Sysjust_Dividend_ETF = 10,

        [Description("Sysjust_Dividend_Fund")]
        Sysjust_Dividend_Fund = 11,

        [Description("Sysjust_ExchangeRate")]
        Sysjust_ExchangeRate = 12,

        [Description("Sysjust_Fundsize_ETF")]
        Sysjust_Fundsize_ETF = 13,

        [Description("Sysjust_Fundsize_Fund_1")]
        Sysjust_Fundsize_Fund_1 = 14,

        [Description("Sysjust_Fundsize_Fund_2")]
        Sysjust_Fundsize_Fund_2 = 15,

        [Description("Sysjust_GlobalIndex")]
        Sysjust_GlobalIndex = 16,

        [Description("Sysjust_GlobalIndex_ROI")]
        Sysjust_GlobalIndex_ROI = 17,

        [Description("Sysjust_Holding_ETF")]
        Sysjust_Holding_ETF = 18,

        [Description("Sysjust_Holding_ETF_2")]
        Sysjust_Holding_ETF_2 = 19,

        [Description("Sysjust_Holding_ETF_3")]
        Sysjust_Holding_ETF_3 = 20,

        [Description("Sysjust_Holding_ETF_4")]
        Sysjust_Holding_ETF_4 = 21,

        [Description("Sysjust_Holding_Fund_1")]
        Sysjust_Holding_Fund_1 = 22,

        [Description("Sysjust_Holding_Fund_2")]
        Sysjust_Holding_Fund_2 = 23,

        [Description("Sysjust_Holding_Fund_3")]
        Sysjust_Holding_Fund_3 = 24,

        [Description("Sysjust_Holding_Fund_4")]
        Sysjust_Holding_Fund_4 = 25,

        [Description("Sysjust_Holding_Fund_5")]
        Sysjust_Holding_Fund_5 = 26,

        [Description("Sysjust_Nav_ETF")]
        Sysjust_Nav_ETF = 27,

        [Description("Sysjust_ETFNAV_HIS")]
        Sysjust_ETFNAV_HIS = 28,

        [Description("Sysjust_Nav_Fund")]
        Sysjust_Nav_Fund = 29,

        [Description("Sysjust_FUNDNAV_HIS")]
        Sysjust_FUNDNAV_HIS = 30,

        [Description("Sysjust_Return_ETF")]
        Sysjust_Return_ETF = 31,

        [Description("Sysjust_Return_ETF_2")]
        Sysjust_Return_ETF_2 = 32,

        [Description("Sysjust_Return_ETF_3")]
        Sysjust_Return_ETF_3 = 33,

        [Description("Sysjust_Return_Fund")]
        Sysjust_Return_Fund = 34,

        [Description("Sysjust_Return_Fund_2")]
        Sysjust_Return_Fund_2 = 35,

        [Description("Sysjust_Return_Fund_3")]
        Sysjust_Return_Fund_3 = 36,

        [Description("Sysjust_Risk_ETF")]
        Sysjust_Risk_ETF = 37,

        [Description("Sysjust_Risk_ETF_2")]
        Sysjust_Risk_ETF_2 = 38,

        [Description("Sysjust_Risk_Fund")]
        Sysjust_Risk_Fund = 39,

        [Description("Sysjust_Risk_Fund_2_Domestic")]
        Sysjust_Risk_Fund_2_Domestic = 40,

        [Description("Sysjust_Risk_Fund_2_Oversea")]
        Sysjust_Risk_Fund_2_Oversea = 41,

        [Description("Sysjust_Risk_Fund_3")]
        Sysjust_Risk_Fund_3 = 42,

        [Description("Sysjust_Risk_Fund_4")]
        Sysjust_Risk_Fund_4 = 43,

        [Description("Sysjust_Shareholder_fund_1")]
        Sysjust_Shareholder_fund_1 = 44,

        [Description("Sysjust_Shareholder_fund_2")]
        Sysjust_Shareholder_fund_2 = 45,

        [Description("Sysjust_USStockList")]
        Sysjust_USStockList = 46,

        [Description("BondList")]
        BondList = 47,

        [Description("BondNav")]
        BondNav = 48,

        [Description("Branch_Data")]
        Branch_Data = 49,

        [Description("EFND")]
        EFND = 50,

        [Description("FUND_BSC")]
        FUND_BSC = 51,

        [Description("FUND_ETF")]
        FUND_ETF = 52,

        [Description("Fund_HighRated")]
        Fund_HighRated = 53,

        [Description("FundTagsList")]
        FundTagsList = 54,

        [Description("FUND_SIZE")]
        FUND_SIZE = 55,

        [Description("FUNDTRB")]
        FUNDTRB = 56,

        [Description("HRIS")]
        HRIS = 57,

        [Description("ETF_NAV_TFJENAV")]
        ETF_NAV_TFJENAV = 58,

        [Description("FUND_NAV_TFJSNAV")]
        FUND_NAV_TFJSNAV = 59,

        [Description("WMS_DOC_RECM")]
        WMS_DOC_RECM = 60,

        [Description("Wms_age_profile_d_mf")]
        Wms_age_profile_d_mf = 61,

        [Description("Wms_zodiac_profile_d_mf")]
        Wms_zodiac_profile_d_mf = 62,

        [Description("TFTFU_STG")]
        TFTFU_STG = 63,
    }
}
