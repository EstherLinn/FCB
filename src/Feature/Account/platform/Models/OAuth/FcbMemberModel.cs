

namespace Feature.Wealth.Account.Models.OAuth
{
    public class FcbMemberModel
    {
        public string WebBankId { get; set; }
        public string MemberName { get; set; }
        public string MemberEmail { get; set; }

        public bool VideoInfoOpen { get; set; }
        public bool ArrivedInfoOpen { get; set; }

        public QuoteChangeEunm StockShowColor { get; set; }

        public string PlatForm { get; set; }
        public string PlatFormId { get; set; }

    }
}
