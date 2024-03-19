using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.FundReturn
{
    public class FundReturnSearchFilterResponse
    {
        /// <summary>
        /// 基金篩選
        /// </summary>
        public IList<FundReturnFilterModel> FundReturnFilter { get; set; }

        /// <summary>
        /// 快速搜尋
        /// </summary>
        public IList<Fund> FundReturnSearch { get; set; }
    }
}