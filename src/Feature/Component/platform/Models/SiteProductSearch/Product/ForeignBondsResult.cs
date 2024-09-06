namespace Feature.Wealth.Component.Models.SiteProductSearch.Product
{
    public class ForeignBondsResult
    {
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

        public int MinIncrementAmountNumber { get; set; }

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

        /// <summary>
        /// 申購價
        /// </summary>
        public decimal? SubscriptionFee { get; set; }

        /// <summary>
        /// 贖回價
        /// </summary>
        public decimal? RedemptionFee { get; set; }

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

        /// <summary>
        /// 關注按鈕 HTML
        /// </summary>
        public string FocusButtonHtml { get; set; }

        /// <summary>
        /// 申購按鈕 HTML
        /// </summary>
        public string SubscribeButtonHtml { get; set; }

        /// <summary>
        /// Autocomplete 申購按鈕 HTML
        /// </summary>
        public string SubscribeButtonAutoHtml { get; set; }

        /// <summary>
        /// Autocomplete 關注按鈕 HTML
        /// </summary>
        public string FocusButtonAutoHtml { get; set; }
    }
}