using System;

namespace Feature.Wealth.Toolkit.Models.Report
{
    public class ScheduleViewModel
    {
        public Guid ScheduleID { get; set; }
        public string EmployeeName { get; set; }
        public string CustomerName { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string BranchName { get; set; }
        public string StatusName { get; set; }
    }
}
