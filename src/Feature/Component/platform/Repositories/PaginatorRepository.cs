using Sitecore.Data.Items;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Repositories
{
    public class PaginatorRepository
    {
        public Item GetPreviousPageItem(Item currentItem)
        {
            if (currentItem?.Parent != null)
            {
                Item[] siblings = ItemUtils.GetChildren(currentItem.Parent).ToArray();

                int currentIndex = -1;
                for (int i = 0; i < siblings.Length; i++)
                {
                    if (siblings[i].ID == currentItem.ID)
                    {
                        currentIndex = i;
                        break;
                    }
                }

                if (currentIndex > 0)
                {
                    return siblings[currentIndex - 1];
                }
            }

            return null;
        }

        public Item GetNextPageItem(Item currentItem)
        {
            if (currentItem?.Parent != null)
            {
                Item[] siblings = ItemUtils.GetChildren(currentItem.Parent).ToArray();

                int currentIndex = -1;
                for (int i = 0; i < siblings.Length; i++)
                {
                    if (siblings[i].ID == currentItem.ID)
                    {
                        currentIndex = i;
                        break;
                    }
                }

                if (currentIndex < siblings.Length - 1)
                {
                    return siblings[currentIndex + 1];
                }
            }

            return null;
        }
    }
}
