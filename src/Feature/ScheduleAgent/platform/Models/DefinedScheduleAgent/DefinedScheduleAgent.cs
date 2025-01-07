using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.QuartzSchedule;

namespace Feature.Wealth.ScheduleAgent.Models.DefinedScheduleAgent
{
    public abstract class DefinedScheduleAgent : SitecronAgentBase
    {
        /// <summary>
        /// 檢查是否為開發環境
        /// </summary>
        protected bool IsDeveloperEnvironment { get; set; } = !Foundation.Wealth.Models.Config.IsEnableCheck;

        /// <summary>
        /// 檢查是否有啟用排程
        /// </summary>
        protected bool IsCheck { get; set; } = Foundation.Wealth.Models.Config.IsAgentEnableCheck;

        protected override async Task Execute()
        {
            // 檢查環境是否為"Developer"，檢查 isCheck 是否為 False
            if (IsDeveloperEnvironment && !IsCheck)
            {
                this.Logger.Error("Agent is disabled. Skipping execution.");
                return; // 如果 isCheck 為 false，則不執行排程
            }

            // 如果 isCheck 為 true ，則繼續執行後續操作
            await ExecuteJobItems();
        }

        protected abstract Task ExecuteJobItems();
    }
}
