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

        /// <summary>
        /// 指標指數
        /// </summary>
        public string IndicatorIndexName { get; set; }
        /// <summary>
        /// 最高經理費
        /// </summary>
        public decimal? MaxManagerFee { get; set; }
        /// <summary>
        /// 最高保管費
        /// </summary>
        public decimal? MaxStorageFee { get; set; }
        /// <summary>
        /// 單一報價
        /// </summary>
        public string SingleQuote { get; set; }
        /// <summary>
        /// 費用備註
        /// </summary>
        public string FeeRemarks { get; set; }

    }
}
