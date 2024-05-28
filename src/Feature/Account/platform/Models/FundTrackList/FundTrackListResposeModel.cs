using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Feature.Wealth.Account.Models.FundTrackList
{
   public class FundTrackListResposeModel
    {
        [JsonProperty("promotionCode")]
        public string PromotionCode { get; set; }

        [JsonProperty("trackLists")]
        public List<TrackList> TrackLists { get; set; }

        [JsonProperty("rt")]

        public string Rt { get; set; }

        public class TrackList
        {
            [JsonProperty("fundCode")]
            public string FundCode { get; set; }

            [JsonProperty("fundType")]
            public string FundType { get; set; }

        }
    }
}
