using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Models.Bond
{
    public class BondModel
    {
        public Item Item { get; set; }
        public IList<Bond> BondList { get; set; } = new List<Bond>();
        public HtmlString BondHtmlString { get; set; }
        public List<string> HotKeywordTags { get; set; }
        public List<string> HotProductTags { get; set; }
        /// <summary>
        /// 計價幣別
        /// </summary>
        public IEnumerable<string> CurrencyList { get; set; }
        /// <summary>
        /// 配息頻率
        /// </summary>
        public IEnumerable<string> PaymentFrequencyList { get; set; }
        /// <summary>
        /// 發行機構
        /// </summary>
        public IEnumerable<string> IssuerList { get; set; }
    }

    public class BondDetailModel
    {
        public Bond BondDetail { get; set; }
    }

    public class Bond
    {
        // BondList

        /// <summary>
        /// 第一銀行債券代碼
        /// </summary>
        public string BondCode { get; set; }
        /// <summary>
        /// ISIN code
        /// </summary>
        public string ISINCode { get; set; }
        /// <summary>
        /// 第一銀行債券名稱
        /// </summary>
        public string BondName { get; set; }
        /// <summary>
        /// 計價幣別英文代碼
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// 計價幣別代碼
        /// </summary>
        public string CurrencyCode { get; set; }
        /// <summary>
        /// 計價幣別名稱
        /// </summary>
        public string CurrencyName { get; set; }
        /// <summary>
        /// 票面利率
        /// </summary>
        public decimal? InterestRate { get; set; }
        /// <summary>
        /// 配息頻率
        /// 0：零息
        /// 1：月
        /// 2：季
        /// 3：半年
        /// 4：年
        /// </summary>
        public int PaymentFrequency { get; set; }
        /// <summary>
        /// 配息頻率
        /// 0：零息
        /// 1：月
        /// 2：季
        /// 3：半年
        /// 4：年
        /// </summary>
        public string PaymentFrequencyName { get; set; }
        /// <summary>
        /// 風險等級
        /// RR1
        /// RR2
        /// RR3
        /// RR4
        /// RR5
        /// </summary>
        public string RiskLevel { get; set; }
        /// <summary>
        /// 銷售對象
        /// N-無限制
        /// Y-限專業投資人
        /// A-限高資產客戶
        /// </summary>
        public string SalesTarget { get; set; }
        /// <summary>
        /// 最低申購金額(外幣)
        /// </summary>
        public int MinSubscriptionForeign { get; set; }
        /// <summary>
        /// 最低申購金額(台幣)
        /// </summary>
        public int MinSubscriptionNTD { get; set; }
        /// <summary>
        /// 最小累進金額
        /// 抓主機T902投資金額計算單位欄
        /// 0：1
        /// 1：1000
        /// 2：10000 
        /// 3：50000
        /// 4：100000 
        /// 5：500000
        /// 6：1000000 
        /// 7：2000 
        /// 8：5000
        /// 9：30000000
        /// A：20000
        /// B：200000
        /// C：10000000
        /// </summary>
        public string MinIncrementAmount { get; set; }
        /// <summary>
        /// 到期日 YYYYMMDD
        /// </summary>
        public string MaturityDate { get; set; }
        /// <summary>
        /// 到期日-年
        /// </summary>
        public decimal? MaturityYear { get; set; }
        /// <summary>
        /// 停止申購日期 YYYYMMDD 有一些寫 29109999
        /// </summary>
        public string StopSubscriptionDate { get; set; }
        /// <summary>
        /// 發行機構提前買回日 YYYYMMDD
        /// </summary>
        public string RedemptionDateByIssuer { get; set; }
        /// <summary>
        /// 發行機構
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 開放個網
        /// </summary>
        public string OpenToPublic { get; set; }
        /// <summary>
        /// 是否上架
        /// 國外債判斷(與結構型商品、結構型金融債一樣)
        /// 1.DUE_DAY&TFTFU_BOND_CALL_DAY<=SYSDATE :N
        /// 2.上架日期 > SYSDATE : N 若沒特別輸入日期一樣為Y
        /// 3.下架日期<=SYSDATE : N 若沒特別輸入日期一樣為Y
        /// </summary>
        public string Listed { get; set; }
        /// <summary>
        /// 上架日期  YYYYMMDD
        /// </summary>
        public string ListingDate { get; set; }
        /// <summary>
        /// 下架日期  YYYYMMDD
        /// </summary>
        public string DelistingDate { get; set; }

        // BondNav

        /// <summary>
        /// 申購價
        /// </summary>
        public decimal? RedemptionFee { get; set; }
        /// <summary>
        /// 贖回價
        /// </summary>
        public decimal? SubscriptionFee { get; set; }
        /// <summary>
        /// 日期 YYYYMMDD
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 保留欄位
        /// </summary>
        public string ReservedColumn { get; set; }
        /// <summary>
        /// 註記
        /// A：新增-->之前沒有提供過資料，新增到資料庫
        /// D：刪除-->之前有提供過資料，將此筆資料從資料庫刪除
        /// G：現有資料-->已經提供過相同的資料，是要比對相同的商品代號與日期，更新幣別/贖回價/申購價欄位
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 前手息
        /// </summary>
        public decimal? PreviousInterest { get; set; }
        /// <summary>
        /// S&P信評
        /// </summary>
        public string SPCreditRating { get; set; }
        /// <summary>
        /// Moody信評
        /// </summary>
        public string MoodyCreditRating { get; set; }
        /// <summary>
        /// Fitch信評
        /// </summary>
        public string FitchCreditRating { get; set; }
        /// <summary>
        /// 殖利率YTM-不含前手息
        /// </summary>
        public decimal? YieldRateYTM { get; set; }
        /// <summary>
        /// 漲跌月 (當天參考淨值價-對應上個月同一日參考申購價)/ 對應上個月同一日參考申購價x100%
        /// </summary>
        public decimal? UpsAndDownsMonth { get; set; }
        /// <summary>
        /// 漲跌季 (當天參考淨值價-對應上三個月同一日參考申購價)/ 對應上三個月同一日參考申購價x100%
        /// </summary>
        public decimal? UpsAndDownsSeason { get; set; }

        public string DetailLink { get; set; }

        /// <summary>
        /// 熱門關鍵字
        /// </summary>
        public List<string> HotKeywordTags { get; set; } = new List<string>();
        /// <summary>
        /// 熱門商品
        /// </summary>
        public List<string> HotProductTags { get; set; } = new List<string>();
        /// <summary>
        /// 優惠標籤
        /// </summary>
        public List<string> Discount { get; set; } = new List<string>();

        /// <summary>
        /// 全名 列表顯示用
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 同 FullName 但是是 Autocomplete 要用的
        /// </summary>
        public string value { get; set; }

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

    public class BondHistoryPrice
    {
        /// <summary>
        /// 商品代號
        /// </summary>
        public string BondCode { get; set; }
        /// <summary>
        /// 幣別
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// 申購價
        /// </summary>
        public decimal? RedemptionFee { get; set; }
        /// <summary>
        /// 贖回價
        /// </summary>
        public decimal? SubscriptionFee { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 商品名稱
        /// </summary>
        public string BondName { get; set; }
    }

    public struct Template
    {
        public readonly struct Bond
        {
            public static readonly ID Id = new ID("{4EBA49B4-047A-4836-A5F6-C07D66E06028}");

            public readonly struct Fields
            {
                public static readonly ID HotKeyword = new ID("{13FF7E0F-AFE2-45E6-B265-6703BD324EA1}");
                public static readonly ID HotProduct = new ID("{A46549F2-B481-432E-9DF4-605377DCAE3A}");
            }
        }

        public readonly struct BondTag
        {
            public static readonly ID Id = new ID("{B5DAC02A-E857-4354-9E23-BCA6B77022BA}");

            public readonly struct Fields
            {
                public static readonly ID TagName = new ID("{5774D701-A5B5-420D-9F7C-B0A9FAC338D5}");
                public static readonly ID ProductCodeList = new ID("{FFAAA195-BB28-4578-B1C8-A3F32C1F13A0}");
            }
        }

        public readonly struct BondTagFolder
        {
            public static readonly ID Id = new ID("{9E562CE9-C4A5-49D8-833C-66C966144317}");

            public readonly struct Children
            {
                public static readonly ID HotKeywordTag = new ID("{DA5ACD7D-812A-4A64-AAE1-AB05997AAA53}");
                public static readonly ID HotProductTag = new ID("{AE7A5294-319F-42FC-861F-C15E3F1A4D76}");
                public static readonly ID Discount = new ID("{525A6666-F8B6-432A-92C3-6F697700E415}");
            }
        }

        public readonly struct TagsType
        {
            public static readonly ID HotKeywordTag = new ID("{EE30339F-1522-48F2-B13C-7F4B156DBBBE}");
            public static readonly ID HotProductTag = new ID("{4FFDFC3B-B1FC-490D-896E-D8B58F55878B}");
            public static readonly ID Discount = new ID("{E8BD65A5-9345-401E-971A-07E789A83411}");
        }

        public readonly struct TagFolder
        {
            public static readonly ID Id = new ID("{CCC1971C-BC26-41C8-9B37-17136D3E3CF1}");

            public readonly struct Fields
            {
                public static readonly ID TagType = new ID("{DAF9A595-54E6-4BA2-961B-FFD3004F9274}");
            }
        }
    }
}
