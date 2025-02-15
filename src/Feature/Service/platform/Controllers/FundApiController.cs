﻿using Feature.Wealth.Service.Filter;
using Feature.Wealth.Service.Models.FundApi;
using Feature.Wealth.Service.Repositories;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Service.Controllers
{
    public class FundApiController : JsonNetController
    {
        private readonly FundApiRepository _fundRepository = new FundApiRepository();

        [HttpGet]
        [AuthorizationFilter]
        [OutputCache(Duration = 30)]
        public ActionResult FundCorpJson()
        {
            var result = new FundCompanyList()
            {
                Funds = _fundRepository.GetOrSetFundCompanyCache("O"),
                WFunds = _fundRepository.GetOrSetFundCompanyCache("D")
            };
            return new JsonNetResult(result);
        }

        [HttpGet]
        [AuthorizationFilter]
        [OutputCache(Duration = 30)]
        public ActionResult FundTargetJson()
        {
            var result = _fundRepository.GetOrSetInvestmentTargets();

            return new JsonNetResult(result);
        }
    }
}