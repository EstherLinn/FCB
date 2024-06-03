using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    /// <summary>
    /// 基金基本資料，檔案名稱：FUND_BSC.txt
    /// </summary>
    [Delimiter(";")]
    [HasHeaderRecord(false)]
    public class FundBsc
    {
        /// <summary>
        /// 資料日期
        /// </summary>
        public string DataDate { get; set; }

        /// <summary>
        /// 國內/境外基金註記
        /// </summary>
        public string DomesticForeignFundIndicator { get; set; }

        /// <summary>
        /// ISIN Code
        /// </summary>
        public string ISINCode { get; set; }

        /// <summary>
        /// 銀行商品代碼
        /// </summary>
        public string BankProductCode { get; set; }

        /// <summary>
        /// 基金名稱
        /// </summary>
        public string FundName { get; set; }

        /// <summary>
        /// 基金英文名稱
        /// </summary>
        public string FundEnglishName { get; set; }

        /// <summary>
        /// 基金公司ID
        /// </summary>
        public string FundCompanyID { get; set; }

        /// <summary>
        /// 基金公司名稱
        /// </summary>
        public string FundCompanyName { get; set; }

        /// <summary>
        /// 海外基金發行公司ID
        /// </summary>
        public string OverseasFundIssuerID { get; set; }

        /// <summary>
        /// 海外基金發行公司名稱
        /// </summary>
        public string OverseasFundIssuerName { get; set; }

        /// <summary>
        /// 基金類型ID
        /// </summary>
        public string FundTypeID { get; set; }

        /// <summary>
        /// 基金類型名稱
        /// </summary>
        public string FundTypeName { get; set; }

        /// <summary>
        /// 投資區域ID
        /// </summary>
        public string InvestmentRegionID { get; set; }

        /// <summary>
        /// 投資區域名稱
        /// </summary>
        public string InvestmentRegionName { get; set; }

        /// <summary>
        /// 投資標的ID
        /// </summary>
        public string InvestmentTargetID { get; set; }

        /// <summary>
        /// 投資標的名稱
        /// </summary>
        public string InvestmentTargetName { get; set; }

        /// <summary>
        /// 基金經理人ID
        /// </summary>
        public string FundManagerID { get; set; }

        /// <summary>
        /// 基金經理人
        /// </summary>
        public string FundManager { get; set; }

        /// <summary>
        /// 規模幣別
        /// </summary>
        public string ScaleCurrency { get; set; }

        /// <summary>
        /// 基金規模日期
        /// </summary>
        public string FundScaleDate { get; set; }

        /// <summary>
        /// 基金規模(百萬)
        /// </summary>
        public string FundScaleMillion { get; set; }

        /// <summary>
        /// 成立日期
        /// </summary>
        public string EstablishmentDate { get; set; }

        /// <summary>
        /// 成立規模(百萬)
        /// </summary>
        public string EstablishmentScaleMillion { get; set; }

        /// <summary>
        /// 計價幣別
        /// </summary>
        public string ValuationCurrency { get; set; }

        /// <summary>
        /// 銷售費(%)
        /// </summary>
        public string SalesFee { get; set; }

        /// <summary>
        /// 經理費(%)
        /// </summary>
        public string ManagementFee { get; set; }

        /// <summary>
        /// 保管費(%)
        /// </summary>
        public string CustodyFee { get; set; }

        /// <summary>
        /// 註冊地
        /// </summary>
        public string RegistrationLocation { get; set; }

        /// <summary>
        /// 傘型基金
        /// </summary>
        public string UmbrellaFund { get; set; }

        /// <summary>
        /// 保管銀行/機構
        /// </summary>
        public string CustodianInstitution { get; set; }

        /// <summary>
        /// 基金統編
        /// </summary>
        public string FundUnifiedIdentificationNumber { get; set; }

        /// <summary>
        /// 風險收益等級(RR)
        /// </summary>
        public string RiskRewardLevel { get; set; }

        /// <summary>
        /// 配息頻率ID
        /// </summary>
        public string DividendFrequencyID { get; set; }

        /// <summary>
        /// 配息頻率名稱
        /// </summary>
        public string DividendFrequencyName { get; set; }

        /// <summary>
        /// 下架註記
        /// </summary>
        public string DelistingIndicator { get; set; }

        /// <summary>
        /// 投資策略/投資標的
        /// </summary>
        public string InvestmentStrategyInvestmentTarget { get; set; }
    }
}