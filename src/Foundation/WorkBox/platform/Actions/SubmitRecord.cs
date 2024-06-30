using Sitecore.Data.Items;
using Sitecore.Workflows.Simple;
using System;

namespace Foundation.WorkBox.Actions.Workflows
{
    /// <summary>
    /// Workflow Command Action - 送審時更新客製欄位記錄送審人
    /// </summary>
    public class SubmitRecord : BaseAction
    {
        protected override void Run(WorkflowPipelineArgs args)
        {
            using (new EditContext(CurrentItem, Sitecore.SecurityModel.SecurityCheck.Disable))
            {
                this.CurrentItem["__Submitted"] = Sitecore.DateUtil.ToIsoDate(DateTime.Now);
                this.CurrentItem["__Submitted by"] = Sitecore.Context.User.Name;
                this.CurrentItem["Rejection"] = "0";
            }
        }
    }
}