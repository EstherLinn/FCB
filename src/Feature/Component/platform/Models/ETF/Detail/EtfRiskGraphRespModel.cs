using System.Collections.Generic;
using System.Net;

namespace Feature.Wealth.Component.Models.ETF.Detail
{
    public class EtfRiskGraphRespModel : IRespModel<IEnumerable<EtfRiskGraph>>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<EtfRiskGraph> Body { get; set; }
    }
}