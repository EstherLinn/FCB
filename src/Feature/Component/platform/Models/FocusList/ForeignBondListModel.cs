
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.FocusList
{
    public class ForeignBondListModel 
    {
        // BondList


        /// <summary>
        /// 第一銀行債券代碼
        /// </summary>
        public string BondCode { get; set; }

        public string FullName { get; set; }
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
        /// 到期日 YYYYMMDD
        /// </summary>
        public string MaturityDate { get; set; }
        /// <summary>
        /// 停止申購日期 YYYYMMDD 有一些寫 29109999
        /// </summary>
        public string StopSubscriptionDate { get; set; }

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

        // BondNav
        /// <summary>
        /// 贖回價 20241212 改從 FUND_ETF 抓 BankSellPrice
        /// </summary>
        public decimal? RedemptionFee { get; set; }
        /// <summary>
        /// 申購價 20241212 改從 FUND_ETF 抓 BankBuyPrice
        /// </summary>
        public decimal? SubscriptionFee { get; set; }

        /// <summary>
        /// 日期 YYYYMMDD 20241212 改從 FUND_ETF 抓 PriceBaseDate
        /// </summary>
        public string Date { get; set; }
        // 歷史紀錄計算
        /// <summary>
        /// 漲跌月 (當天參考淨值價-對應上個月同一日參考申購價)/ 對應上個月同一日參考申購價x100%
        /// </summary>
        public decimal? UpsAndDownsMonth { get; set; }
        /// <summary>
        /// 漲跌季 (當天參考淨值價-對應上三個月同一日參考申購價)/ 對應上三個月同一日參考申購價x100%
        /// </summary>
        public decimal? UpsAndDownsSeason { get; set; }

        public string UpsAndDownsMonthStyle { get; set; } = string.Empty;
        /// <summary>
        /// 漲跌季 (當天參考淨值價-對應上三個月同一日參考申購價)/ 對應上三個月同一日參考申購價x100%
        /// </summary>
        public string UpsAndDownsSeasonStyle { get; set; } = string.Empty;
        /// <summary>
        /// 標籤
        /// </summary>
        public List<string> Tags { get; set; } = new List<string>();

        public Button Button { get; set; }

    }
}
