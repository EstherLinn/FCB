using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 規模變動(境內)，檔案名稱：Sysjust-FundSize-Fund-2.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    public class SysjustFundSizeFund2
    {
        /// <summary>
        /// 一銀代碼
        /// </summary>
        [Index(0)]
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 嘉實代碼
        /// </summary>
        [Index(1)]
        public string SysjustCode { get; set; }

        /// <summary>
        /// 規模日期
        /// </summary>
        [Index(2)]
        public string ScaleDate { get; set; }

        /// <summary>
        /// 規模
        /// </summary>
        [Index(3)]
        public decimal? Scale { get; set; }

        /// <summary>
        /// 幣別
        /// </summary>
        [Index(4)]
        public string Currency { get; set; }
    }
    public sealed class Fundsize2Map : ClassMap<SysjustFundSizeFund2>
    {
        public Fundsize2Map()
        {
            Map(f => f.FirstBankCode).Index(0);
            Map(f => f.SysjustCode).Index(1);
            Map(f => f.ScaleDate).Index(2);
            Map(f => f.Scale).Index(3);
            Map(f => f.Currency).Index(4);
        }
    }
}