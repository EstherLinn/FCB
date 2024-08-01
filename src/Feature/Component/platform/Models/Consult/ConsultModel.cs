using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Web;

namespace Feature.Wealth.Component.Models.Consult
{
    public class ConsultModel
    {
        public Item Item { get; set; }
        public string ReturnLink { get; set; }
        public IList<ConsultSchedule> ConsultSchedules { get; set; }
        public HtmlString ConsultSchedulesHtmlString { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public HtmlString HolidayDatesHtmlString { get; set; }
        public HtmlString ReservedsHtmlString { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
    }

    public class ConsultScheduleModel
    {
        public Item Item { get; set; }
        public string Message { get; set; }
        public string ReturnLink { get; set; }
    }

    public class ConsultListModel
    {
        public Item Item { get; set; }
        public string ConsultLink { get; set; }
        public string ConsultScheduleLink { get; set; }
        public List<QandA> QandAList { get; set; } = new List<QandA>();
        public HtmlString Notice { get; set; }
        public IList<ConsultSchedule> ConsultSchedules { get; set; }
        public HtmlString ConsultSchedulesHtmlString { get; set; }
        public IList<ConsultScheduleForCalendar> ConsultScheduleForCalendars { get; set; }
        public HtmlString ConsultScheduleForCalendarsHtmlString { get; set; }
    }

    public class QandA
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }

    public class ConsultSchedule
    {
        public Guid ScheduleID { get; set; }
        /// <summary>
        /// 員工代碼
        /// </summary>
        public string EmployeeID { get; set; }
        /// <summary>
        /// 員工姓名
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// 客戶代碼
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 客戶姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 預約日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }
        /// <summary>
        /// 預約日期字串
        /// </summary>
        public string ScheduleDateString { get; set; }
        /// <summary>
        /// 預約開始時間 hh:mm
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 預約結束時間 hh:mm
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 分機號碼，1003~1007
        /// </summary>
        public string DNIS { get; set; }
        /// <summary>
        /// 員工URL
        /// </summary>
        public string EmployeeURL { get; set; }
        /// <summary>
        /// 客戶URL
        /// </summary>
        public string CustomerURL { get; set; }
        /// <summary>
        /// 員工部門代碼，主管搜尋用
        /// </summary>
        public string DepartmentCode { get; set; }
        /// <summary>
        /// 分行代碼
        /// </summary>
        public string BranchCode { get; set; }
        /// <summary>
        /// 分行名稱
        /// </summary>
        public string BranchName { get; set; }
        /// <summary>
        /// 分行電話
        /// </summary>
        public string BranchPhone { get; set; }
        /// <summary>
        /// 1：客戶自行預約、2：理顧幫客戶預約
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 客戶電話
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 客戶信箱
        /// </summary>
        public string Mail { get; set; }
        /// <summary>
        /// 諮詢主題，逗號分隔
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 其他諮詢主題或其他諮詢內容
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 0：未確認、1：已確認、2：已完成、3：已取消
        /// </summary>
        public string StatusCode { get; set; }
        public bool Comming { get; set; }
        public bool Start { get; set; }
    }

    public class ConsultScheduleForCalendar
    {
        /// <summary>
        /// 同 ScheduleID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 預約中、預約成功、歷史紀錄
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// reserving、success、history
        /// </summary>
        public string Type { get; set; }
        public string UserName { get; set; }
        public string StaffName { get; set; }
        /// <summary>
        /// YYYY-MM-DDTHH:MM
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        ///  YYYY-MM-DDTHH:MM
        /// </summary>
        public string EndTime { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        /// <summary>
        /// 同 Subject
        /// </summary>
        public string Topic { get; set; }
        /// <summary>
        /// 同 Description
        /// </summary>
        public string Other { get; set; }
        /// <summary>
        /// #fdce5eff、#7dd4a4ff、#c3c3c3ff
        /// </summary>
        public string CategoryColor { get; set; }
        /// <summary>
        /// #F4B00F、#56B280、#9C9C9C
        /// </summary>
        public string PopupColor { get; set; }
        /// <summary>
        /// 判斷是否可以開始視訊(已棄用)
        /// </summary>
        public bool Release { get; set; }
    }


    public class Calendar
    {
        public string Date { get; set; }
        public string Week { get; set; }
        public bool IsHoliday { get; set; }
        public string Description { get; set; }
        public string RealDate { get; set; }
    }

    public class Reserved
    {
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

    public class IMVPRequestData
    {
        /// <summary>
        /// Verification Result
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 對應預約 GUID
        /// </summary>
        public string scheduleId { get; set; }
        /// <summary>
        /// 1 or 2
        /// </summary>
        public string action { get; set; }
        public string empId { get; set; }
        /// <summary>
        /// 1 or 2
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// YYYYMMDD
        /// </summary>
        public string date { get; set; }
        /// <summary>
        /// HHMM
        /// </summary>
        public string startTime { get; set; }
        /// <summary>
        /// HHMM
        /// </summary>
        public string endTime { get; set; }
        public string custId { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
    }

    public class OctonRequestData
    {
        /// <summary>
        /// 對應預約 GUID
        /// </summary>
        public string Authorization { get; set; }
        /// <summary>
        /// 1003~1007
        /// </summary>
        public string dnis { get; set; }
        /// <summary>
        /// YYYY-MM-DD
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// HH:MM
        /// </summary>
        public string Start { get; set; }
        /// <summary>
        /// HH:MM
        /// </summary>
        public string End { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string BranchPhone { get; set; }
    }

    public struct Template
    {
        public readonly struct ConsultSchedule
        {
            public static readonly ID Id = new ID("{9DFE3A22-8E91-4766-AEF6-D31437D13650}");

            public readonly struct Fields
            {
            }
        }

        public readonly struct ConsultList
        {
            public static readonly ID Id = new ID("{616E463F-3397-4070-803F-F518BD93CE1B}");

            public readonly struct Fields
            {
                public static readonly ID QandA = new ID("{A28F1CC6-FA71-432E-A2A7-AB4A36F0165B}");
                public static readonly ID Notice = new ID("{46EFE03B-118E-432B-A1D1-8E1D584B4476}");
            }
        }

        public readonly struct ConsultQandA
        {
            public static readonly ID Id = new ID("{EFAC8BDC-5FD3-41A3-B92A-845BC73C1C00}");

            public readonly struct Fields
            {
                public static readonly ID Question = new ID("{FFDBCC64-871E-4E50-8E53-2BC9ECAEE619}");
                public static readonly ID Answer = new ID("{8D87FD0E-10C1-421C-9E4F-1133829299C8}");
            }
        }
    }
}
