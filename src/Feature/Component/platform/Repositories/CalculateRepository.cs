using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Component.Models.Calculate;
using Feature.Wealth.Component.Models.ETF;
using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.Component.Models.Invest;
using Feature.Wealth.Component.Models.USStock;
using Foundation.Wealth.Extensions;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Manager;
using Foundation.Wealth.Models;
using log4net;
using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Extensions;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;
using static Feature.Wealth.Component.ComponentTemplates;
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
            ? ItemUtils.GetFieldValue(defaultRiskAttributes, ComponentTemplates.SimpleDropdownOption.Fields.OptionValue)
            : GetOptionValue("保守型");

            model.Conservative = GetOptionValue("保守型");
            model.Stable = GetOptionValue("穩健型");
            model.Positive = GetOptionValue("積極型");

            model.RecommendedProductContent = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.RecommendedProductContent);
            model.ConservativeRiskImage = ItemUtils.ImageUrl(dataSource, Templates.Calculate.Fields.ConservativeRiskImage);
            model.ConservativeRiskStockAllocation = ItemUtils.GetInteger(dataSource, Templates.Calculate.Fields.ConservativeRiskStockAllocation) ?? 0;
            model.ConservativeRiskBondAllocation = ItemUtils.GetInteger(dataSource, Templates.Calculate.Fields.ConservativeRiskBondAllocation) ?? 0;
            model.ConservativeRiskCurrencyAllocation = ItemUtils.GetInteger(dataSource, Templates.Calculate.Fields.ConservativeRiskCurrencyAllocation) ?? 0;
            model.ConservativeRiskStockAllocationFieldName = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.ConservativeRiskStockAllocationFieldName);
            model.ConservativeRiskBondAllocationFieldName = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.ConservativeRiskBondAllocationFieldName);
            model.ConservativeRiskCurrencyAllocationFieldName = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.ConservativeRiskCurrencyAllocationFieldName);
            model.ConservativeRiskStockAllocationText = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.ConservativeRiskStockAllocationText);
            model.ConservativeRiskBondAllocationText = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.ConservativeRiskBondAllocationText);
            model.ConservativeRiskCurrencyAllocationText = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.ConservativeRiskCurrencyAllocationText);
            model.RobustRiskImage = ItemUtils.ImageUrl(dataSource, Templates.Calculate.Fields.RobustRiskImage);
            model.RobustRiskStockAllocation = ItemUtils.GetInteger(dataSource, Templates.Calculate.Fields.RobustRiskStockAllocation) ?? 0;
            model.RobustRiskBondAllocation = ItemUtils.GetInteger(dataSource, Templates.Calculate.Fields.RobustRiskBondAllocation) ?? 0;
            model.RobustRiskCurrencyAllocation = ItemUtils.GetInteger(dataSource, Templates.Calculate.Fields.RobustRiskCurrencyAllocation) ?? 0;
            model.RobustRiskStockAllocationFieldName = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.RobustRiskStockAllocationFieldName);
            model.RobustRiskBondAllocationFieldName = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.RobustRiskBondAllocationFieldName);
            model.RobustRiskCurrencyAllocationFieldName = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.RobustRiskCurrencyAllocationFieldName);
            model.RobustRiskStockAllocationText = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.RobustRiskStockAllocationText);
            model.RobustRiskBondAllocationText = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.RobustRiskBondAllocationText);
            model.RobustRiskCurrencyAllocationText = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.RobustRiskCurrencyAllocationText);
            model.PositiveRiskImage = ItemUtils.ImageUrl(dataSource, Templates.Calculate.Fields.PositiveRiskImage);
            model.PositiveRiskStockAllocation = ItemUtils.GetInteger(dataSource, Templates.Calculate.Fields.PositiveRiskStockAllocation) ?? 0;
            model.PositiveRiskBondAllocation = ItemUtils.GetInteger(dataSource, Templates.Calculate.Fields.PositiveRiskBondAllocation) ?? 0;
            model.PositiveRiskCurrencyAllocation = ItemUtils.GetInteger(dataSource, Templates.Calculate.Fields.PositiveRiskCurrencyAllocation) ?? 0;
            model.PositiveRiskStockAllocationFieldName = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.PositiveRiskStockAllocationFieldName);
            model.PositiveRiskBondAllocationFieldName = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.PositiveRiskBondAllocationFieldName);
            model.PositiveRiskCurrencyAllocationFieldName = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.PositiveRiskCurrencyAllocationFieldName);
            model.PositiveRiskStockAllocationText = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.PositiveRiskStockAllocationText);
            model.PositiveRiskBondAllocationText = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.PositiveRiskBondAllocationText);
            model.PositiveRiskCurrencyAllocationText = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.PositiveRiskCurrencyAllocationText);
            model.RemoteConsultationSuccessTitle = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.RemoteConsultationSuccessTitle);
            model.RemoteConsultationSuccessContent = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.RemoteConsultationSuccessContent);
            model.RemoteConsultationSuccessButtonText = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.RemoteConsultationSuccessButtonText);
            model.RemoteConsultationSuccessButtonLink = ItemUtils.GeneralLink(dataSource, Templates.Calculate.Fields.RemoteConsultationSuccessButtonLink).Url;
            model.RemoteConsultationSuccessImage = ItemUtils.ImageUrl(dataSource, Templates.Calculate.Fields.RemoteConsultationSuccessImage);
            model.RemoteConsultationSuccessfulTitle = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.RemoteConsultationSuccessfulTitle);
            model.RemoteConsultationSuccessfulContent = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.RemoteConsultationSuccessfulContent);
            model.RemoteConsultationSuccessfulButtonText = ItemUtils.GetFieldValue(dataSource, Templates.Calculate.Fields.RemoteConsultationSuccessfulButtonText);
            model.RemoteConsultationSuccessfulButtonLink = ItemUtils.GeneralLink(dataSource, Templates.Calculate.Fields.RemoteConsultationSuccessfulButtonLink).Url;
            model.RemoteConsultationSuccessfulImage = ItemUtils.ImageUrl(dataSource, Templates.Calculate.Fields.RemoteConsultationSuccessfulImage);
            model.FundID = ItemUtils.GetMultiLineText(dataSource, Templates.Calculate.Fields.FundID);

            return model;
        }

        /// <summary>
        /// 獲取增加投報率設定值
        /// </summary>
        /// <param name="optionText"></param>
        /// <returns>投報率設定值</returns>
        private string GetOptionValue(string optionText)
        {
            var item = Sitecore.Context.Database.GetItem(new ID("{909E3250-22E5-4E06-85DD-B11262B416A4}"));

            if (item == null)
            {
                return null;
            }

            var optionItem = item.Children
                .FirstOrDefault(child => child.Fields[SimpleDropdownOption.Fields.OptionText]?.Value == optionText);

            return optionItem?.Fields[SimpleDropdownOption.Fields.OptionValue]?.Value;
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
                    Description = HttpUtility.UrlDecode(data.Description),
                    EarningsChart = data.EarningsChart.Select(HttpUtility.UrlDecode).ToList(),
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
                    USING (SELECT @id COLLATE Latin1_General_CS_AS AS PlatFormId) AS source
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

        /// <summary>
        /// 取得基金資料
        /// </summary>
        /// <param name="ExpectedRoi">預期投資報酬率</param>
        /// <param name="ProductFundIDs">推薦基金商品ID</param>
        /// <param name="RiskLevel">商品風險屬性</param>
        /// <returns>9筆基金資料</returns>
        public List<FundModel> GetFundData(string ExpectedRoi, string[] ProductFundIDs, string RiskLevel)
        {
            var FundUrl = FundRelatedSettingModel.GetFundDetailsUrl();
            string FundDataSql;
            if (string.IsNullOrEmpty(ExpectedRoi))
            {
                FundDataSql = @$"
                 SELECT TOP 9 [ProductCode], [FundName], [OneMonthReturnOriginalCurrency], [AvailabilityStatus], [OnlineSubscriptionAvailability]
                 FROM [dbo].[vw_BasicFund]
                 WHERE [RiskLevel] IN ({RiskLevel})
                 ORDER BY [OneYearReturnOriginalCurrency] DESC, [ProductCode] ASC";
            }
            else
            {
                FundDataSql = @$"
                 SELECT TOP 9 [ProductCode], [FundName], [OneMonthReturnOriginalCurrency], [AvailabilityStatus], [OnlineSubscriptionAvailability]
                 FROM [dbo].[vw_BasicFund]
                 WHERE [OneYearReturnOriginalCurrency] >= '{ExpectedRoi}' AND [RiskLevel] IN ({RiskLevel})
                 ORDER BY [OneYearReturnOriginalCurrency] DESC, [ProductCode] ASC";
            }
            var FundData = DbManager.Custom.ExecuteIList<FundModel>(FundDataSql, null, CommandType.Text);

            foreach (var fund in FundData)
            {
                fund.DataIsFormSitecore = false;
            }

            if (FundData.Count < 9 && RiskLevel != "'RR1'")
            {
                var allRecommendedProductCodesSql = string.Join(", ", ProductFundIDs.Select(pc => $"'{pc}'"));
                string allRecommendedDataSql = @$"
                 SELECT [ProductCode], [FundName], [OneMonthReturnOriginalCurrency], [AvailabilityStatus], [OnlineSubscriptionAvailability]
                 FROM [dbo].[vw_BasicFund]
                 WHERE [ProductCode] IN ({allRecommendedProductCodesSql})";

                var allRecommendedData = DbManager.Custom.ExecuteIList<FundModel>(allRecommendedDataSql, null, CommandType.Text);

                var existingProductCodes = FundData.Select(e => e.ProductCode).ToHashSet();
                var neededCount = 9 - FundData.Count;

                if (neededCount > 0)
                {
                    var additionalData = allRecommendedData
                        .Where(r => !existingProductCodes.Contains(r.ProductCode))
                        .OrderBy(r => Array.IndexOf(ProductFundIDs, r.ProductCode))
                        .Take(neededCount)
                        .ToList();
                    foreach (var fund in additionalData)
                    {
                        fund.DataIsFormSitecore = true;
                    }
                    FundData.AddRange(additionalData);
                }
            }


            foreach (var item in FundData)
            {
                item.DisplayOneMonthReturnOriginalCurrency = item.OneMonthReturnOriginalCurrency.FormatDecimalNumber(2);

                //「是否上架」= Y 且「是否可於網路申購」= Y或空白, 顯示申購鈕
                if (item.AvailabilityStatus == "Y" &&
                    (item.OnlineSubscriptionAvailability == "Y" ||
                    string.IsNullOrEmpty(item.OnlineSubscriptionAvailability)))
                {
                    item.SubscribeButtonHtml = PublicHelpers.SubscriptionButtonForCard(null, null, item.ProductCode, InvestTypeEnum.Fund).ToString();
                }
                else
                {
                    item.SubscribeButtonHtml = string.Empty;
                }

                item.FocusButtonHtml = PublicHelpers.FocusButton(null, null, item.ProductCode, item.FundName, InvestTypeEnum.Fund, false).ToString();
                item.CompareButtonHtml = PublicHelpers.CompareButton(null, null, item.ProductCode, item.FundName, InvestTypeEnum.Fund, false).ToString();
                item.FundDetailUrl = $"{FundUrl}?id={item.ProductCode}";

                string tablename = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Nav_Fund);

                string sql = @$"
                 SELECT [NetAssetValue]
                 FROM {tablename} WITH (NOLOCK)
                 WHERE [FirstBankCode] = '{item.ProductCode}'AND [NetAssetValue] IS NOT NULL
                 ORDER BY [NetAssetValueDate] ASC;";

                item.SysjustNavFundData = (List<decimal>)DbManager.Custom.ExecuteIList<decimal>(sql, null, CommandType.Text);
            }

            return (List<FundModel>)FundData;
        }

        /// <summary>
        /// 取得ETF資料
        /// </summary>
        /// <param name="ExpectedRoi">預期投資報酬率</param>
        /// <param name="RiskLevel">風險屬性</param>
        /// <returns>3筆ETF資料</returns>
        public List<EtfModel> GetEtfData(string ExpectedRoi, string RiskLevel)
        {
            var ETFUrl = EtfRelatedLinkSetting.GetETFDetailUrl();

            string EtfDataSql;
            if (string.IsNullOrEmpty(ExpectedRoi))
            {
                EtfDataSql = @$"
                 SELECT TOP 3 [ProductCode], [ETFName], [MonthlyReturnNetValueOriginalCurrency], [AvailabilityStatus], [OnlineSubscriptionAvailability]
                 FROM [dbo].[vw_BasicETF]
                 WHERE [RiskLevel] IN ({RiskLevel})
                 ORDER BY [OneYearReturnMarketPriceOriginalCurrency] DESC, [ProductCode] ASC";
            }
            else
            {
                EtfDataSql = @$"
                 SELECT TOP 3 [ProductCode], [ETFName], [MonthlyReturnNetValueOriginalCurrency], [AvailabilityStatus], [OnlineSubscriptionAvailability]
                 FROM [dbo].[vw_BasicETF]
                 WHERE [OneYearReturnMarketPriceOriginalCurrency] >= '{ExpectedRoi}' AND [RiskLevel] IN ({RiskLevel})
                 ORDER BY [OneYearReturnMarketPriceOriginalCurrency] DESC, [ProductCode] ASC";
            }
            var EtfData = DbManager.Custom.ExecuteIList<EtfModel>(EtfDataSql, null, CommandType.Text);

            if (EtfData.Count < 3 && RiskLevel != "'RR1'")
            {
                EtfDataSql = @$"
                 SELECT TOP 3 [ProductCode], [ETFName], [MonthlyReturnNetValueOriginalCurrency], [AvailabilityStatus], [OnlineSubscriptionAvailability]
                 FROM [dbo].[vw_BasicETF]
                 ORDER BY [OneYearReturnMarketPriceOriginalCurrency] DESC, [ProductCode] ASC";

                EtfData = DbManager.Custom.ExecuteIList<EtfModel>(EtfDataSql, null, CommandType.Text);
            }

            foreach (var item in EtfData)
            {
                item.DisplayMonthlyReturnNetValueOriginalCurrency = item.MonthlyReturnNetValueOriginalCurrency.FormatDecimalNumber(2);

                //「是否上架」= Y 且「是否可於網路申購」= Y或空白, 顯示申購鈕
                if (item.AvailabilityStatus == "Y" &&
                    (item.OnlineSubscriptionAvailability == "Y" ||
                    string.IsNullOrEmpty(item.OnlineSubscriptionAvailability)))
                {
                    item.SubscribeButtonHtml = PublicHelpers.SubscriptionButtonForCard(null, null, item.ProductCode, InvestTypeEnum.ETF).ToString();
                }
                else
                {
                    item.SubscribeButtonHtml = string.Empty;
                }

                item.FocusButtonHtml = PublicHelpers.FocusButton(null, null, item.ProductCode, item.ETFName, InvestTypeEnum.ETF, false).ToString();
                item.CompareButtonHtml = PublicHelpers.CompareButton(null, null, item.ProductCode, item.ETFName, InvestTypeEnum.ETF, false).ToString();
                item.ETFDetailUrl = $"{ETFUrl}?id={item.ProductCode}";
                string tablename = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Nav_ETF);

                string sql = @$"
                 SELECT [NetAssetValue]
                 FROM {tablename} WITH (NOLOCK)
                 WHERE [FirstBankCode] = '{item.ProductCode}'AND [NetAssetValue] IS NOT NULL
                 ORDER BY [NetAssetValueDate] ASC;";

                item.SysjustNavEtfData = (List<decimal>)DbManager.Custom.ExecuteIList<decimal>(sql, null, CommandType.Text);
            }

            return (List<EtfModel>)EtfData;
        }
        /// <summary>
        /// 取得國外股票資料
        /// </summary>
        /// <param name="ExpectedRoi"></param>
        /// <returns></returns>
        public List<StockModel> GetStockData(string ExpectedRoi)
        {
            var StockUrl = USStockRelatedLinkSetting.GetUSStockSearchUrl();
            string StockDataSql;
            string Sysjust_USStockList = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_USStockList);
            string WMS_DOC_RECM = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.WMS_DOC_RECM);


            if (string.IsNullOrEmpty(ExpectedRoi))
            {
                StockDataSql = @$"
                SELECT TOP 3
                    A.[FirstBankCode], 
                    A.[ChineseName], 
                    A.[MonthlyReturn],
                    B.[AvailabilityStatus], 
                    B.[OnlineSubscriptionAvailability]
                FROM 
                    {Sysjust_USStockList} A WITH (NOLOCK)
                JOIN 
                    {WMS_DOC_RECM} B WITH (NOLOCK)
                ON 
                    A.[FirstBankCode] = B.[ProductCode]
                ORDER BY 
                    A.[OneYearReturn] DESC, 
                    A.[FirstBankCode] ASC
                ";
            }
            else
            {
                StockDataSql = @$"
                SELECT TOP 3
                    A.[FirstBankCode], 
                    A.[ChineseName], 
                    A.[MonthlyReturn],
                    B.[AvailabilityStatus], 
                    B.[OnlineSubscriptionAvailability]
                FROM 
                    {Sysjust_USStockList} A WITH (NOLOCK)
                JOIN 
                    {WMS_DOC_RECM} B WITH (NOLOCK)
                ON 
                    A.[FirstBankCode] = B.[ProductCode]
                WHERE 
                    A.[OneYearReturn] >= '{ExpectedRoi}'
                ORDER BY 
                    A.[OneYearReturn] DESC, 
                    A.[FirstBankCode] ASC
                ";
            }
            var StockData = DbManager.Custom.ExecuteIList<StockModel>(StockDataSql, null, CommandType.Text);

            if (StockData.Count < 3)
            {
                StockDataSql = @$"
                SELECT TOP 3
                    A.[FirstBankCode], 
                    A.[ChineseName], 
                    A.[MonthlyReturn],
                    B.[AvailabilityStatus], 
                    B.[OnlineSubscriptionAvailability]
                FROM 
                    {Sysjust_USStockList} A WITH (NOLOCK)
                JOIN 
                    {WMS_DOC_RECM} B WITH (NOLOCK)
                ON 
                    A.[FirstBankCode] = B.[ProductCode]
                ORDER BY 
                    A.[OneYearReturn] DESC, 
                    A.[FirstBankCode] ASC
                ";

                StockData = DbManager.Custom.ExecuteIList<StockModel>(StockDataSql, null, CommandType.Text);
            }

            foreach (var item in StockData)
            {
                item.DisplayMonthlyReturn = item.MonthlyReturn.FormatDecimalNumber(2);

                //「是否上架」= Y 且「是否可於網路申購」= Y或空白, 顯示申購鈕
                if (item.AvailabilityStatus == "Y" &&
                    (item.OnlineSubscriptionAvailability == "Y" ||
                    string.IsNullOrEmpty(item.OnlineSubscriptionAvailability)))
                {
                    item.SubscribeButtonHtml = PublicHelpers.SubscriptionButtonForCard(null, null, item.FirstBankCode, InvestTypeEnum.ForeignStocks).ToString();
                }
                else
                {
                    item.SubscribeButtonHtml = string.Empty;
                }

                item.FocusButtonHtml = PublicHelpers.FocusButton(null, null, item.FirstBankCode, item.ChineseName, InvestTypeEnum.ForeignStocks, false).ToString();
                item.StockDetailUrl = $"{StockUrl}/stock-details?id={item.FirstBankCode}";

                string sql = @$"
                 SELECT [ClosingPrice]
                 FROM [Sysjust_USStockList_History] WITH (NOLOCK)
                 WHERE [FirstBankCode] = '{item.FirstBankCode}'AND [ClosingPrice] IS NOT NULL
                 ORDER BY [DataDate] ASC;";

                item.ClosingPrice = (List<decimal>)DbManager.Custom.ExecuteIList<decimal>(sql, null, CommandType.Text);
            }

            return (List<StockModel>)StockData;
        }
    }
}