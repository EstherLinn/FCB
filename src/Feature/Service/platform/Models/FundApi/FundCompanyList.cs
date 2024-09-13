using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Service.Models.FundApi
{
    public class FundCompanyList
    {
        [JsonProperty("fund")]
        public List<Dictionary<string, string>> Funds { get; set; }
        [JsonProperty("wfund")]
        public List<Dictionary<string, string>> WFunds { get; set; }
    }
}
