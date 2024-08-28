using CsvHelper.Configuration;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    [Delimiter(",")]
    [HasHeaderRecord(false)]
    public class BondList
    {
        [Index(0)]
        public string BondCode { get; set; }

        [Index(1)]
        [NullValues("", "NULL", null)]
        public string ISINCode { get; set; }

        [Index(2)]
        [NullValues("", "NULL", null)]
        public string BondName { get; set; }

        [Index(3)]
        [NullValues("", "NULL", null)]
        public string Currency { get; set; }

        [Index(4)]
        [NullValues("", "NULL", null)]
        public string CurrencyCode { get; set; }

        [Index(5)]
        [TypeConverter(typeof(DecimalConverter))]
        public decimal? InterestRate { get; set; }

        [Index(6)]
        public int? PaymentFrequency { get; set; }

        [Index(7)]
        [NullValues("", "NULL", null)]
        public string RiskLevel { get; set; }

        [Index(8)]
        [NullValues("", "NULL", null)]
        public string SalesTarget { get; set; }

        [Index(9)]
        public int? MinSubscriptionForeign { get; set; }

        [Index(10)]
        public int? MinSubscriptionNTD { get; set; }

        [Index(11)]
        public int? MinIncrementAmount { get; set; }

        [Index(12)]
        [NullValues("", "NULL", null)]
        public string MaturityDate { get; set; }

        [Index(13)]
        [NullValues("", "NULL", null)]
        public string StopSubscriptionDate { get; set; }

        [Index(14)]
        [NullValues("", "NULL", null)]
        public string RedemptionDateByIssuer { get; set; }

        [Index(15)]
        [NullValues("", "NULL", null)]
        public string Issuer { get; set; }

        [Index(16)]
        [NullValues("", "NULL", null)]
        public string OpenToPublic { get; set; }

        [Index(17)]
        [NullValues("", "NULL", null)]
        public string Listed { get; set; }

        [Index(18)]
        [NullValues("", "NULL", null)]
        public string ListingDate { get; set; }

        [Index(19)]
        [NullValues("", "NULL", null)]
        public string DelistingDate { get; set; }
    }

    public class DecimalConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }
            else
            {
                string cleanedText = text.TrimEnd('%').Trim();

                if (decimal.TryParse(cleanedText, out decimal decimalValue))
                {
                    return decimalValue;
                }
            }
            return base.ConvertFromString(text, row, memberMapData);
        }
    }
}
