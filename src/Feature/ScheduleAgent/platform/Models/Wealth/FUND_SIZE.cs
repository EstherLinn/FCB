using CsvHelper.Configuration.Attributes;
using FixedWidthParserWriter;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    public class FundSize : IFixedWidth
    {
        [FixedWidthLineField(Start = 1, Length = 4)]
        public string BankProductCode { get; set; }

        [FixedWidthLineField(Start = 5, Length = 20)]
        public string ISINCode { get; set; }

        [FixedWidthLineField(Start = 25, Length = 5)]
        public string FundCurrency { get; set; }

        private decimal? _fundSizeMillionOriginalCurrency;
        [FixedWidthLineField(Start = 30, Length = 17)]
        public decimal? FundSizeMillionOriginalCurrency
        {
            get => _fundSizeMillionOriginalCurrency;
            set => _fundSizeMillionOriginalCurrency = value / 1000000;
        }

        private decimal? _fundSizeMillionTWD;
        [FixedWidthLineField(Start = 47, Length = 17)]
        public decimal? FundSizeMillionTWD
        {
            get => _fundSizeMillionTWD;
            set => _fundSizeMillionTWD = value / 1000000;
        }

        [FixedWidthLineField(Start = 64, Length = 4)]
        public string ParentFundBankProductCode { get; set; }

        [FixedWidthLineField(Start = 68, Length = 20)]
        public string ParentFundISINCode { get; set; }

        private decimal? _referenceExchangeRate;
        [FixedWidthLineField(Start = 88, Length = 9)]
        public decimal? ReferenceExchangeRate
        {
            get => _referenceExchangeRate;
            set => _referenceExchangeRate = value / 100000;
        }

        public DefaultConfig GetDefaultConfig(int structureTypeId)
        {
            var defaultConfig = new DefaultConfig();

            return defaultConfig;

        }
    }
}