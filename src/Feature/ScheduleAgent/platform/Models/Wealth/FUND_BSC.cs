using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    /// <summary>
    /// 基金基本資料，檔案名稱：TFJSBSC.YYMMDD.1000.TXT
    /// </summary>
    [Delimiter(";")]
    [HasHeaderRecord(true)]
    public class FundBsc
    {
        /// <summary>
        /// 資料日期
        /// </summary>
        [Index(0)]
        public string DataDate { get; set; }

        /// <summary>
        /// 國內/境外基金註記
        /// </summary>
        [Index(1)]
        public string DomesticForeignFundIndicator { get; set; }

        /// <summary>
        /// ISIN Code
        /// </summary>
        [Index(2)]
        public string ISINCode { get; set; }

        /// <summary>
        /// 銀行商品代碼
        /// </summary>
        [Index(3)]
        public string BankProductCode { get; set; }

        /// <summary>
        /// 基金名稱
        /// </summary>
        [Index(4)]
        public string FundName { get; set; }

        /// <summary>
        /// 基金英文名稱
        /// </summary>
        [Index(5)]
        public string FundEnglishName { get; set; }

        /// <summary>
        /// 基金公司ID
        /// </summary>
        [Index(6)]
        public string FundCompanyID { get; set; }

        /// <summary>
        /// 基金公司名稱
        /// </summary>
        [Index(7)]
        public string FundCompanyName { get; set; }

        /// <summary>
        /// 海外基金發行公司ID
        /// </summary>
        [Index(8)]
        public string OverseasFundIssuerID { get; set; }

        /// <summary>
        /// 海外基金發行公司名稱
        /// </summary>
        [Index(9)]
        public string OverseasFundIssuerName { get; set; }

        /// <summary>
        /// 基金類型ID
        /// </summary>
        [Index(10)]
        public string FundTypeID { get; set; }

        /// <summary>
        /// 基金類型名稱
        /// </summary>
        [Index(11)]
        public string FundTypeName { get; set; }

        /// <summary>
        /// 投資區域ID
        /// </summary>
        [Index(12)]
        public string InvestmentRegionID { get; set; }

        /// <summary>
        /// 投資區域名稱
        /// </summary>
        [Index(13)]
        public string InvestmentRegionName { get; set; }

        /// <summary>
        /// 投資標的ID
        /// </summary>
        [Index(14)]
        public string InvestmentTargetID { get; set; }

        /// <summary>
        /// 投資標的名稱
        /// </summary>
        [Index(15)]
        public string InvestmentTargetName { get; set; }

        /// <summary>
        /// 基金經理人ID
        /// </summary>
        [Index(16)]
        public string FundManagerID { get; set; }

        /// <summary>
        /// 基金經理人
        /// </summary>
        [Index(17)]
        public string FundManager { get; set; }

        /// <summary>
        /// 規模幣別
        /// </summary>
        [Index(18)]
        public string ScaleCurrency { get; set; }

        /// <summary>
        /// 基金規模日期
        /// </summary>
        [Index(19)]
        public string FundScaleDate { get; set; }

        /// <summary>
        /// 基金規模(百萬)
        /// </summary>
        [Index(20)]
        public decimal? FundScaleMillion { get; set; }

        /// <summary>
        /// 成立日期
        /// </summary>
        [Index(21)]
        public string EstablishmentDate { get; set; }

        /// <summary>
        /// 成立規模(百萬)
        /// </summary>
        [Index(22)]
        public decimal? EstablishmentScaleMillion { get; set; }

        /// <summary>
        /// 計價幣別
        /// </summary>
        [Index(23)]
        public string ValuationCurrency { get; set; }

        /// <summary>
        /// 銷售費(%)
        /// </summary>
        [Index(24)]
        public decimal? SalesFee { get; set; }

        /// <summary>
        /// 經理費(%)
        /// </summary>
        [Index(25)]
        public decimal? ManagementFee { get; set; }

        /// <summary>
        /// 保管費(%)
        /// </summary>
        [Index(26)]
        public decimal? CustodyFee { get; set; }

        /// <summary>
        /// 註冊地
        /// </summary>
        [Index(27)]
        public string RegistrationLocation { get; set; }

        /// <summary>
        /// 傘型基金
        /// </summary>
        [Index(28)]
        public string UmbrellaFund { get; set; }

        /// <summary>
        /// 保管銀行/機構
        /// </summary>
        [Index(29)]
        public string CustodianInstitution { get; set; }

        /// <summary>
        /// 基金統編
        /// </summary>
        [Index(30)]
        public string FundUnifiedIdentificationNumber { get; set; }

        /// <summary>
        /// 風險收益等級(RR)
        /// </summary>
        [Index(31)]
        public string RiskRewardLevel { get; set; }

        /// <summary>
        /// 配息頻率ID
        /// </summary>
        [Index(32)]
        public string DividendFrequencyID { get; set; }

        /// <summary>
        /// 配息頻率名稱
        /// </summary>
        [Index(33)]
        public string DividendFrequencyName { get; set; }

        /// <summary>
        /// 下架註記
        /// </summary>
        [Index(34)]
        public string DelistingIndicator { get; set; }

        /// <summary>
        /// 投資策略/投資標的
        /// </summary>
        [Index(35)]
        public string InvestmentStrategyInvestmentTarget { get; set; }
    }
}