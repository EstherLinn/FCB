using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
using Feature.Wealth.Component.Models.TabCards;

namespace Feature.Wealth.Component.Models.RecommendedProduct
{
    public class RecommendedProductModel
    {
        public Item Item { get; set; }
        public IList<FundCardBasicDTO> RecommendFunds { get; set; }
        public IList<NavDTO> FundCardsNavs { get; set; }
        public IList<string> FundIDList { get; set; }
        public string DetailLink { get; set; }
    }

    public struct Template
    {
        public struct RecommendedProduct
        {
            public static readonly ID Id = new ID("{268CD276-26A4-4E0C-BA60-A0CF27419525}");

            public struct Fields
            {
                public static readonly ID MainTitle = new ID("{226C25D3-CED3-493B-B0CD-1374C598595B}");
                public static readonly ID FundIDLIst = new ID("{A893598E-1A32-45CB-9ACC-5CA93133D08E}");
            }
        }
    }
}
