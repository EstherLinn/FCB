using Sitecore.Services.Infrastructure.Security;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Feature.Wealth.Account.Models.Api
{
    public class CommonFuncrionsResp : IRespModel<IEnumerable<CommonFunctionsModel>>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<CommonFunctionsModel> Body { get; set; }

        public CommonFuncrionsResp()
        {
            StatusCode = HttpStatusCode.OK;
            Message = string.Empty;
            Body = Enumerable.Empty<CommonFunctionsModel>().ToList();
        }
    }

    public class CommonToolsRespResp : CommonFuncrionsResp
    {
        public IEnumerable<CommonFunctionsModel> Tools { get; set; }

        public CommonToolsRespResp() : base()
        {
           var tools = Enumerable.Empty<CommonFunctionsModel>().ToList();
            tools.Add(new CommonFunctionsModel
            {
                PageGuid = "{1D04400C-B8CF-4D54-BC3B-F642D77E1296}"
            });
            this.Tools = tools;
        }
    }
}
