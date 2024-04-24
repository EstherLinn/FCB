using System;
using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Feature.Wealth.Component.Models.FundSearch
{
    public class FundItem
    {
        public string value { get; set; }
        public FundData data { get; set; }
    }

    public class FundData
    {
        /// <summary>
        /// autocomplete回傳內容
        /// </summary>
        public string Type { get; set; }
        public bool IsLogin { get; set; }
        public bool IsLike { get; set; }
        public string DetailUrl { get; set; }
        public bool Purchase { get; set; }
    }

    /// <summary>
    /// Dapper接資料
    /// </summary>
    public class FundSearchModel
    {
        /// <summary>
        /// 國內國外標示
        /// </summary>
        public string DomesticForeignFundIndicator { get; set; }

        /// <summary>
        /// 一銀產品代號
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 一銀產品名稱
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 淨值日期
        /// </summary>
        public DateTime? NetAssetValueDate { get; set; }

        /// <summary>
        /// 淨值日期顯示(yyyy/MM/dd)
        /// </summary>
        public string NetAssetValueDateFormat => NetAssetValueDate?.ToString("yyyy/MM/dd");

        /// <summary>
        /// 淨值
        /// </summary>
        public decimal? NetAssetValue { get; set; }

        /// <summary>
        /// 計價幣別 
        /// </summary>
        public string CurrencyName { get; set; }

        /// <summary>
        /// 計價幣別代碼-排序使用
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// 一個月報酬原幣-需加%
        /// </summary>
        public decimal? OneMonthReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 三個月報酬原幣-需加%
        /// </summary>
        public decimal? ThreeMonthReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 六個月報酬原幣-需加%
        /// </summary>
        public decimal? SixMonthReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 一年報酬原幣-需加%
        /// </summary>
        public decimal? OneYearReturnOriginalCurrency { get; set; }

        /// <summary>
        /// 一個月報酬台幣-需加%
        /// </summary>
        public decimal? OneMonthReturnTWD { get; set; }

        /// <summary>
        /// 三個月報酬台幣-需加%
        /// </summary>
        public decimal? ThreeMonthReturnTWD { get; set; }

        /// <summary>
        /// 六個月報酬台幣-需加%
        /// </summary>
        public decimal? SixMonthReturnTWD { get; set; }

        /// <summary>
        /// 一年報酬台幣-需加%
        /// </summary>
        public decimal? OneYearReturnTWD { get; set; }

        ///基本資料
        /// <summary>
        /// 基金規模幣別
        /// </summary>
        public string FundCurrency { get; set; }
        
        /// <summary>
        /// 基金規模幣別中文名
        /// </summary>
        public string FundCurrencyName { get; set; }

        /// <summary>
        /// 基金規模幣別ID-排序用
        /// </summary>
        public string FundCurrencyCode { get; set; }

        /// <summary>
        /// 漲跌幅-需加%
        /// </summary>
        public decimal? PercentageChangeInFundPrice { get; set; }

        /// <summary>
        /// 基金規模百萬原幣
        /// </summary>
        public decimal? FundSizeMillionOriginalCurrency { get; set; }

        /// <summary>
        /// 基金規模百萬台幣
        /// </summary>
        public decimal? FundSizeMillionTWD { get; set; }

        /// <summary>
        /// 基金類型
        /// </summary>
        public string FundType { get; set; }

        /// <summary>
        /// 配息頻率
        /// </summary>
        public string DividendFrequencyName { get; set; }

        ///風險指標
        /// <summary>
        /// 風險屬性
        /// </summary>
        public string RiskRewardLevel { get; set; }

        /// <summary>
        /// Sharpe
        /// </summary>
        public decimal? Sharpe { get; set; }

        /// <summary>
        /// Beta
        /// </summary>
        public decimal? Beta { get; set; }

        /// <summary>
        /// Alpha
        /// </summary>
        public decimal? OneYearAlpha { get; set; }

        /// <summary>
        /// 年化標準差
        /// </summary>
        public decimal? AnnualizedStandardDeviation { get; set; }

        /// <summary>
        /// 是否可於網路申購
        /// </summary>
        public string OnlineSubscriptionAvailability { get; set; }

        /// <summary>
        /// 百元標的
        /// </summary>
        public string TargetName { get; set; }

        /// <summary>
        /// 基金公司
        /// </summary>
        public string FundCompanyName { get; set; }

        /// <summary>
        /// 投資標的
        /// </summary>
        public string InvestmentTargetName { get; set; }

        /// <summary>
        /// 投資標的-排序用
        /// </summary>
        public string InvestmentTargetID { get; set; }

        /// <summary>
        /// 投資地區
        /// </summary>
        public string InvestmentRegionName { get; set; }

        /// <summary>
        /// 投資地區-排序用
        /// </summary>
        public string InvestmentRegionID { get; set; }

        /// <summary>
        /// 基金評等
        /// </summary>
        public string FundRating { get; set; }

        /// <summary>
        /// 基金評等
        /// </summary>
        public string FormatFundType { get; set; }
    }

    /// <summary>
    /// 渲染畫面用
    /// </summary>
    public class Funds
    {
        public string DomesticForeignFundIndicator { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string NetAssetValueDate { get; set; }

        public decimal? NetAssetValue { get; set; }

        public KeyValuePair<string, string> Currency { get; set; }
        public KeyValuePair<bool, decimal?> OneMonthReturnOriginalCurrency { get; set; }
        public KeyValuePair<bool, decimal?> ThreeMonthReturnOriginalCurrency { get; set; }
        public KeyValuePair<bool, decimal?> SixMonthReturnOriginalCurrency { get; set; }
        public KeyValuePair<bool, decimal?> OneYearReturnOriginalCurrency { get; set; }
        public KeyValuePair<bool, decimal?> OneMonthReturnTWD { get; set; }
        public KeyValuePair<bool, decimal?> ThreeMonthReturnTWD { get; set; }
        public KeyValuePair<bool, decimal?> SixMonthReturnTWD { get; set; }
        public KeyValuePair<bool, decimal?> OneYearReturnTWD { get; set; }

        public KeyValuePair<string, string> FundCurrency { get; set; }

        public decimal? FundSizeMillionOriginalCurrency { get; set; }

        public decimal? FundSizeMillionTWD { get; set; }

        public KeyValuePair<bool, decimal?> PercentageChangeInFundPrice { get; set; }

        public string FundType { get; set; }

        public string DividendFrequencyName { get; set; }

        public string RiskRewardLevel { get; set; }

        public decimal? Sharpe { get; set; }

        public decimal? Beta { get; set; }

        public decimal? OneYearAlpha { get; set; }

        public decimal? AnnualizedStandardDeviation { get; set; }

        public bool IsOnlineSubscriptionAvailability { get; set; }

        public List<string> Tags { get; set; }

        public string FundCompanyName { get; set; }

        public string InvestmentTargetName { get; set; }

        public List<string> InvestmentRegionName { get; set; }

        public string FundRating { get; set; }

        public string DetailUrl { get; set; }

        public string value { get; set; }

        public FundData Data { get; set; }

    }

    public class SearchBarData
    {
        /// <summary>
        /// 搜尋列計價幣別
        /// </summary>
        public List<string> Currencies { get; set; }

        /// <summary>
        /// 搜尋列基金公司
        /// </summary>
        public List<string> FundCompanies { get; set; }

        /// <summary>
        ///搜尋列投資區域 
        /// </summary>
        public List<string> InvestmentRegions { get; set; }

        /// <summary>
        /// 搜尋列投資標的
        /// </summary>
        public List<string> InvestmentTargets { get; set; }

        /// <summary>
        /// 搜尋列基金類型
        /// </summary>
        public List<string> FundTypeNames { get; set; }
    }

    public class Tags
    {
        public string TagName { get; set; }
        public List<string> ProductCodes { get; set; }
    }

    public struct Template
    {
        public struct FundSearch
        {
            public static readonly ID Id = new ID("{1B968584-1109-4CD2-96F9-0C084A86D001}");

            public struct Fields
            {
                /// <summary>
                /// 基金評等內容
                /// </summary>
                public static readonly string Content = "{587133C2-172C-4AA9-A7B5-44E1D7E41B34}";

                ///// <summary>
                /// Tag的名稱
                /// </summary>
                public static readonly string TagName = "{E5B26CC0-95C1-4C79-A141-FFDFA762434F}";

                /// <summary>
                /// 熱門關鍵字
                /// </summary>
                public static readonly string HotKeywordtags = "{CB6896B6-440A-43B4-9937-41E1DBF3D8BF}";

                /// <summary>
                /// 熱門商品
                /// </summary>
                public static readonly string HotProductags = "{B72C2E97-EB7B-48E3-9268-E9260E0BFE9A}";
            }
        }
        public struct FundTags
        {
            public struct Fields
            {
                /// <summary>
                /// tagFolder
                /// </summary>
                public static readonly string FundTags = "{9EC537F8-9EF9-4AF0-B6B6-A66749E3F22A}";

                /// <summary>
                /// 行銷標籤
                /// </summary>
                public static readonly string HotKeywordTag = "{D463F329-22C5-411D-870B-3679A142A6B9}";

                /// <summary>
                /// 熱銷標籤
                /// </summary>
                public static readonly string HotProductTag = "{DA395C80-5B52-4189-8FB4-1570317137AC}";

                /// <summary>
                /// ProductCodeList
                /// </summary>
                public static readonly string ProductCodeList = "{97F5D259-74DE-4B64-A4A9-EFAC9C0A0CD0}";
            }

        }

    }
   
}
