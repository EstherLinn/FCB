using Feature.Wealth.Account.Filter;
using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Component.Models.ExclusiveRecommendation;
using Feature.Wealth.Component.Repositories;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class ExclusiveRecommendationController : Controller
    {
        private readonly ExclusiveRecommendationRepository _exclusiveRecommendationRepository;

        public ExclusiveRecommendationController()
        {
            _exclusiveRecommendationRepository = new ExclusiveRecommendationRepository();
        }
        [MemberAuthenticationFilter]
        public ActionResult Index()
        {
            var item = RenderingContext.CurrentOrNull?.Rendering.Item;
            var model = new ExclusiveRecommendationModel(item);
            if (!string.IsNullOrEmpty(FcbMemberHelper.GetMemberWebBankId()))
            {
                if (!string.IsNullOrEmpty(FcbMemberHelper.GetMemberRisk()))
                {
                    model.FundTopList = _exclusiveRecommendationRepository.GetTopRiskList(FcbMemberHelper.GetMemberRisk());
                }
                model.FundSameAgeCard = _exclusiveRecommendationRepository.GetSameAgeCard(_exclusiveRecommendationRepository.CalculateAge(FcbMemberHelper.GetMemberBirthDay()));
                model.FundSameZodiacCard = _exclusiveRecommendationRepository.GetZodiacCard(_exclusiveRecommendationRepository.CalculateZodiac(FcbMemberHelper.GetMemberBirthDay()));
                List<string> ids = new List<string>();
                if (model.FundTopList != null)
                {
                    ids.AddRange(model.FundTopList.Select(x => x.ProductCode).ToList());
                }
                ids.AddRange(model.FundSameAgeCard.Select(x => x.ProductCode).ToList());
                ids.AddRange(model.FundSameZodiacCard.Select(x => x.ProductCode).ToList());
                model.FundFullChart = _exclusiveRecommendationRepository.GetChartData(ids.Distinct().ToList());
            }
            return View("/Views/Feature/Wealth/Component/ExclusiveRecommendation/ExclusiveRecommendation.cshtml", model);
        }
    }
}
