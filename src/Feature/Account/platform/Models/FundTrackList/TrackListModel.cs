
using Newtonsoft.Json;

namespace Feature.Wealth.Account.Models.FundTrackList
{
   public class TrackListModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("trackDate")]
        public string TrackDate { get; set; }
    }
}
