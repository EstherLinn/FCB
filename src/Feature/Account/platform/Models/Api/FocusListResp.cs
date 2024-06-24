using System.Collections.Generic;

namespace Feature.Wealth.Account.Models.Api
{
    public class FocusListResp
    {
        public string promotionCode { get; set; }
        public List<TrackModel> TrackList { get; set; }
        public string rt { get; set; }      
    }
    public class TrackModel
    {
        public string fundCode { get; set; }
        public string fundType { get; set; }

    }
}
