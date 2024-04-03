using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.FundDetail
{
    public  class FundScaleRecord
    {
        public string FirstBankCode { get; set; }
        public string ScaleDate { get; set; }
        public decimal Scale { get; set; }
        public string Currency { get; set; }

    }
}
