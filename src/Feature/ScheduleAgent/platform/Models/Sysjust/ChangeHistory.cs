using System;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    public class ChangeHistory
    {
        public string FileName { get; set; }
        public DateTime ModificationDate { get; set; }
        public string ModificationType { get; set; }
        public string DataTable { get; set; }
        public int ModificationLine { get; set; }
    }
}