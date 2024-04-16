using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Models.MutipleLinksLongCard
{
    public class MutipleLinksLongCardModel
    {
        public Item DataSource { get; set; }
        public MutipleLinksLongCardModel(Item item)
        {
            DataSource = item;
        }

    }
    public struct Templates
    {
        public struct MutipleLinksLongCardDataSource
        {
            public static readonly ID Id = new ID("{F6DBF6A2-383A-4615-98F4-53FFC83DC79F}");

            public struct Fields
            {
                public static readonly ID MainTitle = new ID("{E5D850E4-A84E-493B-94B6-E2E9481BFB79}");
                public static readonly ID Description = new ID("{FE3A7D64-971A-4F11-B17C-D21B0FA398B4}");
                public static readonly ID Image = new ID("{206AFB2F-6966-44E0-BD32-14999DA0064D}");
                public static readonly ID LinkText1 = new ID("{2DA63C65-E666-431B-8A8F-E5B37B0C3FC2}");
                public static readonly ID Link1 = new ID("{FDE30D78-79C8-464C-887E-32735C9B7819}");
                public static readonly ID LinkText2 = new ID("{1A87F70B-E132-4542-953C-B2B398E8467F}");
                public static readonly ID Link2 = new ID("{4B458191-A0EE-4D13-9A6C-87D608E5B25F}");
                public static readonly ID LinkText3 = new ID("{EEA55748-03FF-4BF0-9703-43847C0AF715}");
                public static readonly ID Link3 = new ID("{407D1FDC-8D25-4767-BDA8-D4D3B79C4EF6}");
                public static readonly ID LinkText4 = new ID("{3B4D7989-9A5C-4737-8159-E3A7078101F4}");
                public static readonly ID Link4 = new ID("{C4746AAD-6CD2-4415-A907-975925D7DD43}");
            }
        }
    }
}
