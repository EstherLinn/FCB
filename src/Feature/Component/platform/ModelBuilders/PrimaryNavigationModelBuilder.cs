using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System;
using Xcms.Sitecore.Feature.Navigation.Models;
using Xcms.Sitecore.Feature.Navigation.Repositories;
using Xcms.Sitecore.Foundation.Caching;

namespace Feature.Wealth.Component.ModelBuilders
{
    public class PrimaryNavigationModelBuilder(DeclaredNavigationRepository repository, ICacheManager cache)
    {
        private DeclaredNavigationRepository Repository { get; } = repository;

        private ICacheManager Cache { get; } = cache;

        public NavigationMenu GetModel(Item datasource, Item contextItem)
        {
            Assert.ArgumentNotNull(datasource, "datasource");
            Assert.ArgumentNotNull(contextItem, "contextItem");

            /* Rendering Datasource should be the root level Navigation Menu Item,
             * We want to know which menu item to highlight, so we also have to pass in the Context Item.
             *
             * We can't cache this Item by Datasource, because we'll lose context highlighting, but we don't
             * want to generate this model from scratch for every request, so we're going to cache it by the
             * contextItem's ID
             */

            string key = GetCacheKey(contextItem, datasource);

            var model = this.Cache.Get<NavigationMenu>(key);

            if (model == null)
            {
                model = this.Repository.GetNavigation(datasource, contextItem);

                //this.Cache.Add(key, model, DateTime.Now.AddMinutes(30));
            }

            return model;
        }

        private string GetCacheKey(Item contextItem, Item datasource) => GetType().FullName + contextItem.ID + datasource.ID;
    }
}