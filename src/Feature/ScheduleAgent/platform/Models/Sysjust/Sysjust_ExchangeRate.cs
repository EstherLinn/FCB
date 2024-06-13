using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustExchangeRate
    {
        public string CurrencyDescription { get; set; }

        [NullValues("", "NULL", null)]
        public string ExchangeRate { get; set; }
    }
}