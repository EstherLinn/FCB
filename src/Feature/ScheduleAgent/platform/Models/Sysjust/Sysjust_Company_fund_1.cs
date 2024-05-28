using CsvHelper.Configuration.Attributes;
using System;

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
        public string FundCompanyName { get; set; }

        /// <summary>
        /// 基金公司代碼
        /// </summary>
        public string FundCompanyCode { get; set; }

        /// <summary>
        /// 成立期
        /// </summary>
        public DateTime EstablishmentDate { get; set; }

        /// <summary>
        /// 董事長
        /// </summary>
        public string Chairman { get; set; }

        /// <summary>
        /// 總經理
        /// </summary>
        public string GeneralManager { get; set; }

        /// <summary>
        /// 發言人
        /// </summary>
        public string Spokesperson { get; set; }

        /// <summary>
        /// 網址
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// 電子郵件
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 公司簡介
        /// </summary>
        public string CompanyProfile { get; set; }

        /// <summary>
        /// 客服聯絡方式
        /// </summary>
        public string CustomerServiceContact { get; set; }

        /// <summary>
        /// 電話
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 傳真
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 基金數
        /// </summary>
        public string FundCount { get; set; }

        /// <summary>
        /// 基金規模
        /// </summary>
        public string FundSize { get; set; }
    }
}