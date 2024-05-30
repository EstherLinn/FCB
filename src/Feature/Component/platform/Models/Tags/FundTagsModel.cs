using System.Collections.Generic;
using Feature.Wealth.Component.Models.FundDetail;

namespace Feature.Wealth.Component.Models.Tags
{
    public class FundTagsModel
    {
        public class Tags
        {
            public string TagName { get; set; }
            public List<string> ProductCodes { get; set; }
            public FundTagEnum FundTagType { get; set; }
        }

        public struct Template
        {
            public struct Fields
            {
                /// <summary>
                /// 標籤Template
                /// </summary>
                public static readonly string FundTags = "{9EC537F8-9EF9-4AF0-B6B6-A66749E3F22A}";

                /// <summary>
                /// TagsFolder
                /// </summary>
                public static readonly string TagsFolder = "{417B3FFD-E169-4374-B0D5-A8230FE32F5C}";

                /// <summary>
                /// TagsFolderTemplate有標籤種類選擇
                /// </summary>
                public static readonly string TagFolder = "{610A8811-FFD0-46D2-B58D-D4CC298E6589}";

                /// <summary>
                /// Tag的名稱
                /// </summary>
                public static readonly string TagName = "{E5B26CC0-95C1-4C79-A141-FFDFA762434F}";

                /// <summary>
                /// ProductCodeList
                /// </summary>
                public static readonly string ProductCodeList = "{97F5D259-74DE-4B64-A4A9-EFAC9C0A0CD0}";

                /// <summary>
                /// 標籤分類
                /// </summary>
                public static readonly string FundType = "{09FFCB38-4D8A-4954-8ED5-DE917E06F8E7}";
            }

            public struct TagType
            {
                /// <summary>
                /// 優惠標籤
                /// </summary>
                public static readonly string DiscountTag = "{BEDE9AB5-AD58-4E49-AE0F-4F41C1A5091A}";

                /// <summary>
                /// 分類標籤
                /// </summary>
                public static readonly string SortTag = "{785D18F9-B525-4348-9A67-8449068B018D}";

                /// <summary>
                /// 熱門關鍵字標籤
                /// </summary>
                public static readonly string KeywordTag = "{84218B07-D105-47DE-8435-7EC8EFDBC969}";
            }

        }
    }
}
