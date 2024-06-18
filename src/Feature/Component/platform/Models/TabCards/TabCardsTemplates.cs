using Sitecore.Data;

namespace Feature.Wealth.Component.Models.TabCards
{
    public struct TabCardDatasource
    {
        public static readonly ID Id = new ID("{D87D52BC-7667-45A5-B521-993C6B18B33D}");

        public struct Fields
        {
            // 繼承_TabCard
        }
    }

    public struct Tab3CardDatasource
    {
        public static readonly ID Id = new ID("{39A22ABC-8257-4D15-8F2C-AC7A39652A61}");

        public struct Fields
        {
            // 繼承_TabCard

            public static readonly ID Image1 = new ID("{3F092EED-A5C4-4360-8461-52A2ED2D661F}");
            public static readonly ID ArticleLink1 = new ID("{B5D7F204-337D-49E5-BC3A-D34A72EBD817}");
            public static readonly ID Date1 = new ID("{0022D67D-F27B-4714-A8B4-D6C34952E1AA}");
            public static readonly ID Content1 = new ID("{D3081D12-2730-4E9D-B218-F7C0A0E04274}");
            public static readonly ID Image2 = new ID("{F3B2F133-BF77-478B-BBF8-0E43294B45F6}");
            public static readonly ID ArticleLink2 = new ID("{A08D53FC-0A2B-4005-80DB-95C21691B7C3}");
            public static readonly ID Date2 = new ID("{79DFB1BB-6184-40CA-97CE-AE0FADA76A84}");
            public static readonly ID Content2 = new ID("{D67C8598-99BD-4A0B-8FFC-ED492B49711D}");
        }
    }

    public struct _TabCard
    {
        public static readonly ID Id = new ID("{6B29762C-A126-4C2C-83FC-64B22A96BFF8}");

        public struct Fields
        {
            public static readonly ID Banner1 = new ID("{7EB10D54-ADE1-468D-AE52-9955B6556B3B}");
            public static readonly ID BannerLink1 = new ID("{F893E745-3357-4992-8383-B80D31073700}");
            public static readonly ID Title1 = new ID("{895271C9-A39C-4CB7-8E04-06A194CBD3CD}");
            public static readonly ID Description1 = new ID("{FF6864EF-95B1-41C4-93DC-A08B65CE945D}");
            public static readonly ID IsBlackFont1 = new ID("{06B99718-5D2B-4ED1-9675-8E726D63FF6D}");
            public static readonly ID Banner2 = new ID("{8D4BFC58-F229-4F8C-8884-B6FFDC26BDFA}");
            public static readonly ID BannerLink2 = new ID("{06010314-DF44-4313-A5A0-3E86388F45B2}");
            public static readonly ID Title2 = new ID("{5788BE08-87EC-47EB-A26A-AE9D116E50BD}");
            public static readonly ID Description2 = new ID("{C95CDFEB-4267-4290-A0DD-6D12DBF3E4BD}");
            public static readonly ID IsBlackFont2 = new ID("{AE7B32AA-CF85-4353-A7A8-0997BB2A2BEE}");
            public static readonly ID Banner3 = new ID("{A50A09C8-2722-4991-9464-B124BB9FB62C}");
            public static readonly ID BannerLink3 = new ID("{EF2CE684-6A3D-49F4-A613-12A96F4C7E88}");
            public static readonly ID Title3 = new ID("{3373B166-4B4E-474F-A759-97B9B1FA5128}");
            public static readonly ID Description3 = new ID("{153DDF46-DD61-410E-80F9-3C73129E4127}");
            public static readonly ID IsBlackFont3 = new ID("{F213149F-9B8D-46BC-BA43-27015AD58733}");
            public static readonly ID Banner4 = new ID("{54957FE5-628B-44BF-B176-5E666EA5A2A8}");
            public static readonly ID BannerLink4 = new ID("{1DD8EA4D-8FD7-4CEE-AB9B-A1D7CB641ABE}");
            public static readonly ID Title4 = new ID("{4A6C4ACA-2727-4735-91F1-61C431F45717}");
            public static readonly ID Description4 = new ID("{BED28A0C-B67A-41B3-88AF-0B420C5ABC0E}");
            public static readonly ID IsBlackFont4 = new ID("{07182A75-6549-4E5E-A067-0DC18C7E7E28}");
            public static readonly ID Banner5 = new ID("{093042B8-336F-414E-901D-7A482567406E}");
            public static readonly ID BannerLink5 = new ID("{5EA3B7A7-9290-4265-91AC-B812FF219593}");
            public static readonly ID Title5 = new ID("{E94FED94-6C48-4660-BA11-28FAB93956A4}");
            public static readonly ID Description5 = new ID("{F650EF0E-8340-4753-8D15-7E4D8784AD09}");
            public static readonly ID IsBlackFont5 = new ID("{8AD213FB-6AAD-4AF3-89B2-6CF1AAB39354}");
            public static readonly ID Banner6 = new ID("{360ED0C2-ED6E-46FC-9A2B-E0689767BF58}");
            public static readonly ID BannerLink6 = new ID("{3AE1F2CC-8667-4211-A868-5DB4746588CB}");
            public static readonly ID Title6 = new ID("{56208055-0A96-4679-9376-C968F5AD7505}");
            public static readonly ID Description6 = new ID("{D7DF6822-CB08-4507-9C88-A5F98ECE2250}");
            public static readonly ID IsBlackFont6 = new ID("{9906D85F-2EE6-4481-9C33-2EE94FF9D971}");
            public static readonly ID Banner7 = new ID("{BBB035E3-FBE5-45E2-AC1F-7943075D5D87}");
            public static readonly ID BannerLink7 = new ID("{3E69378C-D049-4565-85DE-83E3F7E91DEB}");
            public static readonly ID Title7 = new ID("{BCD51DC6-886F-4CDA-BC46-340BAF67F3C0}");
            public static readonly ID Description7 = new ID("{0ED48DEF-793A-4969-A793-EB5A697C6DEE}");
            public static readonly ID IsBlackFont7 = new ID("{635B4C8B-12E0-4630-8DCD-CBA36B942F7A}");
            public static readonly ID Banner8 = new ID("{CB3751AE-B5A1-43CB-BF5A-067014FFB0B6}");
            public static readonly ID BannerLink8 = new ID("{04C53B63-FD0C-4330-8876-AD77D980FEA4}");
            public static readonly ID Title8 = new ID("{C1EB3A1F-925B-4D1C-B486-0D197D17854A}");
            public static readonly ID Description8 = new ID("{56E8C752-E42E-4052-827B-2D421507382C}");
            public static readonly ID IsBlackFont8 = new ID("{106E1281-2A93-4577-8CB1-6872BEFAB405}");
            public static readonly ID FundIDList = new ID("{420891A7-1186-4135-B33F-14DEEB15FCD1}");
        }
    }
}
