
using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    /// <summary>
    /// 查詢客戶對應的理專資料，檔案名稱：HRIS.txt
    /// </summary>
    [Delimiter(",")]
    [HasHeaderRecord(false)]
    public class Hris
    {
        /// <summary>
        /// 員工代號
        /// </summary>
        [Index(0)]
        [NullValues("", "NULL", null)]
        public string EmployeeCode { get; set; }

        /// <summary>
        /// 員工姓名
        /// </summary>
        [Index(1)]
        [NullValues("", "NULL", null)]
        public string EmployeeName { get; set; }

        /// <summary>
        /// 身分證字號
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string EmployeeID { get; set; }

        /// <summary>
        /// 子公司代碼
        /// </summary>
        [Index(3)]
        [NullValues("", "NULL", null)]
        public string SubsidiaryCode { get; set; }

        /// <summary>
        /// 事業群代碼
        /// </summary>
        [Index(4)]
        [NullValues("", "NULL", null)]
        public string BusinessGroup { get; set; }

        /// <summary>
        /// 事業群名稱
        /// </summary>
        [Index(5)]
        [NullValues("", "NULL", null)]
        public string BusinessName { get; set; }

        /// <summary>
        /// 區處/分行代碼
        /// </summary>
        [Index(6)]
        [NullValues("", "NULL", null)]
        public string OfficeOrBranchCode { get; set; }

        /// <summary>
        /// 區處/分行名稱
        /// </summary>
        [Index(7)]
        [NullValues("", "NULL", null)]
        public string OfficeOrBranchName { get; set; }

        /// <summary>
        /// 部門代碼
        /// </summary>
        [Index(8)]
        [NullValues("", "NULL", null)]
        public string DepartmentCode { get; set; }

        /// <summary>
        /// 部門名稱
        /// </summary>
        [Index(9)]
        [NullValues("", "NULL", null)]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 職務代碼
        /// </summary>
        [Index(10)]
        [NullValues("", "NULL", null)]
        public string PositionCode { get; set; }

        /// <summary>
        /// 職務
        /// </summary>
        [Index(11)]
        [NullValues("", "NULL", null)]
        public string PositionName { get; set; }

        /// <summary>
        /// 職稱代碼
        /// </summary>
        [Index(12)]
        [NullValues("", "NULL", null)]
        public string JobTitleCode { get; set; }

        /// <summary>
        /// 職稱
        /// </summary>
        [Index(13)]
        [NullValues("", "NULL", null)]
        public string JobTitleName { get; set; }

        /// <summary>
        /// 職位代碼
        /// </summary>
        [Index(14)]
        [NullValues("", "NULL", null)]
        public string JobPositionCode { get; set; }

        /// <summary>
        /// 職位
        /// </summary>
        [Index(15)]
        [NullValues("", "NULL", null)]
        public string JobPositionName { get; set; }

        /// <summary>
        /// 直屬主管
        /// </summary>
        [Index(16)]
        [NullValues("", "NULL", null)]
        public string Supervisor { get; set; }

        /// <summary>
        /// 赴任日期
        /// </summary>
        [Index(17)]
        [NullValues("", "NULL", null)]
        public string AppointmentDate { get; set; }

        /// <summary>
        /// 出勤分行
        /// </summary>
        [Index(18)]
        [NullValues("", "NULL", null)]
        public string Attendancebranch { get; set; }

        /// <summary>
        /// 主管階層代碼
        /// </summary>
        [Index(19)]
        [NullValues("", "NULL", null)]
        public string SupervisorCode { get; set; }

        /// <summary>
        /// 主管階層
        /// </summary>
        [Index(20)]
        [NullValues("", "NULL", null)]
        public string SupervisorName { get; set; }

        /// <summary>
        /// 是否具信託執照
        /// </summary>
        [Index(21)]
        [NullValues("", "NULL", null)]
        public string HaveTrustLicense { get; set; }

        /// <summary>
        /// 保險執照
        /// </summary>
        [Index(22)]
        [NullValues("", "NULL", null)]
        public string HaveInsurancseLicense { get; set; }

        /// <summary>
        /// 是否具投顧業務人員資格
        /// </summary>
        [Index(23)]
        [NullValues("", "NULL", null)]
        public string InvestmentQualifications { get; set; }

        /// <summary>
        /// 衍生性金融商品業務人員資格或結構型商品業務人員資格
        /// </summary>
        [Index(24)]
        [NullValues("", "NULL", null)]
        public string DerivativeFinancialOrStructuredQualifications { get; set; }

        /// <summary>
        /// 個金業務人員類別
        /// </summary>
        [Index(25)]
        [NullValues("", "NULL", null)]
        public string PersonalFinanceBusinessPersonnelCategory { get; set; }

        /// <summary>
        /// 區域中心業務組別
        /// </summary>
        [Index(26)]
        [NullValues("", "NULL", null)]
        public string RegionalCenterBusinessGroup { get; set; }

        /// <summary>
        /// 薪等
        /// </summary>
        [Index(27)]
        [NullValues("", "NULL", null)]
        public string SalaryLevel { get; set; }

        /// <summary>
        /// 薪級
        /// </summary>
        [Index(28)]
        [NullValues("", "NULL", null)]
        public string SalaryScale { get; set; }
    }
}
