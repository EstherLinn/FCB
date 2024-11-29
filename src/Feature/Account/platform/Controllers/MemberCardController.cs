using Feature.Wealth.Account.Filter;
using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Account.Models.Consult;
using Feature.Wealth.Account.Models.MemberCard;
using Feature.Wealth.Account.Repositories;
using Sitecore.Mvc.Presentation;
using System;
using System.Web.Mvc;

namespace Feature.Wealth.Account.Controllers
{
    [MemberAuthenticationFilter]
    public class MemberCardController : Controller
    {
        public ActionResult Index()
        {
            var model = RenderingContext.CurrentOrNull?.Rendering.Item;
            var viewModel = new MemberCardViewModel();
            viewModel.MemberCardModel = new MemberCardModel(model);
            if (!string.IsNullOrEmpty(FcbMemberHelper.GetMemberWebBankId()))
            {
                MemberRepository memberRepository = new MemberRepository();
                if (!ConsultRelatedLinkSetting.GetIsMaintain())
                {
                    viewModel.ScheduleDate = memberRepository.GetMemberScheduleDate();
                    viewModel.ScheduleSpace = CalculateDays(viewModel.ScheduleDate);
                }
                var member = FcbMemberHelper.GetMemberAllInfo();

                if (!member.IsManager && member.IsEmployee)
                {
                    viewModel.ScheduleMessage = ConsultRelatedLinkSetting.GetIsMaintain() ? null : memberRepository.GetAdvisrorScheduleMessage();
                }
                else if(!member.IsManager && !member.IsEmployee)
                {
                    viewModel.ScheduleMessage = ConsultRelatedLinkSetting.GetIsMaintain() ? null : memberRepository.GetMemberScheduleMessage();
                    viewModel.BranchInfo = memberRepository.GetMainBranchInfoByBranchCode(member.MainBranchCode);
                }
                if (member.IsManager || member.IsEmployee)
                {
                    viewModel.BranchInfo = memberRepository.GetMainBranchInfoByEmployee(member.AdvisrorID);
                }
            }
            return View("~/Views/Feature/Wealth/Account/MemberCard/MemberCard.cshtml", viewModel);
        }

        private int CalculateDays(DateTime? date)
        {
            if (!date.HasValue)
            {
                return 0;
            }
            DateTime td = DateTime.Today;
            TimeSpan difference = td - date.Value;
            return Math.Abs(difference.Days);
        }

    }
}