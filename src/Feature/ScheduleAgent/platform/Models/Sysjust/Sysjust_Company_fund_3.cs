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
        public string IssuerCompanyID { get; set; }

        /// <summary>
        /// 發行公司中文名稱
        /// </summary>
        public string IssuerCompanyName { get; set; }

        /// <summary>
        /// 發行公司英文名稱
        /// </summary>
        public string IssuerCompanyEnglishName { get; set; }

        /// <summary>
        /// 代理投顧公司 ID 
        /// </summary>
        public string AgentCompanyID { get; set; }

        /// <summary>
        /// 代理投顧公司名稱
        /// </summary>
        public string AgentCompanyName { get; set; }

        /// <summary>
        /// 總代理-成立日期
        /// </summary>
        public string GeneralAgentEstablishmentDate { get; set; }

        /// <summary>
        /// 總代理-電話
        /// </summary>
        public string GeneralAgentTelephone { get; set; }

        /// <summary>
        /// 總代理-傳真
        /// </summary>
        public string GeneralAgentFax { get; set; }

        /// <summary>
        /// 總代理-地址
        /// </summary>
        public string GeneralAgentAddress { get; set; }

        /// <summary>
        /// 總代理-網址
        /// </summary>
        public string GeneralAgentWebsite { get; set; }

        /// <summary>
        /// 總代理-客服專線
        /// </summary>
        public string GeneralAgentCustomerServiceLine { get; set; }

        /// <summary>
        /// 總代理-英文名稱
        /// </summary>
        public string GeneralAgentEnglishName { get; set; }

        /// <summary>
        /// 總代理-公司簡介
        /// </summary>
        public string GeneralAgentCompanyProfile { get; set; }

        /// <summary>
        /// 海外發行公司-設立地點
        /// </summary>
        public string OverseasIssuerLocation { get; set; }

        /// <summary>
        /// 海外發行公司-成立日期
        /// </summary>
        public string OverseasIssuerEstablishmentDate { get; set; }

        /// <summary>
        /// 海外發行公司-網址
        /// </summary>
        public string OverseasIssuerWebsite { get; set; }

        /// <summary>
        /// 海外發行公司-介紹
        /// </summary>
        public string OverseasIssuerIntroduction { get; set; }
    }
}