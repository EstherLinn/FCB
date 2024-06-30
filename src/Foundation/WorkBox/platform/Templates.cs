using Sitecore.Data;

namespace Feature.WorkBox
{
    public class WorkBoxTemplates
    {
        public struct DefaultWorkflow
        {
            public const string ID = "{7F2E9CD4-FBE5-4A71-A379-E981C54E310D}";
        }

        public struct WorkflowsState
        {
            public const string Draft = "{C3692B28-F467-43F5-93B5-21AFB23D2BBA}";
            public const string Approval = "{67DC6D04-021E-41E5-82F1-833416E51E17}";
        }

        public struct DraftAction
        {
            public const string SubmitCommand = "{8E2A5E0A-A068-45E1-BC0F-2856E459DEFD}";
        }
    }
}