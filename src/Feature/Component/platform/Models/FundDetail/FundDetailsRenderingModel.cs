using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.FundDetail
{
    public class FundDetailsRenderingModel
    {
        public Item Item { get; set; }
    }
    public struct Template
    {
        public struct FundDetailsPage
        {
            public static readonly ID Id = new ID("{704285C7-C8F9-4901-9F9C-C8E5FD3C6CB5}");

            public struct Fields
            {
                public static readonly ID Content = new ID("{D64D1C03-2715-4FE8-9EF8-CBD835781571}");
                public static readonly ID Note = new ID("{3F1D490C-8EEC-45C8-B7C3-F7C5658F361C}");
                public static readonly ID Description = new ID("{26D48470-FF6A-4C96-9AF3-9F40FB79777C}");
                public static readonly ID LightboxNote = new ID("{2D960DB3-7A02-45B0-9339-248833B99587}");
            }
        }
    }


}
