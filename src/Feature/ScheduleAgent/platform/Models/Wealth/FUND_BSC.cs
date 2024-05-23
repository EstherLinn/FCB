using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    public class FundBsc
    {
        public string DataDate { get; set; }
        public string DomesticForeignFundIndicator { get; set; }
        public string ISINCode { get; set; }
        public string BankProductCode { get; set; }
        public string FundName { get; set; }
        public string FundEnglishName { get; set; }
        public string FundCompanyID { get; set; }
        public string FundCompanyName { get; set; }
        public string OverseasFundIssuerID { get; set; }
        public string OverseasFundIssuerName { get; set; }
        public string FundTypeID { get; set; }
        public string FundTypeName { get; set; }
        public string InvestmentRegionID { get; set; }
        public string InvestmentRegionName { get; set; }
        public string InvestmentTargetID { get; set; }
        public string InvestmentTargetName { get; set; }
        public string FundManagerID { get; set; }
        public string FundManager { get; set; }
        public string ScaleCurrency { get; set; }
        public string FundScaleDate { get; set; }
        public string FundScaleMillion { get; set; }
        public string EstablishmentDate { get; set; }
        public string EstablishmentScaleMillion { get; set; }
        public string ValuationCurrency { get; set; }
        public string SalesFee { get; set; }
        public string ManagementFee { get; set; }
        public string CustodyFee { get; set; }
        public string RegistrationLocation { get; set; }
        public string UmbrellaFund { get; set; }
        public string CustodianInstitution { get; set; }
        public string FundUnifiedIdentificationNumber { get; set; }
        public string RiskRewardLevel { get; set; }
        public string DividendFrequencyID { get; set; }
        public string DividendFrequencyName { get; set; }
        public string DelistingIndicator { get; set; }
        public string InvestmentStrategyInvestmentTarget { get; set; }
    }
}