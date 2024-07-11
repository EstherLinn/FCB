using Sitecore;
using Sitecore.Pipelines.HttpRequest;
using System.Collections.Generic;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Foundation.Wealth.Pipelines.HttpRequestBegin
{
    public class VersionNotfoundRedirect : HttpRequestProcessor
    {
        public List<string> Sites { get; set; }

        public VersionNotfoundRedirect()
        {
            this.Sites = new List<string>();
        }

        public override void Process(HttpRequestArgs args)
        {
            //未設定使用站台
            if (this.Sites == null || this.Sites.Count == 0)
                return;

            //檢測是否為啟用功能的站台
            var site = Sitecore.Context.GetSiteName();
            if (site == null || !this.Sites.Contains(site))
                return;

            //取得當前執行Item
            var item = Context.Item;
            if (item == null)
                return;

            //檢驗版本與含有顯示設定
            if (item.Versions.Count == 0 && item.HasLayout())
            {
                var homeItem = Sitecore.Context.Site.GetStartItem();

                if (homeItem != null)
                {
                    //取得站台根節點連結
                    string url = Sitecore.Links.LinkManager.GetItemUrl(homeItem);

                    //當前節點已經為首頁
                    if (item.ID == homeItem.ID)
                        url = Sitecore.Web.WebUtil.AddQueryString(url, $"sc_lang", Context.Site.Language);

                    args.HttpContext.Response.Redirect(url, true);
                    args.AbortPipeline();
                }
            }
        }
    }
}