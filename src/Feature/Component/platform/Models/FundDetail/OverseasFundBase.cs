using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.FundDetail
{
    public class OverseasFundBase : FundBase
    {
        /// <summary>
        /// ISIN Code
        /// </summary>
        public string ISINCode { get; set; }

        /// <summary>
        /// 註冊地
        /// </summary>
        public string RegistrationLocation { get; set; }
        /// <summary>
        /// 傘型基金
        /// </summary>
        public string UmbrellaFund { get; set; }
        /// <summary>
        /// 基金核準生效日
        /// </summary>
        public string FundApprovalEffectiveDate { get; set; }
        /// <summary>
        /// 總代理基金生效日
        /// </summary>
        public string MasterAgentFundEffectiveDate { get; set; }


         /// <summary>
         /// 台灣總代理
         /// </summary>
        public string AgentCompanyName { get; set; }
        /// <summary>
        /// 台灣總代理 網址
        /// </summary>
        public string GeneralAgentWebsite { get; set; }
        /// <summary>
        /// 海外發行公司
        /// </summary>
        public string OverseasFundIssuerName { get; set; }

        /// <summary>
        /// 海外發行公司網址
        /// </summary>
        public string OverseasIssuerWebsite { get; set; }


    }
}
