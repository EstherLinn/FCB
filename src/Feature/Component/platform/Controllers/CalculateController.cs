using Feature.Wealth.Component.Repositories;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Feature.Wealth.Component.Models.Calculate;
using System.Collections.Generic;

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

        /// <summary>
        ///  買房首付試算
        /// </summary>
        public ActionResult BuyHouse()
        {
            var model = _calculateRepository.GetCalculateModel();

            return View("/Views/Feature/Wealth/Component/Calculate/BuyHouse.cshtml", model);
        }

        /// <summary>
        ///  退休準備試算
        /// </summary>
        public ActionResult RetirementPreparation()
        {
            var model = _calculateRepository.GetCalculateModel();

            return View("/Views/Feature/Wealth/Component/Calculate/RetirementPreparation.cshtml", model);
        }

        /// <summary>
        /// 儲存試算結果
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCalculationResults(CalculationResultData data)
        {
            object objReturn = new
            {
                success = _calculateRepository.UpdateCalculationResults(data)
            };

            return new JsonNetResult(objReturn);
        }

        /// <summary>
        /// 獲取基金資料
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetFundData(string ExpectedRoi, string[] ProductFundIDs)
        {
            List<FundModel> datas;

            datas = _calculateRepository.GetFundData(ExpectedRoi, ProductFundIDs);

            return new JsonNetResult(datas);
        }

        /// <summary>
        /// 獲取ETF資料
        /// </summary>
        [HttpPost]
        public ActionResult GetEtfData(string ExpectedRoi, string RiskLevel)
        {
            List<EtfModel> datas;

            datas = _calculateRepository.GetEtfData(ExpectedRoi, RiskLevel);

            return new JsonNetResult(datas);
        }
    }
}