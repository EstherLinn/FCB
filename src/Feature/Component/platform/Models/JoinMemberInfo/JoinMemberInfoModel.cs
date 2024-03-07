using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Models.JoinMemberInfo
{
    public class JoinMemberInfoModel
    {
        public Item Item { get; set; }

        private MvcHtmlString GetFieldValue(string fieldName)
        {
            return new MvcHtmlString(Sitecore.Web.UI.WebControls.FieldRenderer.Render(Item, fieldName));
        }

        public MvcHtmlString Title
        {
            get { return GetFieldValue("Title"); }
        }

        public MvcHtmlString Subtitle
        {
            get { return GetFieldValue("Subtitle"); }
        }

        public MvcHtmlString Title1
        {
            get { return GetFieldValue("Titl1"); }
        }

        public MvcHtmlString Title2
        {
            get { return GetFieldValue("Titl2"); }
        }

        public MvcHtmlString Title3
        {
            get { return GetFieldValue("Titl3"); }
        }

        public MvcHtmlString Description1
        {
            get { return GetFieldValue("Description1"); }
        }

        public MvcHtmlString Description2
        {
            get { return GetFieldValue("Description2"); }
        }

        public MvcHtmlString Description3
        {
            get { return GetFieldValue("Description3"); }
        }

        private string GetImageUrl(string fieldName)
        {
            var imgField = (ImageField)Item.Fields[fieldName];
            return imgField?.MediaItem != null ? MediaManager.GetMediaUrl(imgField.MediaItem) : string.Empty;
        }

        public string Image1
        {
            get
            {
                return GetImageUrl("Image1");
            }
        }

        public string Image1_3X
        {
            get
            {
                return GetImageUrl("Image1_3X");
            }
        }

        public string Image2
        {
            get
            {
                return GetImageUrl("Image2");
            }
        }

        public string Image2_3X
        {
            get
            {
                return GetImageUrl("Image2_3X");
            }
        }

        public string Image3
        {
            get
            {
                return GetImageUrl("Image3");
            }
        }

        public string Image3_3X
        {
            get
            {
                return GetImageUrl("Image3_3X");
            }
        }
    }
}
