using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 境外基金發行公司與總代理公司資訊，檔案名稱：Sysjust-Company-Fund-3.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    public class SysjustCompanyFund3
    {
        /// <summary>
        /// 發行公司 ID
        /// </summary>
        [Index(0)]
        public string IssuerCompanyID { get; set; }

        /// <summary>
        /// 發行公司中文名稱
        /// </summary>
        [Index(1)]
        public string IssuerCompanyName { get; set; }

        /// <summary>
        /// 發行公司英文名稱
        /// </summary>
        [Index(2)]
        public string IssuerCompanyEnglishName { get; set; }

        /// <summary>
        /// 代理投顧公司 ID 
        /// </summary>
        [Index(3)]
        public string AgentCompanyID { get; set; }

        /// <summary>
        /// 代理投顧公司名稱
        /// </summary>
        [Index(4)]
        public string AgentCompanyName { get; set; }

        /// <summary>
        /// 總代理-成立日期
        /// </summary>
        [Index(5)]
        public string GeneralAgentEstablishmentDate { get; set; }

        /// <summary>
        /// 總代理-電話
        /// </summary>
        [Index(6)]
        public string GeneralAgentTelephone { get; set; }

        /// <summary>
        /// 總代理-傳真
        /// </summary>
        [Index(7)]
        public string GeneralAgentFax { get; set; }

        /// <summary>
        /// 總代理-地址
        /// </summary>
        [Index(8)]
        public string GeneralAgentAddress { get; set; }

        /// <summary>
        /// 總代理-網址
        /// </summary>
        [Index(9)]
        public string GeneralAgentWebsite { get; set; }

        /// <summary>
        /// 總代理-客服專線
        /// </summary>
        [Index(10)]
        public string GeneralAgentCustomerServiceLine { get; set; }

        /// <summary>
        /// 總代理-英文名稱
        /// </summary>
        [Index(11)]
        public string GeneralAgentEnglishName { get; set; }

        /// <summary>
        /// 總代理-公司簡介
        /// </summary>
        [Index(12)]
        public string GeneralAgentCompanyProfile { get; set; }

        /// <summary>
        /// 海外發行公司-設立地點
        /// </summary>
        [Index(13)]
        public string OverseasIssuerLocation { get; set; }

        /// <summary>
        /// 海外發行公司-成立日期
        /// </summary>
        [Index(14)]
        public string OverseasIssuerEstablishmentDate { get; set; }

        /// <summary>
        /// 海外發行公司-網址
        /// </summary>
        [Index(15)]
        public string OverseasIssuerWebsite { get; set; }

        /// <summary>
        /// 海外發行公司-介紹
        /// </summary>
        [Index(16)]
        public string OverseasIssuerIntroduction { get; set; }
    }
}