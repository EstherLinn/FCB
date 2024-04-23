﻿using Sitecore.Data;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.ETF.Search
{
    public class EtfSearchResultModel
    {
        public string DetailPageLink { get; set; }

        public List<EtfSearchResult> ResultProducts { get; set; }
    }

    public struct Templates
    {
        public struct EtfSearchDatasource
        {
            public static readonly ID Id = new ID("{6664CA2A-3452-4F39-8F75-F18DEF5238DF}");

            public struct Fields
            {
                public static readonly ID DetailPageLink = new ID("{56695F44-A5FD-4BF8-8430-BD7B5D5B06A5}");
                public static readonly ID HotKeyword = new ID("{B29BDB52-5899-4F80-A909-C54735395DF7}");
                public static readonly ID HotProduct = new ID("{26A932EB-7E4F-4DE6-9AD9-205FBC9BAFE5}");
            }
        }

        public struct TagContent
        {
            public static readonly ID Id = new ID("{6132D100-78A2-440A-8CF8-B9AE3146BE8B}");

            public struct Fields
            {
                public static readonly ID TagKey = new ID("{EB50204C-7637-4247-9734-E3A2493B61E8}");
                public static readonly ID ProductCode = new ID("{2396DFEA-C6DA-4720-AAE2-44B4BC373AAA}");
                public static readonly ID Type = new ID("{E043FDCE-AF2F-4847-A024-D1ABAFF8E3B9}");
            }
        }

        public struct DropdownOption
        {
            public static readonly ID Id = new ID("{362DF993-E969-4243-898A-B01297B4B18A}");

            public struct Fields
            {
                public static readonly ID OptionText = new ID("{8532457A-4AF0-488D-8C45-34AC0AE7A859}");
                public static readonly ID OptionValue = new ID("{B7E1B3B4-5A73-4C5E-A67B-C7DD68779F83}");
            }
        }
    }
}