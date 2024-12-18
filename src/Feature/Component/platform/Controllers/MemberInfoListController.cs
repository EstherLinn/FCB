using Feature.Wealth.Account.Filter;
using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Component.Repositories;
using Foundation.Wealth.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Controllers
{
    public class MemberInfoListController : JsonNetController
    {
        private readonly InfoListRepository _infoListRepository;
        public MemberInfoListController()
        {
            _infoListRepository = new InfoListRepository();
        }
        public ActionResult Index()
        {

            return View("/Views/Feature/Wealth/Component/MemberInfoList/MemberInfoList.cshtml");
        }
        [HttpPost]
        public ActionResult GetAllInfoByMember()
        {
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new EmptyResult();
            }
            return new JsonNetResult(_infoListRepository.GetAllInfoByMember());
        }

        [HttpPost]
        public ActionResult GetTopFiveInfoByMember()
        {
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new EmptyResult();
            }
            return new JsonNetResult(_infoListRepository.GetTopFiveInfoByMember());
        }

        [HttpPost]
        [MemberAuthenticationFilter]
        public ActionResult SetInfoHaveReadByMember(string mailInfoType, int recordNumber)
        {
            object obj = new
            {
                success = false,
                block = false
            };
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                obj = new
                {
                    success = false,
                    block = true
                };
                return new JsonNetResult(obj);
            }
            obj = new
            {
                success = _infoListRepository.SetInfoHaveReadByMember(InputSanitizerHelper.InputSanitizer(mailInfoType), recordNumber)
            };
            return new JsonNetResult(obj);
        }

        [HttpPost]
        [MemberAuthenticationFilter]
        public ActionResult SetAllInfoHaveReadByMember()
        {
            object obj = new
            {
                success = false,
                block = false
            };
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                obj = new
                {
                    success = false,
                    block = true
                };
                return new JsonNetResult(obj);
            }
            obj = new
            {
                success = _infoListRepository.SetAllInfoHaveReadByMember()
            };
            return new JsonNetResult(obj);
        }
    }
}
