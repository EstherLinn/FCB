using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Account.Helpers
{
  public static  class MemberRelatedLinkHelper
    {
        public struct Template
        {
            public struct MemberRelatedLink
            {
                public static readonly ID Root = new ID("{1D504FEF-3A5D-41A5-AFFF-96555715C615}");

                public struct Fields
                {
                    public static readonly ID MemberSetting = new ID("{B40E99A5-246F-476D-AA51-425D3E83C648}");
                    public static readonly ID FocusList = new ID("{4DD89F12-BD12-4B73-9001-1231EFB8078C}");
                    public static readonly ID ExclusiveRecommendation = new ID("{E6D42BD5-1072-44AF-9490-9BAD198215DE}");
                    public static readonly ID FinancialManagementTrial = new ID("{925BAF2B-ED54-4676-816B-BA9C6C16253D}");
                    public static readonly ID FavoriteNews = new ID("{1F3C07C7-D767-48B9-893D-B3CC076BBF93}");
                    public static readonly ID RemoteFinancialConsulting = new ID("{F4D0C8CC-6F2B-4204-9A62-3821C97A4738}");
                    public static readonly ID ReserveConsulting = new ID("{0FE5783A-C6DC-48E1-83EF-8735C5FC5FE5}");
                    public static readonly ID InfoList = new ID("{85F0E5A8-BC5D-4C95-9FAA-25E6636FC678}");
                }
            }

        }
        /// <summary>
        /// 取得個人設定連結
        /// </summary>
        /// <returns></returns>
        public static string GetMemberSettingUrl()
        {
            Item MemberRealtedItem = ItemUtils.GetItem(Template.MemberRelatedLink.Root);
            return MemberRealtedItem.GeneralLink(Template.MemberRelatedLink.Fields.MemberSetting)?.Url;
        }
        /// <summary>
        /// 取得關注清單連結
        /// </summary>
        /// <returns></returns>
        public static string GetFocusListUrl()
        {
            Item MemberRealtedItem = ItemUtils.GetItem(Template.MemberRelatedLink.Root);
            return MemberRealtedItem.GeneralLink(Template.MemberRelatedLink.Fields.FocusList)?.Url;
        }
        /// <summary>
        /// 取得專屬推薦連結
        /// </summary>
        /// <returns></returns>
        public static string GetExclusiveRecommendationUrl()
        {
            Item MemberRealtedItem = ItemUtils.GetItem(Template.MemberRelatedLink.Root);
            return MemberRealtedItem.GeneralLink(Template.MemberRelatedLink.Fields.ExclusiveRecommendation)?.Url;
        }
        /// <summary>
        /// 取得理財試算連結
        /// </summary>
        /// <returns></returns>
        public static string GetFinancialManagementTrialUrl()
        {
            Item MemberRealtedItem = ItemUtils.GetItem(Template.MemberRelatedLink.Root);
            return MemberRealtedItem.GeneralLink(Template.MemberRelatedLink.Fields.FinancialManagementTrial)?.Url;
        }
        /// <summary>
        /// 取得收藏新聞連結
        /// </summary>
        /// <returns></returns>
        public static string GetFavoriteNewsUrl()
        {
            Item MemberRealtedItem = ItemUtils.GetItem(Template.MemberRelatedLink.Root);
            return MemberRealtedItem.GeneralLink(Template.MemberRelatedLink.Fields.FavoriteNews)?.Url;
        }
        /// <summary>
        /// 取得遠距理財連結
        /// </summary>
        /// <returns></returns>
        public static string GetRemoteFinancialConsultingUrl()
        {
            Item MemberRealtedItem = ItemUtils.GetItem(Template.MemberRelatedLink.Root);
            return MemberRealtedItem.GeneralLink(Template.MemberRelatedLink.Fields.RemoteFinancialConsulting)?.Url;
        }
        /// <summary>
        /// 取得預約諮詢連結
        /// </summary>
        /// <returns></returns>
        public static string GetReserveConsultingUrl()
        {
            Item MemberRealtedItem = ItemUtils.GetItem(Template.MemberRelatedLink.Root);
            return MemberRealtedItem.GeneralLink(Template.MemberRelatedLink.Fields.ReserveConsulting)?.Url;
        }
        /// <summary>
        /// 取得通知列表連結
        /// </summary>
        /// <returns></returns>
        public static string GetInfoListUrl()
        {
            Item MemberRealtedItem = ItemUtils.GetItem(Template.MemberRelatedLink.Root);
            return MemberRealtedItem.GeneralLink(Template.MemberRelatedLink.Fields.InfoList)?.Url;
        }

    }
}
