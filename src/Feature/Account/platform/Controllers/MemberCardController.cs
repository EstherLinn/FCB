using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Feature.Wealth.Account.Filter;
using Sitecore.Mvc.Presentation;
using Feature.Wealth.Account.Models.MemberCard;
using Feature.Wealth.Account.Repositories;
using Feature.Wealth.Account.Helpers;

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
                viewModel.ScheduleDate = memberRepository.GetMemberScheduleDate();
                viewModel.ScheduleSpace = CalculateDays(viewModel.ScheduleDate);
                var member = FcbMemberHelper.GetMemberAllInfo();

                if (!member.IsManager && member.IsEmployee)
                {
                    viewModel.ScheduleMessage = memberRepository.GetAdvisrorScheduleMessage();
                }
                else if(!member.IsManager && !member.IsEmployee)
                {
                    viewModel.ScheduleMessage = memberRepository.GetMemberScheduleMessage();
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