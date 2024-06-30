/* 本功能為刪除Item之編審放流程的一部分
 * 覆寫了原先Sitecore的Item:Delete功能，加上了將Item排入編審放的功能
 * 啟用本功能需修改Workflow Template，以及將IIS下的App_Config\Sitecore\CMS.Core\Sitecore.Commands.config
 *
 * 修改以下這個command:
 * <command name="item:delete" type="Sitecore.Shell.Framework.Commands.Delete,Sitecore.Kernel" />
 * 變更為：
 * <command name="item:delete" type="Foundation.WorkBox.Commands.Workflows.DeleteWorkFlow,Foundation.WorkBox" />
 *
 */

using Feature.WorkBox;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Exceptions;
using Sitecore.Globalization;
using Sitecore.SecurityModel;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;
using Sitecore.Workflows;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Foundation.WorkBox.Commands.Workflows
{
    /// <summary>
    /// 實作下刪除之編審放，將欲刪除的Item加入編審放
    /// </summary>
    public class DeleteWorkFlow : Delete
    {
        private CommandContext _commandContext;

        /// <summary>
        /// 欲刪除之Item是否屬於System相關Item
        /// (是的話不走編審放)
        /// </summary>
        /// <param name="context">目標Item</param>
        /// <returns>True for System Item, False for Content Item</returns>
        private bool IsSystemItem(Item context)
        {
            return !(context.Paths.IsMediaItem || context.Paths.IsContentItem);
        }

        /// <summary>
        /// 只有在Deletion Stage被打勾 且 已經簽核至Publish State時才允許刪除
        /// </summary>
        /// <param name="context">欲刪除之Item</param>
        /// <returns>True for Delete, False for Auction</returns>
        private bool IsDeletionStage(Item context)
        {
            if (context.Fields["Deletion"] != null)
            {
                Sitecore.Data.Fields.CheckboxField chk = (Sitecore.Data.Fields.CheckboxField)context.Fields["Deletion"];
                var workflow = context.State?.GetWorkflow();

                //如果沒有指定WORKFLOW => 可以刪除
                if (chk.Checked && workflow == null)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 是否已進入 Deletion Stage
        /// </summary>
        /// <param name="context">欲刪除之Item</param>
        /// <returns>True for Delete, False for Auction</returns>
        private bool IsDeleted(Item context)
        {
            if (context.Fields["Deletion"] != null)
            {
                Sitecore.Data.Fields.CheckboxField chk = (Sitecore.Data.Fields.CheckboxField)context.Fields["Deletion"];
                if (chk.Checked)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 取得該WorkFlow的Submit ID
        /// </summary>
        /// <param name="workflowId">WorkFlowId</param>
        /// <returns>SubmitId</returns>
        public string GetSubmitIdByWorkflowId(string workflowId)
        {
            if (workflowId == WorkBoxTemplates.DefaultWorkflow.ID)
            {
                return WorkBoxTemplates.DraftAction.SubmitCommand;
            }
            else
            {
                throw new Exception("DeleteWorkflow error on GetSubmitIdByWorkflowId:" + workflowId);
            }
        }

        /// <summary>
        /// 取得目前Item的Default workflow Initial Step Id
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetWorkflowInitialStep(string workflowID)
        {
            string fieldName = "Initial state";
            string workflowStateID = string.Empty;
            if (!string.IsNullOrEmpty(workflowID))
            {
                Database db = Database.GetDatabase("master");
                if (db == null)
                    return workflowStateID;

                Item defaultWorkflow = db.GetItem(workflowID);
                if (defaultWorkflow != null && defaultWorkflow.Fields[fieldName] != null)
                    workflowStateID = defaultWorkflow[fieldName];
            }
            return workflowStateID;
        }

        /// <summary>
        /// 變更目標Item所處之工作流程State
        /// </summary>
        /// <param name="item">目標Item</param>
        /// <param name="workflowStateId">指定之工作流程ID</param>
        /// <returns>WorkflowResult</returns>
        public WorkflowResult ChangeStateAndExecuteActions(Item item, ID workflowStateId)
        {
            using (new SecurityDisabler())
            {
                using (new EditContext(item))
                {
                    item.Fields["Previous State"].Value = item.Fields[FieldIDs.WorkflowState].Value;
                    string workflowId = item.State?.GetWorkflow()?.WorkflowID;
                    item[FieldIDs.Workflow] = workflowId;
                    item[FieldIDs.DefaultWorkflow] = workflowId;
                    item[FieldIDs.WorkflowState] = workflowStateId.ToString();
                    item.Fields["Deletion"].Value = "1";
                    item["__Submitted"] = DateUtil.ToIsoDate(DateTime.Now);
                    item["__Submitted by"] = Context.User.Name;
                }
            }

            return new WorkflowResult(true, "OK", workflowStateId);
        }

        /// <summary>
        /// 複寫原有Command
        /// </summary>
        /// <param name="context"></param>
        public override void Execute(CommandContext context)
        {
            if (Context.User.IsAdministrator)
            {
                base.Execute(context);
                return;
            }

            if (context?.Items?.Length == 0)
            {
                SheerResponse.Alert("The selected item could not be found.\n\nIt may have been deleted by another user.\n\nSelect another item.", new string[0]);
                return;
            }
            _commandContext = context;
            var contextItem = context.Items[0];
            var workflow = contextItem.State?.GetWorkflow()?.WorkflowID ?? string.Empty;
            if (IsSystemItem(contextItem) || IsDeletionStage(contextItem) || string.IsNullOrEmpty(workflow))
            {
                //直接刪除
                //SheerResponse.Eval("this.Content.loadNextSearchedItem('{0}')", new object[] { context.Items[0].ID });
                //Sitecore.Shell.Framework.Items.Delete(context.Items);
                base.Execute(context);
            }
            else
            {
                if (!IsDeleted(contextItem))
                {
                    //var states = workflow.GetStates();
                    NameValueCollection parameters = new NameValueCollection();
                    parameters["id"] = contextItem.ID.ToString();
                    parameters["name"] = contextItem.Name;
                    parameters["database"] = contextItem.Database.Name;
                    parameters["language"] = contextItem.Language.ToString();
                    parameters["workflowid"] = workflow;
                    parameters["commandid"] = GetSubmitIdByWorkflowId(workflow);
                    parameters["workflow_draft"] = GetWorkflowInitialStep(workflow);
                    // Execute Run method
                    Context.ClientPage.Start(this, "Run", parameters);
                }
                else
                {
                    StartDialog(contextItem);
                }
            }
        }

        /// <summary>
        /// 設置彈窗訊息
        /// </summary>
        /// <param name="item"></param>
        private void StartDialog(Item item)
        {
            ClientPipelineArgs cpa = new ClientPipelineArgs();
            cpa.Parameters.Add("id", item.ID.ToString());
            cpa.Parameters.Add("lan", item.Language.Name);
            Context.ClientPage.Start(this, "DialogProcessor", cpa);
        }

        /// <summary>
        /// 等待確認刷新
        /// </summary>
        /// <param name="args"></param>
        protected void DialogProcessor(ClientPipelineArgs args)
        {
            var id = args.Parameters["id"];
            var lang = args.Parameters["lan"];

            if (!args.IsPostBack)
            {
                SheerResponse.Alert("本節點已進入刪除的編審流程", true);
                args.WaitForPostBack(true);
            }
            else
            {
                string load = $"item:load(id={id},language={lang})";
                Context.ClientPage.ClientResponse.Timer(load, 2);
            }
        }

        /// <summary>
        /// 取得目前Context之CommandState
        /// </summary>
        /// <param name="context">選定之Item context</param>
        /// <returns>CommandState</returns>
        public override CommandState QueryState(CommandContext context)
        {
            Error.AssertObject(context, "context");
            if (Context.IsAdministrator)
                return base.QueryState(context);
            if (context.Items.Length == 0)
            {
                return CommandState.Disabled;
            }
            Item[] items = context.Items;
            int i = 0;

            while (i < items.Length)
            {
                Item item = items[i];
                CommandState result;
                if (item.Versions.Count == 0)
                    return CommandState.Disabled;

                if (!item.Access.CanDelete())
                {
                    result = CommandState.Disabled;
                }
                else if (item.Appearance.ReadOnly)
                {
                    result = CommandState.Disabled;
                }
                else
                {
                    if (!IsLockedByOther(item))
                    {
                        i++;
                        continue;
                    }
                    result = CommandState.Disabled;
                }
                return result;
            }
            return base.QueryState(context);
        }

        /// <summary>
        /// Client Shell視窗
        /// </summary>
        /// <param name="args"></param>
        protected void Run(ClientPipelineArgs args)
        {
            try
            {
                if (args.IsPostBack)
                {
                    #region PostBack

                    Assert.ArgumentNotNull(args, "args");
                    Item current = _commandContext.Items[0];
                    string text = args.Parameters["commandid"];
                    string workflowId = args.Parameters["workflowid"];
                    string workflow_draft = args.Parameters["workflow_draft"];

                    ID.TryParse(text, out ID @null);

                    ItemUri itemUri = new ItemUri(args.Parameters["id"], Language.Parse(args.Parameters["language"]), current.Version, Client.ContentDatabase);
                    List<ItemUri> itemUriList = new List<ItemUri>() { itemUri };

                    // Confirm 視窗
                    if (args.CustomData.Count == 0)
                    {
                        if (args.Result == "yes")
                        {
                            #region Prompt=Yes

                            if (_commandContext?.Items.Length == 1)
                            {
                                if (Sitecore.Context.User.IsAdministrator || IsSystemItem(current) || IsDeletionStage(current))
                                {
                                    SheerResponse.Eval("if(this.Content && this.Content.loadNextSearchedItem){{this.Content.loadNextSearchedItem('{0}');}}", new object[]
                                    {
                                    _commandContext.Items[0].ID
                                    });

                                    Sitecore.Shell.Framework.Items.Delete(_commandContext.Items);
                                }
                                else
                                {
                                    bool flag = (args.Parameters["ui"] != null && args.Parameters["ui"] == "1") || (args.Parameters["suppresscomment"] != null && args.Parameters["suppresscomment"] == "1");
                                    //呼叫 Comment 視窗
                                    if (@null != ID.Null && !flag)
                                    {
                                        WorkflowUIHelper.DisplayCommentDialog(itemUriList, @null);
                                        args.CustomData.Add("window", "comment");
                                        args.WaitForPostBack();
                                        return;
                                    }
                                }
                            }

                            #endregion Prompt=Yes
                        }

                        #endregion PostBack
                    }
                    //// Comment 視窗
                    else if (args.Result != null)
                    {
                        if (args.Result.Length > 2000)
                        {
                            Context.ClientPage.ClientResponse.ShowError(new Exception(string.Format("The comment is too long.\n\nYou have entered {0} characters.\nA comment cannot contain more than 2000 characters.", args.Result.Length)));
                            WorkflowUIHelper.DisplayCommentDialog(itemUriList, @null);
                            args.WaitForPostBack();
                            return;
                        }
                        //// args.Result == "undefined" 為 cancel 舉動
                        if (args.Result == "undefined")
                        {
                            return;
                        }
                        else if (args.Result != "null" && args.Result != "cancel")
                        {
                            //確認刪除時將 WorkFlowStatus 轉成編輯狀態，否則無法正確取得 Comment 視窗
                            var workflow = current.State?.GetWorkflow();
                            if (workflow == null)
                                return;

                            var versionItem = current.Versions.AddVersion();

                            workflow.Start(versionItem);

                            string result = args.Result;
                            Sitecore.Collections.StringDictionary commentFields;
                            if (!string.IsNullOrEmpty(result))
                            {
                                commentFields = WorkflowUIHelper.ExtractFieldsFromFieldEditor(result);
                            }
                            else
                            {
                                commentFields = new Sitecore.Collections.StringDictionary();
                            }
                            try
                            {
                                if (!string.IsNullOrWhiteSpace(workflowId))
                                {
                                    //紀錄 comment
                                    //Processor completionCallback = new Processor("Workflow complete state item count", this, "WorkflowCompleteStateItemCount");
                                    //WorkflowUIHelper.ExecuteCommand(itemUri, workflowId, text, commentFields, completionCallback);

                                    workflow.Execute(text, versionItem, commentFields, false);

                                    //處理 Item
                                    if (versionItem.Fields["Deletion"].Value != "1")
                                    {
                                        using (new EditContext(versionItem, SecurityCheck.Disable))
                                        {
                                            versionItem.Fields["Previous State"].Value = current.Fields[FieldIDs.WorkflowState].Value;
                                            //string workflowId = item.GetWorkflow()?.WorkflowID;
                                            //item[FieldIDs.Workflow] = workflowId;
                                            //item[FieldIDs.DefaultWorkflow] = workflowId;
                                            //item[FieldIDs.WorkflowState] = workflowStateId.ToString();
                                            versionItem.Fields["Deletion"].Value = "1";
                                            versionItem["__Submitted"] = DateUtil.ToIsoDate(DateTime.Now);
                                            versionItem["__Submitted by"] = Context.User.Name;
                                        }
                                    }
                                    StartDialog(versionItem);
                                }
                            }
                            catch (WorkflowStateMissingException)
                            {
                                SheerResponse.Alert("One or more items could not be processed because their workflow state does not specify the next step.", new string[0]);
                            }
                        }
                    }
                }
                else
                {
                    #region First Confirm

                    if (_commandContext.Items.Length == 1)
                    {
                        Item current = _commandContext.Items[0];
                        string confirmText = string.Format(@"Are you sure you want to delete ""{0}""?", current.DisplayName);
                        if (Sitecore.Context.Language.Name.ToLower() != "en")
                        {
                            confirmText = string.Format(@"您是否確定要刪除 ""{0}""?", current.DisplayName);
                        }
                        SheerResponse.Confirm(confirmText);
                        args.WaitForPostBack();
                    }

                    #endregion First Confirm
                }
            }
            catch (Exception ex)
            {
                Log.Error($"{GetType().Name} Error", ex, this);
            }
        }
    }
}