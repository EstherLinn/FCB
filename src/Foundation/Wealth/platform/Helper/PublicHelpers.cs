
using System.Text;
using System.Web;
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
        public static MvcHtmlString CompareButton(this HtmlHelper helper, object attributes, string id, string name, object investType, bool isListButton)
        {
            var builder = new TagBuilder("a");
            builder.MergeAttribute("href", "#");
            builder.MergeAttribute("eh-compare", string.Empty);
            builder.MergeAttribute("eh-compare-type", investType.ToString());
            builder.MergeAttribute("eh-compare-id", id);
            builder.MergeAttribute("eh-compare-name", name);
            builder.MergeAttribute("data-eh", "compare-init,compare-click");
            builder.MergeAttribute("data-msg", "加入比較|取消比較");
            builder.MergeAttribute("data-ia", "true");
            builder.MergeAttribute("eh-compare-add", "false");
            builder.MergeAttribute(isListButton == true ? "data-after-lt" : "data-after", "比較");
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
        public static MvcHtmlString FocusButton(this HtmlHelper helper, object attributes, string id, string name, object investType, bool isListButton)
        {
            var builder = new TagBuilder("a");
            builder.MergeAttribute("href", "#");
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
        /// 關注按鈕attributes
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="attributes">自訂屬性</param>
        /// <param name="id">投資種類代號</param>
        /// <param name="investType">投資種類</param>
        /// <returns>HtmlString</returns>
        public static HtmlString FocusTag(this HtmlHelper helper, object attributes, string id, string name, object investType)
        {
            StringBuilder sbBuilder = new StringBuilder();
            sbBuilder.Append(string.Format("href='{0}' ", "#"));
            sbBuilder.Append("eh-focus ");
            sbBuilder.Append(string.Format("eh-focus-type='{0}' ", investType.ToString()));
            sbBuilder.Append(string.Format("eh-focus-id='{0}' ", id));
            sbBuilder.Append(string.Format("eh-focus-name='{0}' ", name));
            sbBuilder.Append(string.Format("data-eh='{0}' ", "focus-init,focus-click"));
            sbBuilder.Append(string.Format("data-msg='{0}' ", "加入關注|取消關注"));
            sbBuilder.Append(string.Format("data-ia='{0}' ", "true"));
            sbBuilder.Append(string.Format("eh-focus-add='{0}' ", "false"));

            if (attributes != null)
            {
                var attr = HtmlHelper.AnonymousObjectToHtmlAttributes(attributes);
                foreach (var item in attr)
                {
                    sbBuilder.Append(string.Format("{0}='{1}' ", item.Key, item.Value));
                }
                if (!attr.ContainsKey("class"))
                {
                    sbBuilder.Append(string.Format("class='{0}' ", "o-statusBtn o-statusBtn--like"));
                }
            }
            else
            {
                //defalut class
                sbBuilder.Append(string.Format("class='{0}' ", "o-statusBtn o-statusBtn--like"));
            }

            return new HtmlString(sbBuilder.ToString());
        }

        /// <summary>
        /// 取消關注按鈕attributes
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="attributes">自訂屬性</param>
        /// <param name="id">投資種類代號</param>
        /// <param name="investType">投資種類</param>
        /// <returns>HtmlString</returns>
        public static HtmlString FocusCancelButton(this HtmlHelper helper, object attributes, string id, string name, object investType)
        {
            var builder = new TagBuilder("a");
            builder.MergeAttribute("href", "#popupCancelFocus");
            builder.MergeAttribute("eh-tracklist-focus",string.Empty);
            builder.MergeAttribute("eh-focus-type", investType.ToString());
            builder.MergeAttribute("eh-focus-id", id);
            builder.MergeAttribute("eh-focus-name ", name);
            builder.MergeAttribute("data-eh", "focuscancel-click");
            builder.MergeAttribute("data-msg", "加入關注|取消關注");
            builder.MergeAttribute("data-ia", "true");
            builder.MergeAttribute("data-ia-toast", "false");
            builder.MergeAttribute("data-popup", "true");
            builder.MergeAttribute("data-after-lt", "關注");
            builder.MergeAttribute("data-focus", string.Empty);

            builder.AddCssClass("o-statusBtn o-statusBtn--like is-active");
            if (attributes != null)
            {
                builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(attributes));
            }
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }
        /// <summary>
        /// 通知按鈕attributes
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="attributes">自訂屬性</param>
        /// <param name="id">投資種類代號</param>
        /// <param name="investType">投資種類</param>
        /// <returns>HtmlString</returns>
        public static HtmlString InfoButton(this HtmlHelper helper, object attributes, string id, string name, object investType)
        {
            var builder = new TagBuilder("a");
            builder.MergeAttribute("href", "#popupAlert");
            builder.MergeAttribute("eh-tracklist-info", string.Empty);
            builder.MergeAttribute("eh-info-type", investType.ToString());
            builder.MergeAttribute("eh-info-id", id);
            builder.MergeAttribute("eh-info-name ", name);
            builder.MergeAttribute("data-eh", "info-init,info-click");
            builder.MergeAttribute("data-msg", "加入通知|取消通知");
            builder.MergeAttribute("data-ia", "true");
            builder.MergeAttribute("data-ia-toast", "false");
            builder.MergeAttribute("data-popup", "true");
            builder.MergeAttribute("data-after-lt", "通知");
            builder.MergeAttribute("data-alert",id);

            builder.AddCssClass("o-statusBtn o-statusBtn--alert");
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
        public static MvcHtmlString SubscriptionButton(this HtmlHelper helper, object attributes, string id, object investType, bool isListButton)
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
        /// 申購按鈕 card使用
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="attributes">自訂屬性</param>
        /// <param name="id">投資種類代號</param>
        /// <param name="investType">投資種類</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString SubscriptionButtonForCard(this HtmlHelper helper, object attributes, string id, object investType)
        {
            var builder = new TagBuilder("a");
            builder.MergeAttribute("href", "#");
            builder.MergeAttribute("eh-subscription", string.Empty);
            builder.MergeAttribute("eh-subscription-type", investType.ToString());
            builder.MergeAttribute("eh-subscription-id", id);
            builder.MergeAttribute("data-eh", "subscription-init,subscription-click");
            builder.MergeAttribute("data-popup", "true");
            builder.AddCssClass("o-tableBtn o-tableBtn--card");
            builder.SetInnerText("申購");
            if (attributes != null)
            {
                builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(attributes));
            }
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// 申購按鈕attributes
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="attributes">自訂屬性</param>
        /// <param name="id">投資種類代號</param>
        /// <param name="investType">投資種類</param>
        /// <returns>HtmlString</returns>
        public static HtmlString SubscriptionTag(this HtmlHelper helper, object attributes, string id, string name, object investType)
        {
            StringBuilder sbBuilder = new StringBuilder();
            sbBuilder.Append(string.Format("href='{0}' ", "#"));
            sbBuilder.Append("eh-subscription ");
            sbBuilder.Append(string.Format("eh-subscription-type='{0}' ", investType.ToString()));
            sbBuilder.Append(string.Format("eh-subscription-id='{0}' ", id));
            sbBuilder.Append(string.Format("data-eh='{0}' ", "subscription-init,subscription-click"));
            sbBuilder.Append(string.Format("data-popup='{0}' ", "true"));
            if (attributes != null)
            {
                var attr = HtmlHelper.AnonymousObjectToHtmlAttributes(attributes);
                foreach (var item in attr)
                {
                    sbBuilder.Append(string.Format("{0}='{1}' ", item.Key, item.Value));
                }
                if (!attr.ContainsKey("class"))
                {
                    sbBuilder.Append(string.Format("class='{0}' ", "o-btn o-btn--primary o-btn--auto o-btn--thinest"));
                }
            }
            else
            {
                //defalut class
                sbBuilder.Append(string.Format("class='{0}' ", "o-btn o-btn--primary o-btn--auto o-btn--thinest"));
            }

            return new HtmlString(sbBuilder.ToString());
        }

        /// <summary>
        /// 幣別匯率Link
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="attributes">自訂屬性</param>
        /// <param name="currencyName">幣別名稱</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString CurrencyLink(this HtmlHelper helper, object attributes, string currencyName)
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

        public static string FocusButtonString(object attributes, string id, string name, object investType, bool isListButton)
        {
            var builder = new TagBuilder("a");
            builder.MergeAttribute("href", "#");
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
            return builder.ToString(TagRenderMode.Normal);
        }

        public static string SubscriptionButtonString(object attributes, string id, object investType, bool isListButton)
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
            return builder.ToString(TagRenderMode.Normal);
        }

        public static string CompareButtonString(object attributes, string id, string name, object investType, bool isListButton)
        {
            var builder = new TagBuilder("a");
            builder.MergeAttribute("href", "#");
            builder.MergeAttribute("eh-compare", string.Empty);
            builder.MergeAttribute("eh-compare-type", investType.ToString());
            builder.MergeAttribute("eh-compare-id", id);
            builder.MergeAttribute("eh-compare-name", name);
            builder.MergeAttribute("data-eh", "compare-init,compare-click");
            builder.MergeAttribute("data-msg", "加入比較|取消比較");
            builder.MergeAttribute("data-ia", "true");
            builder.MergeAttribute("eh-compare-add", "false");
            builder.MergeAttribute(isListButton == true ? "data-after-lt" : "data-after", "比較");
            builder.AddCssClass("o-statusBtn o-statusBtn--compare");
            if (attributes != null)
            {
                builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(attributes));
            }
            return builder.ToString(TagRenderMode.Normal);
        }
    }
}
