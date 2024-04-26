using Feature.Wealth.Component.Models.GlobalIndex;
using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.GlobalIndex
{
    public class GlobalIndexModel
    {
        public Item Item { get; set; }
        public string DetailLink { get; set; }
        public IList<GlobalIndex> GlobalIndexList { get; set; }
        public Dictionary<string, GlobalIndex> GlobalIndexDictionary { get; set; }
        public string GlobalIndexHighchartsDataJson { get; set; }
    }

    public class GlobalIndex
    {
        public string IndexCode { get; set; }
        public string IndexName { get; set; }
        public string IndexCategoryID { get; set; }
        public string IndexCategoryName { get; set; }
        public string DataDate { get; set; }
        public string MarketPrice { get; set; }
        public string MarketPriceText { get; set; }
        public string Change { get; set; }
        public string ChangePercentage { get; set; }
        public bool UpOrDown { get; set; }
        public IList<GlobalIndex> GlobalIndexHistory { get; set; }
        public string DetailLink { get; set; }
    }

    public class GlobalIndexHighchartsData
    {
        public string IndexCode { get; set; }
        public List<float> Data { get; set; }
    }

    public class GlobalIndexDetailModel
    {
        public string DetailLink { get; set; }
        public string FundLink { get; set; }
        public string ETFLink { get; set; }
        public GlobalIndexDetail GlobalIndexDetail { get; set; }
        public string RelevantGlobalIndex { get; set; } = "[]";
        public string RelevantGlobalIndex_Success { get; set; } = "Success";
        public string RelevantFund { get; set; } = "[]";
        public string RelevantFund_Success { get; set; } = "Success";
        public string RelevantETF { get; set; } = "[]";
        public string RelevantETF_Success { get; set; } = "Success";
        public string PriceData_D { get; set; } = "[]";
        public string PriceData_D_Success { get; set; } = "Success";
        public string PriceData_W { get; set; } = "[]";
        public string PriceData_W_Success { get; set; } = "Success";
        public string PriceData_M { get; set; } = "[]";
        public string PriceData_M_Success { get; set; } = "Success";
    }

    public class GlobalIndexDetail
    {
        public string IndexCode { get; set; }
        public string IndexName { get; set; }
        public string IndexCategoryID { get; set; }
        public string IndexCategoryName { get; set; }
        public string DataDate { get; set; }
        public string MarketPrice { get; set; }
        public string MarketPriceText { get; set; }
        public string Change { get; set; }
        public string ChangePercentage { get; set; }
        public bool UpOrDown { get; set; }
        public int ViewCount { get; set; }
        public string DailyReturn { get; set; }
        public string WeeklyReturn { get; set; }
        public string OneMonthReturn { get; set; }
        public string ThreeMonthReturn { get; set; }
        public string SixMonthReturn { get; set; }
        public string YeartoDateReturn { get; set; }
        public string OneYearReturn { get; set; }
        public string ThreeYearReturn { get; set; }
    }

    public class RelevantInformation
    {
        public string id { get; set; }
        public string name { get; set; }
        public decimal? d1 { get; set; }
        public decimal? d7 { get; set; }
        public decimal? m1 { get; set; }
        public decimal? m3 { get; set; }
        public decimal? m6 { get; set; }
        public decimal? ytd { get; set; }
        public decimal? y1 { get; set; }
        public decimal? y3 { get; set; }
        public decimal? y5 { get; set; }
        public string djid { get; set; }
        public string DetailLink { get; set; }
    }

    public class PriceData
    {
        /// <summary>
        /// 要使用 javascript 的時間
        /// </summary>
        public double date { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }
        public double value { get; set; }
    }

    public struct Template
    {
        public readonly struct GlobalIndex
        {
            public static readonly ID Id = new ID("{B54DB03A-8CB6-45C7-ACCB-F84B87593659}");

            public readonly struct Fields
            {
                public static readonly ID DetailLink = new ID("{5F5A949F-D1E7-4186-B33E-2F18D9F0EDB6}");
            }
        }

        public readonly struct GlobalIndexDetail
        {
            public static readonly ID Id = new ID("{044BF03D-629E-42F3-9407-FD34CF23E450}");

            public readonly struct Fields
            {
                public static readonly ID DetailLink = new ID("{819DB951-2F86-4E4C-9952-8CC676742E4B}");
                // 這個因為基金有給連結方法所以沒用了，但先留著
                public static readonly ID FundLink = new ID("{982A7102-E6CE-4C16-8E93-3D1756C36004}");
                public static readonly ID ETFLink = new ID("{0C0C2892-7BC4-4F02-B5BB-E13714368312}");
            }
        }
    }

    public enum RelevantInformationType
    {
        GlobalIndex = 1,
        Fund = 2,
        ETF = 3
    }
}