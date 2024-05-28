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
        public string IssueCompanyID { get; set; }

        /// <summary>
        /// 發行公司英文名稱
        /// </summary>
        [Index(1)]
        public string EnglishName { get; set; }

        /// <summary>
        /// 設立地點
        /// </summary>
        [Index(2)]
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