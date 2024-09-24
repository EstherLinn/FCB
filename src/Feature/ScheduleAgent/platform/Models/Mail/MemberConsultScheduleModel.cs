using System;

namespace Feature.Wealth.ScheduleAgent.Models.Mail
{
    public class MemberConsultScheduleModel
    {
        public Guid ScheduleID { get; set; }
        /// <summary>
        /// 預約日期
        /// </summary>
        public DateTime ScheduleDate { get; set; }
        /// <summary>
        /// 員工姓名
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 預約開始時間 hh:mm
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 預約結束時間 hh:mm
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 分行名稱
        /// </summary>
        public string BranchName { get; set; }   
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

    }
}
