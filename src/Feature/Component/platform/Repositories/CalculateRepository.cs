using Feature.Wealth.Component.Models.Calculate;
using Sitecore.Mvc.Presentation;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Component.Repositories
{
    public class CalculateRepository
    {
        public CalculateModel GetCalculateModel()
        {
            var dataSource = RenderingContext.CurrentOrNull?.ContextItem;

            if (dataSource == null || dataSource.TemplateID != Template.Calculate.Id)
            {
                return null;
            }

            CalculateModel model = new CalculateModel();

            model.Datasource = dataSource;
            model.MainTitle = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.MainTitle);
            model.MainContent = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.MainContent);
            model.Image = ItemUtils.ImageUrl(dataSource, Template.Calculate.Fields.Image);
            model.AnticipatedInvestmentTipTitle = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.AnticipatedInvestmentTipTitle);
            model.AnticipatedInvestmentTipContent = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.AnticipatedInvestmentTipContent);
            model.LifeExpectancyTipTitle = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.LifeExpectancyTipTitle);
            model.LifeExpectancyTipContent = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.LifeExpectancyTipContent);
            model.RetirementExpensesTipTitle = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.RetirementExpensesTipTitle);
            model.RetirementExpensesTipContent = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.RetirementExpensesTipContent);
            model.EstimatedPensionTipTitle = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.EstimatedPensionTipTitle);
            model.EstimatedPensionTipContent = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.EstimatedPensionTipContent);
            model.SuccessImage = ItemUtils.ImageUrl(dataSource, Template.Calculate.Fields.SuccessImage);
            model.SuccessContent = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.SuccessContent);
            model.UnsuccessfulImage = ItemUtils.ImageUrl(dataSource, Template.Calculate.Fields.UnsuccessfulImage);
            model.UnsuccessfulContent = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.UnsuccessfulContent);
            model.ExpectedReturnRemarks = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.ExpectedReturnRemarks);

            var defaultRiskAttributes = ItemUtils.GetReferenceFieldItem(dataSource, Template.Calculate.Fields.DefaultRiskAttributes);
            model.DefaultRiskAttributes = defaultRiskAttributes != null
            ? ItemUtils.GetFieldValue(defaultRiskAttributes, ComponentTemplates.DropdownOption.Fields.OptionValue)
            : "2";

            model.StockAllocation = ItemUtils.GetInteger(dataSource, Template.Calculate.Fields.StockAllocation);
            model.BondAllocation = ItemUtils.GetInteger(dataSource, Template.Calculate.Fields.BondAllocation);
            model.CurrencyAllocation = ItemUtils.GetInteger(dataSource, Template.Calculate.Fields.CurrencyAllocation);
            model.StockAllocationFieldName = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.StockAllocationFieldName);
            model.BondAllocationFieldName = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.BondAllocationFieldName);
            model.CurrencyAllocationFieldName = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.CurrencyAllocationFieldName);
            model.StockAllocationText = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.StockAllocationText);
            model.BondAllocationText = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.BondAllocationText);
            model.CurrencyAllocationText = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.CurrencyAllocationText);
            model.Notice = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.Notice);
            model.RemoteConsultationTitle = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.RemoteConsultationTitle);
            model.RemoteConsultationContent = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.RemoteConsultationContent);
            model.RemoteConsultationButtonText = ItemUtils.GetFieldValue(dataSource, Template.Calculate.Fields.RemoteConsultationButtonText);
            model.RemoteConsultationButtonLink = ItemUtils.GeneralLink(dataSource, Template.Calculate.Fields.RemoteConsultationButtonLink).Url;
            model.RemoteConsultationImage = ItemUtils.ImageUrl(dataSource, Template.Calculate.Fields.RemoteConsultationImage);

            return model;
        }
    }
}
