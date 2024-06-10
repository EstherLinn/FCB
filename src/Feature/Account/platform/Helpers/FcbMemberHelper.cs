using Feature.Wealth.Account.Models.OAuth;
using Newtonsoft.Json;

namespace Feature.Wealth.Account.Helpers
{
   public class FcbMemberHelper
    {
        public static FcbMemberModel fcbMemberModel => JsonConvert.DeserializeObject<FcbMemberModel>(Sitecore.Context.User.Profile.GetCustomProperty("MemberInfo"));
        public static bool CheckMemberLogin()
        {
            return Sitecore.Context.IsLoggedIn && fcbMemberModel != null;

        }
        public static FcbMemberModel GetMemberAllInfo()
        {
            return fcbMemberModel;
        }
        public static QuoteChangeEunm GetMemberStyleGlobal()
        {
            return fcbMemberModel.StockShowColor;
        }
        public static bool IsMemberStyleGlobal()
        {
            return fcbMemberModel.StockShowColor == QuoteChangeEunm.International;
        }
        public static string GetMemberPlatFormId()
        {
            return fcbMemberModel.PlatFormId;
        }
        public static PlatFormEunm GetMemberPlatForm()
        {
            return fcbMemberModel.PlatForm ;
        }
        public static string GetMemberWebBankId()
        {
            return fcbMemberModel.WebBankId;
        }
    }
}
