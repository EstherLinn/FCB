using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using Sitecore.Workflows.Simple;
using System;

namespace Foundation.WorkBox.Actions.Workflows
{
    public class AutoDelete : BaseAction
    {
        protected override void Run(WorkflowPipelineArgs args)
        {
            Item workFlowItem = this.CurrentItem;
            if (workFlowItem.Fields["Deletion"].Value == "1")
            {
                string deletedBy = workFlowItem["__Submitted by"];
                if (!string.IsNullOrEmpty(deletedBy) && Sitecore.Security.Accounts.User.Exists(deletedBy))
                {
                    //Getting Sitecore User Object with UserName
                    Sitecore.Security.Accounts.User scUser = Sitecore.Security.Accounts.User.FromName(deletedBy, false);
                    //Switching Context User
                    try
                    {
                        //提供Submitted User刪除權限
                        using (new SecurityDisabler())
                        {
                            using (new Sitecore.Security.Accounts.UserSwitcher(scUser))
                            {
                                workFlowItem.Recycle();
                            }
                        }

                    }
                    catch (Exception)
                    {
                        using (new SecurityDisabler())
                        {
                            workFlowItem.Recycle();
                        }
                    }
                }
                else
                {
                    using (new SecurityDisabler())
                    {
                        workFlowItem.Recycle();
                    }
                }
            }
        }
    }
}