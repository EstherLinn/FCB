using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.Compare
{
    public class CompareBase
    {
        /// <summary>
        /// 三個月內最高淨值
        /// </summary>
        public decimal? MaxNetAssetValueThreeMonths { get; set; }
        /// <summary>
        /// 三個月內最低淨值
        /// </summary>
        public decimal? MinNetAssetValueThreeMonths { get; set; }
    }
}
