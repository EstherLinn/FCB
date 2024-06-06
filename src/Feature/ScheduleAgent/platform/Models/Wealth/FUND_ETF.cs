using CsvHelper.Configuration.Attributes;
using FixedWidthParserWriter;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    /// <summary>
    /// 產品淨值資料檔，檔案名稱：FUND_ETF.txt
    /// </summary>
    public class FundEtf : IFixedWidth
    {
        /// <summary>
        /// 產品識別碼
        /// </summary>
         [FixedWidthLineField(Start = 1, Length = 1)]
        public string ProductIdentifier { get; set; }

        /// <summary>
        /// 資料日期
        /// </summary>
        [FixedWidthLineField(Start = 3, Length = 7)]
        public string DataDate { get; set; }

        /// <summary>
        /// 銀行產品代號
        /// </summary>
        [FixedWidthLineField(Start = 11, Length = 4)]
        public string BankProductCode { get; set; }

        /// <summary>
        /// ETF幣別
        /// </summary>
        [FixedWidthLineField(Start = 16, Length = 3)]
        public string ETFCurrency { get; set; }

        /// <summary>
        /// 一銀買價
        /// </summary>
        [FixedWidthLineField(Start = 20, Length = 10)]
        public string BankBuyPrice { get; set; }

        /// <summary>
        /// 一銀賣價
        /// </summary>
        [FixedWidthLineField(Start = 31, Length = 10)]
        public string BankSellPrice { get; set; }

        /// <summary>
        /// 價格基準日
        /// </summary>
        [FixedWidthLineField(Start = 42, Length = 7)]
        public string PriceBaseDate { get; set; }

        /// <summary>
        /// 產品名稱
        /// </summary>
        [FixedWidthLineField(Start = 50, Length = 42)]
        public string ProductName { get; set; }
       

        public DefaultConfig GetDefaultConfig(int structureTypeId)
        {
            var defaultConfig = new DefaultConfig();

            return defaultConfig;

        }
    }
}