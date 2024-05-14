using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Models.USStock
{
    public class USStockModel
    {
        public Item Item { get; set; }
        public IList<USStock> USStockList { get; set; }
        public Dictionary<string, USStock> USStockDictionary { get; set; }
        public HtmlString USStockHtmlString { get; set; }
        public List<string> HotKeywordTags { get; set; }
        public List<string> HotProductTags { get; set; }
    }

    public class USStockDetailModel
    {
        public USStock USStock { get; set; }
    }

    public class USStock
    {
        public string FirstBankCode { get; set; }
        public string FundCode { get; set; }
        public string EnglishName { get; set; }
        public string ChineseName { get; set; }
        public string FullName { get; set; }
        public string DataDate { get; set; }
        public float? ClosingPrice { get; set; }
        public string ClosingPriceText { get; set; }
        public decimal? DailyReturn { get; set; }
        public decimal? WeeklyReturn { get; set; }
        public decimal? MonthlyReturn { get; set; }
        public decimal? ThreeMonthReturn { get; set; }
        public decimal? OneYearReturn { get; set; }
        public decimal? YeartoDateReturn { get; set; }
        public decimal? ChangePercentage { get; set; }
        public decimal? SixMonthReturn { get; set; }
        public string DetailLink { get; set; }
        public int ViewCount { get; set; }
        public string OnlineSubscriptionAvailability { get; set; }
        public List<string> HotKeywordTags { get; set; } = new List<string>();
        public List<string> HoProductTags { get; set; } = new List<string>();
        /// <summary>
        /// 同 FullName 但是是 Autocomplete 要用的
        /// </summary>
        public string value { get; set; }
        public bool IsLogin { get; set; }
        public bool IsLike { get; set; }
        //詳細頁用 MvcHtmlString
        public MvcHtmlString FocusButton { get; set; }
        public MvcHtmlString SubscribeButton { get; set; }
        //列表頁用 string
        public string FocusButtonHtml { get; set; }
        public string SubscribeButtonHtml { get; set; }
    }

    public struct Template
    {
        public readonly struct USStock
        {
            public static readonly ID Id = new ID("{FF4E9273-DCD5-4028-8F0B-D1D1993E3FD8}");

            public readonly struct Fields
            {
                public static readonly ID DetailLink = new ID("{E0C4643A-ED70-4F1E-B218-050AAF12F8B4}");
                public static readonly ID hotKeyword = new ID("{07A7EA84-24C9-4028-BE96-6374872CAA73}");
                public static readonly ID hotProduct = new ID("{D93B63C2-E682-4FB9-BB26-66B7EB04FBFF}");
            }
        }

        public readonly struct USStockTag
        {
            public static readonly ID Id = new ID("{944C4C0A-DDBD-4984-B2F6-D7D24EC769C3}");

            public readonly struct Fields
            {
                public static readonly ID CampaignTypeCode = new ID("{BC124AFC-E27F-420F-9C9F-368028551045}");
                public static readonly ID FirstBankCodeList = new ID("{AACA00D0-8DF6-4571-9792-7F5EF8556F01}");
            }
        }
    }
}