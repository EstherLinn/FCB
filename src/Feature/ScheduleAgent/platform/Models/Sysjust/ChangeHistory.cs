using System;
using System.ComponentModel;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    public class ChangeHistory
    {
        public string FileName { get; set; }
        public DateTime ModificationDate { get; set; }
        public string ModificationType { get; set; }
        public string DataTable { get; set; }
        public int ModificationLine { get; set; }
        public double TotalSeconds { get; set; }
        public string Success { get; set; }
        public string ModificationID { get; set; }
    }

    public enum ModificationID
    {
        [Description("最新資料")]
        最新資料 = 100,

        [Description("資料差異更新")]
        資料差異更新 = 101,

        [Description("倉儲")]
        OdbcDone = 102,

        [Description("完成")]
        Done = 200,

        [Description("Error")]
        Error = 404
    }
}