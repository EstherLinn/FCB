using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Service.Models.WhiteListIp
{
    public class ApiWhiteListSetting
    {
        public static List<string> ApiWhiteList()
        {
            List<string> result = new List<string>();
            Item item = ItemUtils.GetItem(Template.WhiteList.Root);

            //未上節點，預設通過
            if (item == null)
            {
                return null;
            }

            string whiteListString = ItemUtils.GetFieldValue(item, Template.WhiteList.Fields.IPList);

            if (!string.IsNullOrEmpty(whiteListString))
            {
                result = whiteListString.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            }

            return result;
        }

        public static bool CkeckApiAllow()
        {
            bool result = false;
            Item item = ItemUtils.GetItem(Template.WhiteList.Root);

            if (item == null)
            {
                return true;
            }

            var ipAllow = ItemUtils.GetFieldValue(item, Template.WhiteList.Fields.IPAllow);
            if (ipAllow == "1")
            {
                result = true;
                return result;
            }
            return result;
        }
    }

    public struct Template
    {
        public struct WhiteList
        {
            public static readonly ID Root = new ID("{A83635DC-5D9F-46AC-842B-97B5629A8CCE}");

            public struct Fields
            {
                /// <summary>
                /// IP白名單
                /// </summary>
                public static readonly string IPList = "{196B6280-ED1C-45FD-9CD7-E97B9E2F9F7F}";

                public static readonly string IPAllow = "{A8E18B9D-E778-41F0-9028-0460CB9D930C}";
            }
        }

    }
}