using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// ETF歷史淨值，檔案名稱：SYSJUST-ETFNAV-HIS.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustEtfNavHis
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [NullValues("", "NULL", null)]
        public string Date { get; set; }

        /// <summary>
        /// 市價
        /// </summary>
        [NullValues("", "NULL", null)]
        public string MarketPrice { get; set; }

        /// <summary>
        /// 淨值日期
        /// </summary>
        [NullValues("", "NULL", null)]
        public string NetAssetValue { get; set; }
    }
}
