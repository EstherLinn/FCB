using Feature.Wealth.Account.Models.OAuth;
using Newtonsoft.Json;
using System;
using Feature.Wealth.Account.Models.Consult;
using System.Linq;
using Feature.Wealth.Account.Repositories;

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
            return fcbMemberModel.StockShowColor == QuoteChangeEunm.International || fcbMemberModel.StockShowColor == QuoteChangeEunm.InternationalArrow;
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

        public static DateTime? GetMemberBirthDay()
        {
            return fcbMemberModel.Birthday;
        }
        public static string GetMemberRisk()
        {
            return fcbMemberModel.Risk;
        }
        public static string GetMemberGender()
        {
            return fcbMemberModel.Gender;
        }
        public static string GetMemberSalFalg()
        {
            return fcbMemberModel.SalFlag;
        }

        public static bool BranchCanUseConsult()
        {
            // 找分行代碼白名單
            var branchCodeList = ConsultRelatedLinkSetting.BranchCodeList();

            // 沒輸入就是全開放
            if(branchCodeList == null || !branchCodeList.Any())
            {
                return true;
            }

            if(fcbMemberModel == null)
            {
                return false;
            }

            if(string.IsNullOrEmpty(fcbMemberModel.AdvisrorID))
            {
                return branchCodeList.Contains(fcbMemberModel.MainBranchCode);
            }
            else
            {
                MemberRepository memberRepository = new MemberRepository();
                var branch = memberRepository.GetMainBranchInfoByEmployee(fcbMemberModel.AdvisrorID);
                return branchCodeList.Contains(branch.BranchCode);
            }
        }
    }
}
