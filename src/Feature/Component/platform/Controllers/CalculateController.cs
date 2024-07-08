﻿using Feature.Wealth.Component.Repositories;
using System.Web.Mvc;

namespace Feature.Wealth.Component.Controllers
{
    public class CalculateController : Controller
    {
        private readonly CalculateRepository _calculateRepository = new CalculateRepository();

        /// <summary>
        ///  存一桶金試算
        /// </summary>
        public ActionResult Saving()
        {
            var model = _calculateRepository.GetCalculateModel();

            return View("/Views/Feature/Wealth/Component/Calculate/Saving.cshtml", model);
        }
        /// <summary>
        ///  教育基金試算
        /// </summary>
        public ActionResult EducationFund()
        {
            var model = _calculateRepository.GetCalculateModel();

            return View("/Views/Feature/Wealth/Component/Calculate/EducationFund.cshtml", model);
        }
    }
}