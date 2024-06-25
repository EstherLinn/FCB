using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustExchangeRate
    {
        [Index(0)]
        public string CurrencyDescription { get; set; }

        [Index(1)]
        [NullValues("", "NULL", null)]
        public string ExchangeRate { get; set; }
    }
}