using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Feature.Wealth.Account.Models.Api
{
   public class CommonFuncrionsResp: IRespModel<IEnumerable<CommonFunctionsModel>>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<CommonFunctionsModel> Body { get; set; }

        public CommonFuncrionsResp() {
            StatusCode = HttpStatusCode.OK;
            Message = string.Empty;
            Body = Enumerable.Empty<CommonFunctionsModel>().ToList();
        }
    }
}
