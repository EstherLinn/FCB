namespace Feature.Wealth.Account.Models.OAuth
{
    public class Employee
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeID { get; set; }
        public string SubsidiaryCode { get; set; }
        public string BusinessGroup { get; set; }
        public string BusinessName { get; set; }
        public string OfficeOrBranchCode { get; set; }
        public string OfficeOrBranchName { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string PositionCode { get; set; }
        public string PositionName { get; set; }
        public string JobTitleCode { get; set; }
        public string JobTitleName { get; set; }
        public string JobPositionCode { get; set; }
        public string JobPositionName { get; set; }
        public string Supervisor { get; set; }
        public string AppointmentDate { get; set; }
        public string Attendancebranch { get; set; }
        public string SupervisorCode { get; set; }
        public string SupervisorName { get; set; }
        public string HaveTrustLicense { get; set; }
        public string HaveInsurancseLicense { get; set; }
        public string InvestmentQualifications { get; set; }
        public string DerivativeFinancialOrStructuredQualifications { get; set; }
        public string PersonalFinanceBusinessPersonnelCategory { get; set; }
        public string RegionalCenterBusinessGroup { get; set; }
        public string SalaryLevel { get; set; }
        public string SalaryScale { get; set; }

        /// <summary>
        /// 是否為理顧
        /// </summary>
        public bool IsEmployee { get; set; } = false;

        /// <summary>
        /// 是否為理顧主管
        /// </summary>
        public bool IsManager { get; set; } = false;
    }
}
