using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Component.Models.MemberCalculationList;
using Foundation.Wealth.Manager;
using Newtonsoft.Json;

namespace Feature.Wealth.Component.Repositories
{
    public class MemberCalculationListRepository
    {
        public MemberCalculationListModel GetMemberCalculationList()
        {
            MemberCalculationListModel lists = new MemberCalculationListModel();

            string strSql = @"
            SELECT [PlatFormId],[SavingList],[EducationFundList],[BuyHouseList],[RetirementPreparationList] 
            FROM [dbo].[MemberCalculationList] 
            WHERE [PlatFormId] COLLATE Latin1_General_CS_AS = @platFormId";

            var platFormId = FcbMemberHelper.GetMemberPlatFormId();

            var para = new { platFormId };

            var result = DbManager.Custom.ExecuteIList<dynamic>(strSql, para, commandType: System.Data.CommandType.Text);

            if (result != null && result.Count > 0)
            {
                var record = result[0];

                lists.PlatFormId = record.PlatFormId;

                if (!string.IsNullOrEmpty(record.SavingList))
                {
                    lists.SavingList = JsonConvert.DeserializeObject<CalculationListData>(record.SavingList);
                }

                if (!string.IsNullOrEmpty(record.EducationFundList))
                {
                    lists.EducationFundList = JsonConvert.DeserializeObject<CalculationListData>(record.EducationFundList);
                }

                if (!string.IsNullOrEmpty(record.BuyHouseList))
                {
                    lists.BuyHouseList = JsonConvert.DeserializeObject<CalculationListData>(record.BuyHouseList);
                }

                if (!string.IsNullOrEmpty(record.RetirementPreparationList))
                {
                    lists.RetirementPreparationList = JsonConvert.DeserializeObject<CalculationListData>(record.RetirementPreparationList);
                }
            }

            return lists;
        }
    }
}