using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Feature.Wealth.Account.Models.ReachInfo
{
    public  class ReachInfoResp : IRespModel<IEnumerable<ReachInfo>>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<ReachInfo> Body { get; set; }

        public ReachInfoResp()
        {
            StatusCode = HttpStatusCode.OK;
            Message = string.Empty;
            Body = Enumerable.Empty<ReachInfo>().ToList();
        }
    }
}
