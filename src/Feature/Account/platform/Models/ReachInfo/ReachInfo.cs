﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Account.Models.ReachInfo
{
    public class ReachInfo
    {
        [JsonIgnore]
        public string PlatFormId { get; set; }
        public string InvestType { get; set; }
        public string InvestId { get; set; }
        public decimal? PriceValue { get; set; }
        public string InfoType { get; set; }
        public decimal? ReachValue { get; set; }
        public decimal? RiseValue { get; set; }
        public decimal? FallValue { get; set; }
        public decimal? RisePercent { get; set; }
        public decimal? FallPercent { get; set; }

        public DateTime? InfoStartDate { get; set; }
        public DateTime? InfoEndDate { get; set; }
        public DateTime? SetDateTime { get; set; }
        public bool OpenInfo { get; set; }

    }
}
