using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.FundDetail
{
    public class FundTagModel
    {
        public string FundTagTitle { get; set; }

        public FundTagEnum FundTagType { get; set; }

        public List<string> FundIdList { get; set; }
    }
}
