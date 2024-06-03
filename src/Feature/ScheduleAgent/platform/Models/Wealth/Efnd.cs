using CsvHelper.Configuration.Attributes;
using FixedWidthParserWriter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    /// <summary>
    /// 基金現金配息檔，檔案名稱：EFND.txt
    /// </summary>
    public class Efnd : IFixedWidth
    {
        /// <summary>
        /// 投資標的代號
        /// </summary>
        [FixedWidthLineField(Start = 1, Length = 8)]
        public string INVEST_FUND_NO { get; set; }

        /// <summary>
        /// 撥款日
        /// </summary>
        [FixedWidthLineField(Start = 9, Length = 7)]
        public string DEPOSIT_DAY { get; set; }

        /// <summary>
        /// 配息基準日
        /// </summary>
        [FixedWidthLineField(Start = 16, Length = 7)]
        public string BASE_DAY { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        [FixedWidthLineField(Start = 23, Length = 3)]
        public string CURRENCY_TYPE { get; set; }

        /// <summary>
        /// 配息率
        /// </summary>
        private decimal? _dividend;
        [FixedWidthLineField(Start = 26, Length = 13)]
        public decimal? DIVIDEND
        {
            get => _dividend;
            set => _dividend = value / 1000000000;
        }

        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdateTime { get; set; }

        public Efnd()
        {
            UpdateTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public DefaultConfig GetDefaultConfig(int structureTypeId)
        {
            var defaultConfig = new DefaultConfig();

            return defaultConfig;

        }
    }
}
