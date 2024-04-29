using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.FundDetail
{
    public class FundBase
    {
        /// <summary>
        /// 商品代號
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 基金名稱中文
        /// </summary>
        public string FundName { get; set; }
        /// <summary>
        /// 是否為百元基金
        /// </summary>
        public string TargetName { get; set; }
        /// <summary>
        /// 基金公司ID
        /// </summary>
        public string FundCompanyID { get; set; }
        /// <summary>
        /// 基金名稱英文
        /// </summary>
        public string FundEnglishName { get; set; }
        /// <summary>
        /// 最新淨值
        /// </summary>
        public decimal? NetAssetValue { get; set; }

        /// <summary>
        /// 三個月內最高淨值
        /// </summary>
        public decimal? MaxNetAssetValueThreeMonths { get; set; }
        /// <summary>
        /// 三個月內最低淨值
        /// </summary>
        public decimal? MinNetAssetValueThreeMonths { get; set; }

        /// <summary>
        /// 漲幅
        /// </summary>
        public decimal? QuoteChange { get; set; }
        /// <summary>
        /// 漲跌幅%
        /// </summary>
        public decimal? QuoteChangePercent { get; set; }

        /// <summary>
        /// 計價幣別
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 計價幣別-中文
        /// </summary>
        public string ValuationCurrency { get; set; }

        /// <summary>
        /// 最新淨值日期
        /// </summary>
        public string NetAssetValueDate { get; set; }

        /// <summary>
        /// 風險等級
        /// </summary>
        public string RiskLevel { get; set; }
        /// <summary>
        /// 網路申購
        /// </summary>
        public string OnlineSubscriptionAvailability { get; set; }
         /// <summary>
         /// 配息頻率
         /// </summary>
        public string DividendFrequencyName { get; set; }
        /// <summary>
        /// 投資區域
        /// </summary>
        public string InvestmentRegionName { get; set; }
        /// <summary>
        /// 基金規模
        /// </summary>
        public string FundScaleMillion { get; set; }

        /// <summary>
        /// 基金規模幣別
        /// </summary>
        public string FundScaleCurreny { get; set; }

        /// <summary>
        /// 基金規模日期
        /// </summary>
        public string FundScaleDate { get; set; }
        /// <summary>
        /// 投資標的
        /// </summary>
        public string InvestmentTargetName { get; set; }
        /// <summary>
        /// 基金類型
        /// </summary>
        public string FundTypeName { get; set; }
        /// <summary>
        /// 基金評等
        /// </summary>
        public string FundRating { get; set; }
        /// <summary>
        /// 成立日期
        /// </summary>
        public string EstablishmentDate { get; set; }
        /// <summary>
        /// 是否上架
        /// </summary>
        public string WithdrawnApproval { get; set; }
        /// <summary>
        /// 上架時間
        /// </summary>
        public string ListingDate { get; set; }
        /// <summary>
        /// 已撤銷核備
        /// </summary>
        public string AvailabilityStatus { get; set; }
        /// <summary>
        /// 下架時間
        /// </summary>
        public string DelistingDate { get; set; }
        /// <summary>
        /// 保管機構/銀行
        /// </summary>
        public string CustodianInstitution { get; set; }
        /// <summary>
        /// 經理人
        /// </summary>
        public string FundManager { get; set; }
        /// <summary>
        /// 申購手續費
        /// </summary>
        public string SubscriptionFee { get; set; }
        /// <summary>
        /// 贖回手續費
        /// </summary>
        public string RedemptionFee { get; set; }
        /// <summary>
        /// 申購淨值日
        /// </summary>
        public string SubscriptionDate { get; set; }
        /// <summary>
        /// 贖回淨值日
        /// </summary>
        public string RedemptionDate { get; set; }
        /// <summary>
        /// 贖回入款日
        /// </summary>
        public string RedemptionDepositDate { get; set; }
        /// <summary>
        /// 最低單筆投資金額
        /// </summary>
        public string MinimumSingleInvestmentAmount { get; set; }
        /// <summary>
        /// 最低定期定額投資金額
        /// </summary>
        public string MinimumRegularInvestmentAmount { get; set; }
        /// <summary>
        /// 最低定期不定額投資金額
        /// </summary>
        public string MinimumIrregularInvestmentAmount { get; set; }
        /// <summary>
        /// 銀行相關說明
        /// </summary>
        public string BankRelatedInstructions { get; set; }

        /// <summary>
        /// 投資策略/目的 境外:策略 國內:目的
        /// </summary>
        public string InvestmentStrategyInvestmentTarget { get; set; }

        /// <summary>
        /// 最近一次配息發放日
        /// </summary>
        public string ReleaseDate { get; set; }
        /// <summary>
        /// 最近一次除息日
        /// </summary>
        public string ExDividendDate { get; set; }
        /// <summary>
        /// 每單位分配金額
        /// </summary>
        public string Dividend { get; set; }

        /// <summary>
        /// 三個月績效表現
        /// </summary>
        public decimal? ThreeMonthReturnOriginalCurrency { get; set; }
        /// <summary>
        /// 六個月績效表現
        /// </summary>
        public decimal? SixMonthReturnOriginalCurrency { get; set; }
        /// <summary>
        /// 年初至今績效現
        /// </summary>
        public decimal? YeartoDateReturnOriginalCurrency { get; set; }

        public string ViewCount { get; set; }

    }
}
