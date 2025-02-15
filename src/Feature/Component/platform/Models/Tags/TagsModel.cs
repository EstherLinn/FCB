﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.Tags
{
    public class TagsModel
    {
        public class Tags
        {
            public string TagName { get; set; }
            public List<string> ProductCodes { get; set; }
            public string FundType { get; set; }
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
                public static readonly string FundType = "{33FD775E-EBCB-40E3-A7C3-3B7C5528006A}";
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
            }

        }
    }
}
