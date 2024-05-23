using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    public class SysjustFundSizeFund2
    {
        [Index(0)]
        public string FirstBankCode { get; set; }

        [Index(1)]
        public string SysjustCode { get; set; }

        [Index(2)]
        public string ScaleDate { get; set; }

        [Index(3)]
        public string Scale { get; set; }

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