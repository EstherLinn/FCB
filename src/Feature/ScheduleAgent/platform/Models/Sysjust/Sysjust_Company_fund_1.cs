using CsvHelper.Configuration;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using System;
using System.Globalization;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 境內基金公司列表，檔案名稱：Sysjust-Company-Fund-1.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    public class SysjustCompanyFund1
    {
        /// <summary>
        /// 基金公司名稱
        /// </summary>
        [Index(0)]
        [NullValues("", "NULL", null)]
        public string FundCompanyName { get; set; }

        /// <summary>
        /// 基金公司代碼
        /// </summary>
        [Index(1)]
        [NullValues("", "NULL", null)]
        public string FundCompanyCode { get; set; }

        /// <summary>
        /// 成立期
        /// </summary>

        [Index(2)]
        [TypeConverter(typeof(DateConverter))]
        public string EstablishmentDate { get; set; }

        /// <summary>
        /// 董事長
        /// </summary>
        [Index(3)]
        [NullValues("", "NULL", null)]
        public string Chairman { get; set; }

        /// <summary>
        /// 總經理
        /// </summary>
        [Index(4)]
        [NullValues("", "NULL", null)]
        public string GeneralManager { get; set; }

        /// <summary>
        /// 發言人
        /// </summary>
        [Index(5)]
        [NullValues("", "NULL", null)]
        public string Spokesperson { get; set; }

        /// <summary>
        /// 網址
        /// </summary>
        [Index(6)]
        [NullValues("", "NULL", null)]
        public string Website { get; set; }

        /// <summary>
        /// 電子郵件
        /// </summary>
        [Index(7)]
        [NullValues("", "NULL", null)]
        public string Email { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Index(8)]
        [NullValues("", "NULL", null)]
        public string Address { get; set; }

        /// <summary>
        /// 公司簡介
        /// </summary>
        [Index(9)]
        [NullValues("", "NULL", null)]
        public string CompanyProfile { get; set; }

        /// <summary>
        /// 客服聯絡方式
        /// </summary>
        [Index(10)]
        [NullValues("", "NULL", null)]
        public string CustomerServiceContact { get; set; }

        /// <summary>
        /// 電話
        /// </summary>
        [Index(11)]
        [NullValues("", "NULL", null)]
        public string Telephone { get; set; }

        /// <summary>
        /// 傳真
        /// </summary>
        [Index(12)]
        [NullValues("", "NULL", null)]
        public string Fax { get; set; }

        /// <summary>
        /// 基金數
        /// </summary>
        [Index(13)]
        [NullValues("", "NULL", null)]
        public string FundCount { get; set; }

        /// <summary>
        /// 基金規模
        /// </summary>
        [Index(14)]
        public decimal? FundSize { get; set; }
    }
}