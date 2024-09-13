﻿using Feature.Wealth.Service.Models.FundApi;
using Feature.Wealth.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Service.Controllers
{
    public class FundApiController : JsonNetController
    {
        private readonly FundApiRepository _fundRepository = new FundApiRepository();

        [HttpGet]
        public ActionResult FundCorpJson()
        {
            var result = new FundCompanyList()
            {
                Funds = _fundRepository.GetFundCompany("O"),
                WFunds = _fundRepository.GetFundCompany("D")
            };
            return new JsonNetResult(result);
        }

        [HttpGet]
        public ActionResult FundTargetJson()
        {
            var result = _fundRepository.GetInvestmentTargets();

            return new JsonNetResult(result);
        }
    }
}
