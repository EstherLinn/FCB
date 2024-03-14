using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Sitecore.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.IndexBanner
{
    public class IndexBannerModel
    {
        public Item DataSource { get; set; }

        public IList<Banner> Items { get; set; }

        public class Banner
        {
            public Banner(Item item)
            {
                Item = item;
            }

            public Item Item { get; set; }
            public string TitleField => Item["Title"];
            public string SubtitleField => Item["Subtitle"];
            public string ButtonText => Item["Button Text"];

            public string ButtonLink
            {
                get
                {
                    var btnLink = ((LinkField)Item.Fields["Button Link"]);
                    return btnLink.Url;
                }
            }


            public string Image
            {
                get
                {
                    var imgField = (ImageField)Item.Fields["Image"];
                    return imgField?.MediaItem != null ? MediaManager.GetMediaUrl(imgField.MediaItem) : string.Empty;
                }
            }
        }
    }
}
