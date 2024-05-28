using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Account.Models.MemberCard
{
   public class MemberCardModel
    {
        public Item DataSource { get; set; }
        public string Title { get; set; }
        public string ButtonLink { get; set; }
        public MemberCardModel(Item item)
        {
            this.DataSource = item;
        }

    }
    public struct Templates
    {
        public struct MemberCard
        {
            public static readonly ID Id = new ID("{F39D4AB0-FB3F-4D98-BE8E-255A5BD8A000}");

            public struct Fields
            {
                /// <summary>
                /// 標題
                /// </summary>
                public static readonly ID Title = new ID("{9E20BEB5-61A3-4D9E-B9D2-E8285C159235}");

                /// <summary>
                /// 按鈕連結
                /// </summary>
                public static readonly ID ButtonLink = new ID("{1C970B30-C780-4ECB-AFB0-354E544844B3}");

            }
        }
    }

}
