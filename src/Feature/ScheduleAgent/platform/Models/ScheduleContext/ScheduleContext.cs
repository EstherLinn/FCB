using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.ScheduleAgent.Models.ScheduleContext
{
    /// <summary>
    /// Schedule Context
    /// </summary>
    public class ScheduleContext
    {
        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 排程名稱
        /// </summary>
        public string ScheduleName { get; set; }

        /// <summary>
        /// 執行緒ID
        /// </summary>
        public int ThreadId { get; set; }

        /// <summary>
        /// 任務執行ID
        /// </summary>
        public string TaskExecutionId { get; set; }
    }
}
