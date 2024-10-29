using System;

namespace Feature.Wealth.Toolkit.Models.TableViewer.SitecoreMemberRecord
{
    internal class AuthenticationHistory
    {
        public long Id { get; set; }
        public string Action { get; set; }
        public string UserName { get; set; }
        public DateTime Created { get; set; }
        public string FullName { get; set; }
        public string DepartmentName { get; set; }
        public string Roles { get; set; }
        public string IP { get; set; }
    }
}