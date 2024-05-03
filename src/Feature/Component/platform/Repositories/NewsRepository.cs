using Feature.Wealth.Component.Models.MarketNews;
using Sitecore.Data.Items;
using System.Text;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Repositories
{
    public class NewsRepository
    {
        public string GetMarketNewsDetailsPageItemId()
        {
            Item FundRealtedItem = ItemUtils.GetItem(Template.MarketNewsRelatedLink.Root);
            return FundRealtedItem.GeneralLink(Template.MarketNewsRelatedLink.Fields.DetailLink)?.TargetItem.ID.ToString();
        }

        public string GetMarketNewsDetailsUrl()
        {
            Item FundRealtedItem = ItemUtils.GetItem(Template.MarketNewsRelatedLink.Root);
            return FundRealtedItem.GeneralLink(Template.MarketNewsRelatedLink.Fields.DetailLink)?.Url;
        }

        public string FullWidthToHalfWidth(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
            {
                if (c >= 0xFF01 && c <= 0xFF5E)
                {
                    sb.Append((char)(c - 0xFEE0));
                }
                else if (c == 0x3000)
                {
                    sb.Append((char)0x20);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
