

using System.Collections.Generic;

namespace Feature.Wealth.Account.Models.OAuth
{
    public class FcbMemberModel
    {
        public string WebBankId { get; set; }
        public string MemberName { get; set; }
        public string MemberEmail { get; set; }

        public string Birthday { get; set; }
        public string Risk { get; set; }
        public string Advisror { get; set; }
        public string AdvisrorID { get; set; }

        public bool VideoInfoOpen { get; set; }
        public bool ArrivedInfoOpen { get; set; }

        public string CommonFunctions { get; set; }

        public QuoteChangeEunm StockShowColor { get; set; }

        public PlatFormEunm PlatForm { get; set; }
        public string PlatFormId { get; set; }

        public FcbMemberModel()
        {
        }

        public FcbMemberModel(string memberName, string memberEmail, bool videoInfoOpen,
            bool arrivedInfoOpen, QuoteChangeEunm stockShowColor, PlatFormEunm platForm, string platFormId)
        {
            this.MemberName = memberName;
            this.MemberEmail = memberEmail;
            this.VideoInfoOpen = videoInfoOpen;
            this.ArrivedInfoOpen = arrivedInfoOpen;
            this.StockShowColor = stockShowColor;
            this.PlatForm = platForm;
            this.PlatFormId = platFormId;
        }

        public FcbMemberModel(string webBankId, string memberName, string memberEmail,
            string risk, string advisror, string advisrorID, bool videoInfoOpen,
            bool arrivedInfoOpen, QuoteChangeEunm stockShowColor, PlatFormEunm platForm, string platFormId)
        {
            this.WebBankId = webBankId;
            this.MemberName = memberName;
            this.MemberEmail = memberEmail;
            this.Risk = risk;
            this.Advisror = advisror;
            this.AdvisrorID = advisrorID;
            this.VideoInfoOpen = videoInfoOpen;
            this.ArrivedInfoOpen = arrivedInfoOpen;
            this.StockShowColor = stockShowColor;
            this.PlatForm = platForm;
            this.PlatFormId = platFormId;
        }

    }
}
