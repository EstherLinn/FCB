using System;

namespace Feature.Wealth.Account.Models.MemberLog
{
    public class MemberLog
    {
        public string PlatForm { get; set; }
        public string PlatFormId { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public string ClientIp { get; set; }
        public string Port { get; set; }
        public string Browser { get; set; }
        public DateTime ActionTime { get; set; }

    }
}
