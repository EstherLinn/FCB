using CsvHelper.Configuration;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using System;
using System.Globalization;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// ETF 發行公司列表，檔案名稱：Sysjust-Company-ETF.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    public class SysjustCompanyEtf
    {
        /// <summary>
        /// 發行公司 ID
        /// </summary>
        [Index(0)]
        [NullValues("", "NULL", null)]
        public string IssueCompanyID { get; set; }

        /// <summary>
        /// 發行公司英文名稱
        /// </summary>
        [Index(1)]
        [NullValues("", "NULL", null)]
        public string EnglishName { get; set; }

        /// <summary>
        /// 設立地點
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string Location { get; set; }

        /// <summary>
        /// 成立日期
        /// </summary>
        [Index(3)]
        [TypeConverter(typeof(DateConverter))]
        public string EstablishmentDate { get; set; }

        /// <summary>
        /// 公司網址
        /// </summary>
        [Index(4)]
        [NullValues("", "NULL", null)]
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
                var formats = new[] { "yyyy/M/ddtth:mm:ss", "yyyy/M/dtth:mm:ss" };
                if (DateTime.TryParseExact(text, formats, cultureInfo, DateTimeStyles.None, out DateTime establishment))
                {
                    return establishment.ToString("yyyyMMdd");
                }
            }
            return base.ConvertFromString(text, row, memberMapData);
        }
    }
}