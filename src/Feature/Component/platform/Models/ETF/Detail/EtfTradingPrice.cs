using System;

namespace Feature.Wealth.Component.Models.ETF.Detail
{
    public class EtfTradingPrice
    {
        /// <summary>
        /// 銀行產品代號
        /// </summary>
        public string BankProductCode { get; set; }

        /// <summary>
        /// ETF幣別
        /// </summary>
        public string ETFCurrency { get; set; }

        /// <summary>
        /// 價格基準日
        /// </summary>
        public DateTime? PriceBaseDate { get; set; }

        /// <summary>
        /// 產品名稱
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 資料日期
        /// </summary>
        public string DataDate { get; set; }

        /// <summary>
        /// 前一筆價格基準日
        /// </summary>
        public DateTime? PreviousPriceBaseDate { get; set; }

        #region 買價

        /// <summary>
        /// 一銀買價
        /// </summary>
        public decimal? BankBuyPrice { get; set; }

        /// <summary>
        /// 前一筆買價
        /// </summary>
        public decimal? PreviousBuyPrice { get; set; }

        /// <summary>
        /// 買價 漲跌
        /// </summary>
        public decimal? BuyPriceChange { get; set; }

        /// <summary>
        /// 買價 漲跌幅
        /// </summary>
        public decimal? BuyPriceChangePercentage { get; set; }

        /// <summary>
        /// 最高買價(年)
        /// </summary>
        public decimal? MaxBuyPrice { get; set; }

        /// <summary>
        /// 最低買價(年)
        /// </summary>
        public decimal? MinBuyPrice { get; set; }

        /// <summary>
        /// 買價漲跌樣式
        /// </summary>
        public string BuyPriceChangeStyle { get; set; }

        /// <summary>
        /// 買價漲跌幅樣式
        /// </summary>
        public string BuyPriceChangePercentageStyle { get; set; }

        #endregion 買價

        #region 賣價

        /// <summary>
        /// 一銀賣價
        /// </summary>
        public decimal? BankSellPrice { get; set; }

        /// <summary>
        /// 前一筆賣價
        /// </summary>
        public decimal? PreviousSellPrice { get; set; }

        /// <summary>
        /// 賣價 漲跌
        /// </summary>
        public decimal? SellPriceChange { get; set; }

        /// <summary>
        /// 賣價 漲跌幅
        /// </summary>
        public decimal? SellPriceChangePercentage { get; set; }

        /// <summary>
        /// 最高賣價(年)
        /// </summary>
        public decimal? MaxSellPrice { get; set; }

        /// <summary>
        /// 最低賣價(年)
        /// </summary>
        public decimal? MinSellPrice { get; set; }

        /// <summary>
        /// 賣價漲跌樣式
        /// </summary>
        public string SellPriceChangeStyle { get; set; }

        /// <summary>
        /// 賣價漲跌幅樣式
        /// </summary>
        public string SellPriceChangePercentageStyle { get; set; }

        #endregion 賣價
    }
}