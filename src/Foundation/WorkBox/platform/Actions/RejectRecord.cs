using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using Sitecore.Workflows.Simple;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Foundation.WorkBox.Actions.Workflows
{
    /// <summary>
    /// Workflow Command Action - 退回時更新及記錄狀態
    /// </summary>
    public class RejectRecord : BaseAction
    {
        protected override void Run(WorkflowPipelineArgs args)
        {
            if (this.CurrentItem.IsChecked("Deletion"))
            {
                //配合DeleteWorkFlow動作為進版刪除，移除版本
                using (new SecurityDisabler())
                {
                    this.CurrentItem.RecycleVersion();
                }
                return;
            }
            using (new EditContext(CurrentItem, SecurityCheck.Disable))
            {
                this.CurrentItem["Rejection"] = "1";
                this.CurrentItem["Deletion"] = "0";

                var submmitedUser = this.CurrentItem["__Submitted by"];
                //將Lock回歸送審人
                if (!string.IsNullOrEmpty(submmitedUser) && Sitecore.Security.Accounts.User.Exists(submmitedUser))
                {
                    var user = Sitecore.Security.Accounts.User.FromName(submmitedUser, false);
                    if (this.CurrentItem.Locking.CanLock())
                    {
                        using (new Sitecore.Security.Accounts.UserSwitcher(user))
                        {
                            this.CurrentItem.Locking.Lock();
                        }
                    }
                }
            }
        }
    }
}