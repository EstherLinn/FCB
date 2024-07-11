using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Component.Repositories;
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
        public ActionResult SetInfoHaveReadByMember(string mailInfoType, int recordNumber)
        {
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new EmptyResult();
            }
            var obj = new
            {
                success = _infoListRepository.SetInfoHaveReadByMember(mailInfoType, recordNumber)
            };
            return new JsonNetResult(obj);
        }

        [HttpPost]
        public ActionResult SetAllInfoHaveReadByMember()
        {
            if (!FcbMemberHelper.CheckMemberLogin())
            {
                return new EmptyResult();
            }
            var obj = new
            {
                success = _infoListRepository.SetAllInfoHaveReadByMember()
            };
            return new JsonNetResult(obj);
        }
    }
}
