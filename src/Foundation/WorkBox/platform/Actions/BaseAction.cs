using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Workflows.Simple;

namespace Foundation.WorkBox.Actions.Workflows
{
    public abstract class BaseAction : IWorkflowAction
    {
        /// <summary>
        /// Action的Item
        /// </summary>
        protected Item InnerItem { get; set; }

        /// <summary>
        /// 被執行Action的Item
        /// </summary>
        protected Item CurrentItem { get; set; }

        public virtual void Process(WorkflowPipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            ProcessorItem processorItem = args.ProcessorItem;
            Item currentItem = args.DataItem;

            if (processorItem == null || currentItem == null)
                return;
            this.InnerItem = processorItem.InnerItem;
            this.CurrentItem = currentItem;
            Run(args);
        }

        /// <summary>
        /// 客製程式實作執行
        /// </summary>
        /// <param name="args"></param>
        protected abstract void Run(WorkflowPipelineArgs args);
    }
}