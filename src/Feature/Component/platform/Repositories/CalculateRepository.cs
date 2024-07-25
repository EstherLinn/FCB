using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Component.Models.Calculate;
using Foundation.Wealth.Manager;
using log4net;
using Newtonsoft.Json;
using Sitecore.Mvc.Presentation;
using System;
using System.Data.SqlClient;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using Templates = Feature.Wealth.Component.Models.Calculate.Template;

namespace Feature.Wealth.Component.Repositories
{
    public class CalculateRepository
    {
        private ILog Log { get; } = Logger.Account;

        /// <summary>
        /// 取得試算Model
        /// </summary>
        /// <returns>試算Model</returns>
        public CalculateModel GetCalculateModel()
        {
            var dataSource = RenderingContext.CurrentOrNull?.ContextItem;

            if (dataSource == null || dataSource.TemplateID != Templates.Calculate.Id)
            {
                return null;
            }

            CalculateModel model = new CalculateModel();

            model.Datasource = dataSource;
            model.MainTitle = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.MainTitle);
            model.MainContent = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.MainContent);
            model.Image = ItemUtils.ImageUrl(dataSource, Templates.Calculate.Fields.Image);
            model.AnticipatedInvestmentTipTitle = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.AnticipatedInvestmentTipTitle);
            model.AnticipatedInvestmentTipContent = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.AnticipatedInvestmentTipContent);
            model.LifeExpectancyTipTitle = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.LifeExpectancyTipTitle);
            model.LifeExpectancyTipContent = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.LifeExpectancyTipContent);
            model.RetirementExpensesTipTitle = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.RetirementExpensesTipTitle);
            model.RetirementExpensesTipContent = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.RetirementExpensesTipContent);
            model.EstimatedPensionTipTitle = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.EstimatedPensionTipTitle);
            model.EstimatedPensionTipContent = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.EstimatedPensionTipContent);
            model.SuccessImage = ItemUtils.ImageUrl(dataSource, Templates.Calculate.Fields.SuccessImage);
            model.SuccessContent = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.SuccessContent);
            model.UnsuccessfulImage = ItemUtils.ImageUrl(dataSource, Templates.Calculate.Fields.UnsuccessfulImage);
            model.UnsuccessfulContent = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.UnsuccessfulContent);
            model.ExpectedReturnRemarks = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.ExpectedReturnRemarks);

            var defaultRiskAttributes = ItemUtils.GetReferenceFieldItem(dataSource, Templates.Calculate.Fields.DefaultRiskAttributes);
            model.DefaultRiskAttributes = defaultRiskAttributes != null
            ? ItemUtils.GetFieldValue(defaultRiskAttributes, ComponentTemplates.DropdownOption.Fields.OptionValue)
            : "2";

            model.StockAllocation = ItemUtils.GetInteger(dataSource, Templates.Calculate.Fields.StockAllocation);
            model.BondAllocation = ItemUtils.GetInteger(dataSource, Templates.Calculate.Fields.BondAllocation);
            model.CurrencyAllocation = ItemUtils.GetInteger(dataSource, Templates.Calculate.Fields.CurrencyAllocation);
            model.StockAllocationFieldName = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.StockAllocationFieldName);
            model.BondAllocationFieldName = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.BondAllocationFieldName);
            model.CurrencyAllocationFieldName = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.CurrencyAllocationFieldName);
            model.StockAllocationText = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.StockAllocationText);
            model.BondAllocationText = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.BondAllocationText);
            model.CurrencyAllocationText = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.CurrencyAllocationText);
            model.Notice = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.Notice);
            model.RemoteConsultationTitle = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.RemoteConsultationTitle);
            model.RemoteConsultationContent = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.RemoteConsultationContent);
            model.RemoteConsultationButtonText = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.RemoteConsultationButtonText);
            model.RemoteConsultationButtonLink = ItemUtils.GeneralLink(dataSource, Templates.Calculate.Fields.RemoteConsultationButtonLink).Url;
            model.RemoteConsultationImage = ItemUtils.ImageUrl(dataSource, Templates.Calculate.Fields.RemoteConsultationImage);

            return model;
        }

        /// <summary>
        /// 更新試算資料
        /// </summary>
        /// <param name="data">試算資料</param>
        /// <returns>更新是否成功</returns>
        public bool UpdateCalculationResults(CalculationResultData data)
        {
            var success = false;

            if (data == null)
            {
                return success;
            }

            try
            {
                var jsonStr = JsonConvert.SerializeObject(new
                {
                    Name = data.Name,
                    DateTime = data.DateTime,
                    ResultHasGap = data.ResultHasGap,
                    Description = data.Description,
                    EarningsChart = data.EarningsChart,
                    Readingbar = data.Readingbar,
                    ChartsRevenues = data.ChartsRevenues,
                    ChartsRewards = data.ChartsRewards,
                    ChartsRewardsCategories = data.ChartsRewardsCategories
                });

                if (!Enum.TryParse(data.Type, out CalculateTypeEnum calculateType))
                {
                    return success;
                }

                string columnName;
                switch (calculateType)
                {
                    case CalculateTypeEnum.EducationFundList:
                        columnName = "EducationFundList";
                        break;
                    case CalculateTypeEnum.SavingList:
                        columnName = "SavingList";
                        break;
                    case CalculateTypeEnum.BuyHouseList:
                        columnName = "BuyHouseList";
                        break;
                    case CalculateTypeEnum.RetirementPreparationList:
                        columnName = "RetirementPreparationList";
                        break;
                    default:
                        return success;
                }

                var strSql = @$"
                    MERGE MemberCalculationList AS target
                    USING (SELECT @id AS PlatFormId) AS source
                    ON (target.PlatFormId = source.PlatFormId)
                    WHEN MATCHED THEN 
                        UPDATE SET {columnName} = @jsonStr 
                    WHEN NOT MATCHED BY TARGET THEN 
                        INSERT (PlatFormId, {columnName}) VALUES (@id, @jsonStr);";

                var para = new
                {
                    id = FcbMemberHelper.GetMemberPlatFormId(),
                    jsonStr
                };

                var affectedRows = DbManager.Custom.ExecuteNonQuery(strSql, para, commandType: System.Data.CommandType.Text);
                success = affectedRows != 0;
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return success;
        }
    }
}