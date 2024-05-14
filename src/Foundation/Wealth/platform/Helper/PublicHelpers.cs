
using System.Web.Mvc;

namespace Foundation.Wealth.Helper
{
    public static class PublicHelpers
    {
        /// <summary>
        /// 比較按鈕
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="attributes">自訂屬性</param>
        /// <param name="name">投資種類名稱</param>
        /// <param name="id">投資種類代號</param>
        /// <param name="investType">投資種類</param>
        /// <param name="isListButton">是否為列表按鈕</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString CompareButton(this HtmlHelper helper, object attributes,string id,string name,object investType,bool isListButton = true)
        {
            var builder = new TagBuilder("a");
            builder.MergeAttribute("href", "javascript:;");
            builder.MergeAttribute("eh-compare", string.Empty);
            builder.MergeAttribute("eh-compare-type", investType.ToString());
            builder.MergeAttribute("eh-compare-id", id);
            builder.MergeAttribute("eh-compare-name", name);
            builder.MergeAttribute("data-eh", "compare-init,compare-click");
            builder.MergeAttribute("data-msg", "加入比較|取消比較");
            builder.MergeAttribute("data-ia", "true");
            builder.MergeAttribute("eh-compare-add", "false");
            builder.MergeAttribute(isListButton == true ? "data-after-lt":"data-after", "比較");
            builder.AddCssClass("o-statusBtn o-statusBtn--compare");
            if (attributes != null)
            {
                builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(attributes));
            }
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }
        /// <summary>
        /// 關注按鈕
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="attributes">自訂屬性</param>
        /// <param name="id">投資種類代號</param>
        /// <param name="investType">投資種類</param>
        /// <param name="isListButton">是否為列表按鈕</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString FocusButton(this HtmlHelper helper, object attributes, string id,string name, object investType, bool isListButton = true)
        {
            var builder = new TagBuilder("a");
            builder.MergeAttribute("href", "javascript:;");
            builder.MergeAttribute("eh-focus", string.Empty);
            builder.MergeAttribute("eh-focus-type", investType.ToString());
            builder.MergeAttribute("eh-focus-id", id);
            builder.MergeAttribute("eh-focus-name", name);
            builder.MergeAttribute("data-eh", "focus-init,focus-click");
            builder.MergeAttribute("data-msg", "加入關注|取消關注");
            builder.MergeAttribute("data-ia", "true");
            builder.MergeAttribute("eh-focus-add", "false");
            builder.MergeAttribute(isListButton == true ? "data-after-lt" : "data-after", "關注");
            builder.AddCssClass("o-statusBtn o-statusBtn--like");
            if (attributes != null)
            {
                builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(attributes));
            }
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }
        /// <summary>
        /// 申購按鈕
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="attributes">自訂屬性</param>
        /// <param name="id">投資種類代號</param>
        /// <param name="investType">投資種類</param>
        /// <param name="isListButton">是否為列表按鈕</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString SubscriptionButton(this HtmlHelper helper, object attributes, string id, object investType, bool isListButton = true)
        {
            var builder = new TagBuilder("a");
            builder.MergeAttribute("href", "#");
            builder.MergeAttribute("eh-subscription", string.Empty);
            builder.MergeAttribute("eh-subscription-type", investType.ToString());
            builder.MergeAttribute("eh-subscription-id", id);
            builder.MergeAttribute("data-eh", "subscription-init,subscription-click");
            builder.MergeAttribute("data-popup", "true");
            builder.AddCssClass(isListButton == true ? "o-tableBtn o-tableBtn--card@lt" : "o-fixedBtn");
            builder.SetInnerText("申購");
            if (attributes != null)
            {
                builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(attributes));
            }
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// 幣別匯率Link
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="attributes">自訂屬性</param>
        /// <param name="currencyName">幣別名稱</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString CurrencyLink(this HtmlHelper helper, object attributes,string currencyName)
        {
            var builder = new TagBuilder("span");
            if (string.IsNullOrEmpty(currencyName))
            {               
                builder.SetInnerText("-");
                return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
            }
            else
            {
                builder = new TagBuilder("a");
                builder.SetInnerText(currencyName);
                builder.MergeAttribute("href", "#popupCurrencyWebSite");
                builder.MergeAttribute("data-popup", "true");
                builder.AddCssClass("o-link");
            }
            if (attributes != null)
            {
                builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(attributes));
            }
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }
    }
}
