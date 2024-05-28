using Sitecore.Data;

namespace Feature.Wealth.Component.Models.HeaderWidget
{
    public class HeaderWidgetModel
    {
        public struct Template
        {
            public struct Login
            {
                public static readonly ID Root = new ID("{8B08C8DA-1155-4BF1-80F2-13E812B21D9B}");

                public struct Fields
                {
                    public static readonly ID FocusListLink = new ID("{75C6D10B-B841-45A0-95D6-60022259C2DF}");
                    public static readonly ID ExclusiveRecommendationLink = new ID("{8293F79B-77B1-4034-893D-266F8DBCB052}");
                    public static readonly ID FinancialManagementTrialCalculationLink = new ID("{DBFF9851-7E19-49A7-B70E-E5F692C3B8E8}");
                    public static readonly ID FavoriteNewsLink = new ID("{1384A701-2D94-4B9D-A492-57F01CE02E0E}");
                    public static readonly ID RemoteFinancialConsultingLink = new ID("{E87A9573-716B-47E6-9338-E15C29BB4B51}");
                    public static readonly ID ReserveConsultingLink = new ID("{98A58219-A5FE-480C-9FB4-3E8E79E806AB}");
                }
            }
        }
    }
}
