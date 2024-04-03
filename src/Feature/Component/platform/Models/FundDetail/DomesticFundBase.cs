using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.FundDetail
{
    public class DomesticFundBase : FundBase
    {
        /// <summary>
        /// 基金統編
        /// </summary>
        public string FundUnifiedIdentificationNumber { get; set; }
        /// <summary>
        /// 成立時規模
        /// </summary>
        public decimal? EstablishmentScaleMillion { get; set; }
        /// <summary>
        /// 定時定額
        /// </summary>
        public string RegularInvestmentPlan { get; set; }
        /// <summary>
        /// 基金公司
        /// </summary>
        public string FundCompanyName { get; set; }

        /// <summary>
        /// 基金公司
        /// </summary>
        public string FundCompanyWebSite { get; set; }

        /// <summary>
        /// 一年內配息次數
        /// </summary>
        public string DividendFrequencyOneYear { get; set; }


    }
}
