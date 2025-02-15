﻿using Sitecore.Data;

namespace Feature.Wealth.Component
{
    public class ComponentTemplates
    {
        public struct FundTag
        {
            public static readonly ID FundTagRoot = new ID("{BBAF1962-669A-4A56-A4BC-974369FEDB1F}");
            public static readonly ID FundTagItem = new ID("{E7C05C03-3A38-4186-A358-32A9253AD7DF}");

            public struct Fields
            {
                public static readonly ID FundTagTitle = new ID("{5361C9AE-E716-4127-A0A6-AF28CCE39134}");
                public static readonly ID FundTagType = new ID("{9828043B-3665-4CC1-A66B-9D007791CF8B}");
                public static readonly ID FundIdList = new ID("{0280C1D9-7E84-41AA-BD5E-2C8D94E59653}");
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

        public struct SimpleDropdownOption
        {
            public static readonly ID Id = new ID("{12414F7A-2E63-45EB-8AB5-586CB1122A11}");

            public struct Fields
            {
                public static readonly ID OptionText = new ID("{0EE26DDB-2503-4436-83F0-AC203B8241E5}");
                public static readonly ID OptionValue = new ID("{2099E923-E6F8-4F87-A714-5B27B6722D39}");
            }
        }

        public struct Category
        {
            public static readonly ID Id = new ID("{44DAE333-F9A8-45B1-A876-310C13E5A50A}");

            public struct Fields
            {
                public static readonly ID TagType = new ID("{E043FDCE-AF2F-4847-A024-D1ABAFF8E3B9}");
            }
        }

        public struct TagContent    
        {
            public static readonly ID Id = new ID("{6132D100-78A2-440A-8CF8-B9AE3146BE8B}");

            public struct Fields
            {
                public static readonly ID TagName = new ID("{EB50204C-7637-4247-9734-E3A2493B61E8}");
                public static readonly ID ProductCodeList = new ID("{2396DFEA-C6DA-4720-AAE2-44B4BC373AAA}");
            }
        }
    }
}