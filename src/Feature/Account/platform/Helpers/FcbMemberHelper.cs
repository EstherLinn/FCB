using Feature.Wealth.Account.Models.OAuth;
using Newtonsoft.Json;

namespace Feature.Wealth.Account.Helpers
{
   public class FcbMemberHelper
    {
        public static bool CheckMemberLogin()
        {
            return Sitecore.Context.IsLoggedIn && GetMemberAllInfo() != null;

        }
        public static FcbMemberModel GetMemberAllInfo()
        {
            return JsonConvert.DeserializeObject<FcbMemberModel>(Sitecore.Context.User.Profile.GetCustomProperty("MemberInfo"));
        }

        public static string GetMemberPlatFormId()
        {
            return GetMemberAllInfo().PlatFormId;
        }
    }
}
