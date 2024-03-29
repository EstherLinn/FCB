using System.Collections.Generic;
using System.Net;

namespace Feature.Wealth.Component.Models.FundDetail
{
    public class FundRiskGraphRespModel : IRespModel<IEnumerable<FundRiskGraph>>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<FundRiskGraph> Body { get; set; }
    }
}
