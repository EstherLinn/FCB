using Sitecore.Data.Items;
using System.Web;

namespace Feature.Wealth.Component.Models.ETF.Detail
{
    public class EtfDetailBase
    {
        public Item Datasource { get; set; }

        /// <summary>
        /// 搜尋頁連結
        /// </summary>
        public string SearchPageLink { get; set; }

        /// <summary>
        /// 詳細頁附註說明
        /// </summary>
        public HtmlString AccordionContent { get; set; }

        /// <summary>
        /// 風險指標 Intro
        /// </summary>
        public HtmlString RiskIntro { get; set; }

        /// <summary>
        /// 風險象限圖 說明
        /// </summary>
        public HtmlString RiskQuadrantChartDescription { get; set; }

        /// <summary>
        /// 配息紀錄 說明
        /// </summary>
        public HtmlString DividendRecordDescription { get; set; }
    }
}