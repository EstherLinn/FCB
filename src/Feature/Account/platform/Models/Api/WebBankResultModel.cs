using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Account.Models.Api
{
   public class WebBankResultModel
    {
        public string txReqId { get; set; }
        public string LoginResult { get; set; }
        public string LoginDttm { get; set; }
        public string errMsg { get; set; }
        public string fnct { get; set; }
        public custDataModel custData { get; set; }
        public string sign { get; set; }

        public class custDataModel
        {
            public string custId { get; set; }

        }
    }
}
