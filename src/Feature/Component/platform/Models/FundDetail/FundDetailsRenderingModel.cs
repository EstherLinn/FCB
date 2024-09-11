using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Feature.Wealth.Component.Models.FundDetail
{
    public class FundDetailsRenderingModel
    {
        public bool CanRender { get; set; }
        public Item Item { get; set; }
        public HtmlString Content { get; set; }
        public HtmlString Note { get; set; }
        public HtmlString Description { get; set; }
        public HtmlString LightboxNote { get; set; }
        public HtmlString StandardDeviation { get; set; }
        public HtmlString Sharpe { get; set; }
        public HtmlString Alpha { get; set; }
        public HtmlString Beta { get; set; }
        public HtmlString Rsquared { get; set; }
        public HtmlString IndexCorrelationCoefficient { get; set; }
        public HtmlString TrackingError { get; set; }
        public HtmlString Variance { get; set; }

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
                public static readonly ID StandardDeviation = new ID("{27A21E9F-423C-4D63-93BC-A3D8BEEEA52F}");
                public static readonly ID Sharpe = new ID("{851A06D4-5979-4C9D-98F0-85A684333C7A}");
                public static readonly ID Alpha = new ID("{3231B3A4-2E66-4ADA-AD3F-04FADFD899D7}");
                public static readonly ID Beta = new ID("{BDB25F6C-18A3-4445-9B4F-B0BF71439EC4}");
                public static readonly ID Rsquared = new ID("{B00C31F0-68D8-46DB-9856-7D1CEA44428D}");
                public static readonly ID IndexCorrelationCoefficient = new ID("{98194D48-BC11-4F53-884C-4E42AEE9AD0B}");
                public static readonly ID TrackingError = new ID("{3D8CFDC1-CD40-4785-B05D-E9BE4621273E}");
                public static readonly ID Variance = new ID("{99FF87AB-4F60-4CF6-9D12-6E8002502367}");

            }
        }
    }


}
