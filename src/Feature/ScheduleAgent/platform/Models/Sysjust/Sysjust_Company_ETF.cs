using CsvHelper.Configuration;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using System;
using System.Globalization;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    public class SysjustCompanyEtf
    {
        [Index(0)]
        public string IssueCompanyID { get; set; }

        [Index(1)]
        public string EnglishName { get; set; }

        [Index(2)]
        public string Location { get; set; }

        [Index(3)]
        [TypeConverter(typeof(DateConverter))]
        public string EstablishmentDate { get; set; }

        [Index(4)]
        public string Website { get; set; }
    }

    public class DateConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }
            else
            {
                var cultureInfo = new CultureInfo("zh-TW");
                if (DateTime.TryParseExact(text, "yyyy/M/ddtth:mm:ss", cultureInfo, DateTimeStyles.None, out DateTime Establishment))
                {
                    return Establishment.ToString("yyyyMMdd");
                }
            }
            return base.ConvertFromString(text, row, memberMapData);
        }
    }
}