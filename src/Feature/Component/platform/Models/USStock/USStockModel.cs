﻿using Sitecore.Data;
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
        public Item Item { get; set; }
        public string PageID { get; set; }
        public USStock USStock { get; set; }
        public string b2brwdDomain { get; set; }
        public string SearchUrl { get; set; } = USStockRelatedLinkSetting.GetUSStockSearchUrl();
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
        public string AvailabilityStatus { get; set; }
        public List<string> HotKeywordTags { get; set; } = new List<string>();
        public List<string> HotProductTags { get; set; } = new List<string>();
        public List<string> Discount { get; set; } = new List<string>();
        /// <summary>
        /// Autocomplete 要用的 FundCode + FullName
        /// </summary>
        public string value { get; set; }
        public bool IsLogin { get; set; }
        public bool IsLike { get; set; }
        //詳細頁用 MvcHtmlString
        public MvcHtmlString FocusButton { get; set; }
        public MvcHtmlString SubscribeButton { get; set; }
        //列表頁列表用 string
        public string FocusButtonHtml { get; set; }
        public string SubscribeButtonHtml { get; set; }
        //列表頁自動完成用 string
        public string AutoFocusButtonHtml { get; set; }
        public string AutoSubscribeButtonHtml { get; set; }
    }

    public struct Template
    {
        public readonly struct USStock
        {
            public static readonly ID Id = new ID("{FF4E9273-DCD5-4028-8F0B-D1D1993E3FD8}");

            public readonly struct Fields
            {
                public static readonly ID DetailLink = new ID("{E0C4643A-ED70-4F1E-B218-050AAF12F8B4}");
                public static readonly ID HotKeyword = new ID("{07A7EA84-24C9-4028-BE96-6374872CAA73}");
                public static readonly ID HotProduct = new ID("{D93B63C2-E682-4FB9-BB26-66B7EB04FBFF}");
            }
        }

        public readonly struct USStockTag
        {
            public static readonly ID Id = new ID("{944C4C0A-DDBD-4984-B2F6-D7D24EC769C3}");

            public readonly struct Fields
            {
                public static readonly ID TagName = new ID("{BC124AFC-E27F-420F-9C9F-368028551045}");
                public static readonly ID ProductCodeList = new ID("{AACA00D0-8DF6-4571-9792-7F5EF8556F01}");
            }
        }

        public readonly struct USStockDetail
        {
            public static readonly ID Id = new ID("{23CB2BBD-8A9F-4529-8847-B8F64DF39982}");

            public readonly struct Fields
            {
                public static readonly ID b2brwdDomain = new ID("{845DB51B-8483-4D03-83BC-767649F4A5FB}");
            }
        }

        public readonly struct USStockTagFolder
        {
            public static readonly ID Id = new ID("{11080F3C-C242-450E-B4FA-73DE8E2E5AAF}");

            public readonly struct Children
            {
                public static readonly ID HotKeywordTag = new ID("{47E526C1-E041-4C90-82F5-713AD5ECA6B1}");
                public static readonly ID HotProductTag = new ID("{134A1E87-CAEF-4470-B619-662DA3CA1416}");
                public static readonly ID Discount = new ID("{AFEACB28-F841-47B6-8BA8-867988633D41}");
            }
        }

        public readonly struct TagsType
        {
            public static readonly ID HotKeywordTag = new ID("{AEB37529-AD2E-4A15-B1B8-326EACB58954}");
            public static readonly ID HotProductTag = new ID("{EB96F527-C17A-4462-9D13-4136D6EB9084}");
            public static readonly ID Discount = new ID("{339563FD-22BC-441D-B171-472E6FB085CE}");
        }

        public readonly struct TagFolder
        {
            public static readonly ID Id = new ID("{E097EA37-5FD5-4CC3-BC85-D50FFA3FAD8B}");

            public readonly struct Fields
            {
                public static readonly ID TagType = new ID("{B721EF77-056E-4D8F-836D-BCA0B59ABC11}");
            }
        }
    }
}