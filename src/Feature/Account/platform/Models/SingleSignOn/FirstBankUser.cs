namespace Feature.Wealth.Account.Models.SingleSignOn
{
    public enum SsoDomain
    {
        /// <summary>
        /// 第一銀行
        /// </summary>
        fcb = 10
    }

    public class TokenModel
    {
        /// <summary>
        /// X-workforceID 員工編號
        /// </summary>
        public string WorkForceId { get; set; }
    }

    public class FirstBankUser : ISsoUser
    {
        public FirstBankUserProfile Profile { get; } = new();

        public string LocalName => this.Profile?.EmployeeId ?? "";
    }

    public class FirstBankUserProfile
    {
        /// <summary>
        /// 員工信箱
        /// </summary>
        public string EmployeeEmail { get; set; }

        #region === Custom Property ===

        /// <summary>
        /// 員工代碼
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// 員工姓名
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 子公司代碼
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        /// 事業群代碼
        /// </summary>
        public string BusinessGroupCode { get; set; }

        /// <summary>
        /// 事業群名稱
        /// </summary>
        public string BusinessGroupName { get; set; }

        /// <summary>
        /// 分行代碼
        /// </summary>
        public string BranchCode { get; set; }

        /// <summary>
        /// 分行名稱
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// 部門代碼
        /// </summary>
        public string DepartmentCode { get; set; }

        /// <summary>
        /// 部門名稱
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 職務代碼
        /// </summary>
        public string JobCode { get; set; }

        /// <summary>
        /// 職務名稱
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 職稱代碼
        /// </summary>
        public string JobTitleCode { get; set; }

        /// <summary>
        /// 職稱名稱
        /// </summary>
        public string JobTitleName { get; set; }

        /// <summary>
        /// 職位代碼
        /// </summary>
        public string PositionCode { get; set; }

        /// <summary>
        /// 職位名稱
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// 直屬主管
        /// </summary>
        public string Supervisor { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string Roles { get; set; }

        #endregion === Custom Property ===
    }
}