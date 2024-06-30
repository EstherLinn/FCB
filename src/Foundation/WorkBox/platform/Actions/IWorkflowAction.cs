using Sitecore.Workflows.Simple;

namespace Foundation.WorkBox.Actions.Workflows
{
    /// <summary>
    /// 執行Workflow Action
    /// 於Sitecore後台Workflow Commands下依序讀取Children Type欄位(Type & Assembly)建立實體invoke Process方法
    /// </summary>
    public interface IWorkflowAction
    {
        /// <summary>
        /// Sitecore後台程式進入點
        /// </summary>
        /// <param name="args"></param>
        public void Process(WorkflowPipelineArgs args);
    }
}