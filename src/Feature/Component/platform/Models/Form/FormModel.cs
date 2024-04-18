using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.Form
{
    public class FormModel
    {
        public Item DataSource { get; set; }
        public IList<Form> Items { get; set; }

        public class Form
        {
            public Form(Item item)
            {
                Item = item;
            }
            public Item Item { get; set; }
        }

    }

    public struct Templates
    {
        public struct Form
        {
            public static readonly ID Id = new ID("{E8B7E902-DCAE-42F7-A6A5-F97B0AE3C44E}");

            public struct Fields
            {
                /// <summary>
                /// 主標題
                /// </summary>
                public static readonly ID MainTitle = new ID("{004A4DE2-F1BB-4105-9970-06E30F732CB6}");

                /// <summary>
                /// 描述
                /// </summary>
                public static readonly ID Description = new ID("{9F35BD49-C3C9-4324-91E6-51296DA8343D}");

                /// <summary>
                /// 副標題
                /// </summary>
                public static readonly ID Subtitle = new ID("{CDA5ACE8-C1C4-495C-9E70-C8DF83A1F061}");

                /// <summary>
                /// 表格類型
                /// </summary>
                public static readonly ID FormType = new ID("{983114A4-E50F-4698-8ACA-EF229770BCF9}");

                /// <summary>
                /// 註解
                /// </summary>
                public static readonly ID Notes = new ID("{2B1FED26-3F69-49AC-88FC-0557F73E7D39}");

                /// <summary>
                /// 表格標題-第1欄
                /// </summary>
                public static readonly ID Title1 = new ID("{51CF8DDA-310D-4B43-B298-7F89C2DE5143}");

                /// <summary>
                /// 表格標題-第2欄
                /// </summary>
                public static readonly ID Title2 = new ID("{01A3D48D-ECF8-4B71-B989-57C65DAAF491}");

                /// <summary>
                /// 表格標題-第3欄
                /// </summary>
                public static readonly ID Title3 = new ID("{6F3C5899-83F0-4E7E-9F74-EAC03733BCCB}");

                /// <summary>
                /// 表格標題-第4欄
                /// </summary>
                public static readonly ID Title4 = new ID("{1312F64C-5CF5-4C98-A581-BBFFE23380BF}");

                /// <summary>
                /// 表格標題-第5欄
                /// </summary>
                public static readonly ID Title5 = new ID("{1C782EDB-58D4-4D4B-8D2D-5975A9BB094B}");

                /// <summary>
                /// 表格標題-第6欄
                /// </summary>
                public static readonly ID Title6 = new ID("{F86B24D9-0AD2-4942-BD86-4171454D4215}");
            }
        }

        public struct FormData
        {
            public static readonly ID Id = new ID("{9D2C445B-5B59-4408-BBB3-7FDADDE9E085}");

            public struct Fields
            {
                /// <summary>
                /// 表格內容-第1欄
                /// </summary>
                public static readonly ID column1 = new ID("{A2C07241-CD35-45C7-B54F-DE62DF82FB14}");

                /// <summary>
                /// 表格內容-第2欄
                /// </summary>
                public static readonly ID column2 = new ID("{90861EC9-F290-431E-9BA9-8DED228A2B84}");

                /// <summary>
                /// 表格內容-第3欄
                /// </summary>
                public static readonly ID column3 = new ID("{7BC8A242-207D-437E-AC0F-9BEBBDF2E744}");

                /// <summary>
                /// 表格內容-第4欄
                /// </summary>
                public static readonly ID column4 = new ID("{135DD954-2EC8-46EC-85DA-FA98FF6A223B}");

                /// <summary>
                /// 表格內容-第5欄
                /// </summary>
                public static readonly ID column5 = new ID("{F152BD8C-FFF6-4C50-A7FB-1E7CBEA5A72F}");

                /// <summary>
                /// 表格內容-第6欄
                /// </summary>
                public static readonly ID column6 = new ID("{8FB55716-02D4-4C36-9B5B-814936653E9B}");
            }
        }
    }
}
