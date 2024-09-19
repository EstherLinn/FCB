using System;

namespace Feature.Wealth.Toolkit.Models.TableViewer.SitecoreMemberRecord
{
    internal class AuthenticationHistory
    {
        public long Id { get; set; }
        public string Action { get; set; }
        public string UserName { get; set; }
        public DateTime Created { get; set; }
    }
}