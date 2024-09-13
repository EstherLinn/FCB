using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.SearchBar
{
    public class ForeignBondsResult
    {
        /// <summary>
        /// 第一銀行債券代碼
        /// </summary>
        public string BondCode { get; set; }

        /// <summary>
        /// 第一銀行債券名稱
        /// </summary>
        public string BondName { get; set; }

        /// <summary>
        /// Autocomplete 申購按鈕 HTML
        /// </summary>
        public string SubscribeButtonAutoHtml { get; set; }

        /// <summary>
        /// Autocomplete 關注按鈕 HTML
        /// </summary>
        public string FocusButtonAutoHtml { get; set; }
    }
}
