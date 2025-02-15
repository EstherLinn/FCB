﻿using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.ScheduleAgent.Models.Wealth;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Manager;
using Foundation.Wealth.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Component.Repositories
{
    public class FundRepository
    {
        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly string FundDetailsCacheKey = $"Fcb_FundDetailsCache";
        private readonly string FundDetailTempCacheKey = $"Fcb_FundDetailTemp";

        /// <summary>
        /// 新元件使用
        /// </summary>
        /// <param name="fundId"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public FundViewModel GetOrSetFundDetailsCacheNew(string fundId, string indicator)
        {
            var FundDic = (Dictionary<string, FundViewModel>)_cache.Get(FundDetailsCacheKey) ?? new Dictionary<string, FundViewModel>();
            FundViewModel FundFullData;
            if (!FundDic.Any())
            {
                FundDic.Add(fundId, CreateFundDetailsDataNew(fundId, indicator));
                _cache.Set(FundDetailsCacheKey, FundDic, DateTimeOffset.Now.AddMinutes(60));
            }
            else
            {
                if (FundDic.TryGetValue(fundId, out FundFullData))
                {
                    return FundFullData;
                }
                else
                {
                    FundDic.Add(fundId, CreateFundDetailsDataNew(fundId, indicator));
                }
            }
            FundFullData = FundDic[fundId];
            return FundFullData;
        }

        /// <summary>
        /// 新元件使用
        /// </summary>
        /// <param name="fundId"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        private FundViewModel CreateFundDetailsDataNew(string fundId, string indicator)
        {
            FundViewModel fundViewModel = new FundViewModel();
            if (indicator == nameof(FundEnum.D))
            {
                fundViewModel.FundBaseData = GetDomesticFundBasic(fundId);
                fundViewModel.FundAccordingStockHoldings = GetFundHoldingStockPercent(fundId);
                fundViewModel.FundTypeRanks = GetSameTypeFundListDatas(GetSameTypeFundList(fundId), fundId);

            }
            else
            {
                fundViewModel.FundBaseData = GetOverseasFundBasic(fundId);
                fundViewModel.FundTypeRanks = GetSameTypeFundListDatas(GetSameTypeFundListByOverseas(fundId), fundId);
            }

            fundViewModel.TagsDic = GetTagsById(fundId);
            fundViewModel.FundCloseYearsNetValue = GetNetAssetValueWithCloseYear(fundId);
            fundViewModel.FundRateOfReturn = GetRateOfReturn(fundId);
            fundViewModel.FundThiryDaysNetValue = GetThrityDaysNetValue(fundId);
            fundViewModel.FundAnnunalRateOfReturn = GetAnnualRateOfReturn(fundId);
            fundViewModel.FundAccumulationRateOfReturn = GetAccumulationRateOfReturn(fundId);
            fundViewModel.FundDowJonesIndexs = GetDowJonesIndex(fundId);
            fundViewModel.FundStockHoldings = GetFundIndustryPercent(fundId);
            fundViewModel.FundStockAreaHoldings = GetFundAreaPercent(fundId);
            fundViewModel.FundTopTenStockHolding = GetTopTenHoldingFund(fundId, indicator);
            fundViewModel.FundRiskindicators = GetRiskindicators(fundId);
            fundViewModel.FundReturnCompare = GetRateOfReturnCompare(fundId, indicator);
            fundViewModel.FundYearRateOfReturn = GetYearRateOfReturnCompare(fundId);
            fundViewModel.FundDividendRecords = GetDividendRecord(fundId);
            fundViewModel.FundScaleRecords = GetScaleMove(fundId, indicator);
            fundViewModel.FeePostCollectionType = GetFeePostCollectionType(fundId);
            if (fundViewModel.FundBaseData != null)
            {
                fundViewModel.SameCompanyFunds = GetSameCompanyFunds(fundId, fundViewModel.FundBaseData.FundCompanyID);
                fundViewModel.FundBaseData.IndicatorIndexCode = GetIndicatorIndexCode(fundId, indicator);
            }
            return fundViewModel;
        }
        public FundViewModel GetOrSetFundDetailsCache(string fundId, string indicator)
        {
            var FundDic = (Dictionary<string, FundViewModel>)_cache.Get(FundDetailsCacheKey) ?? new Dictionary<string, FundViewModel>();
            FundViewModel FundFullData;
            if (!FundDic.Any())
            {
                FundDic.Add(fundId, CreateFundDetailsData(fundId, indicator));
                _cache.Set(FundDetailsCacheKey, FundDic, DateTimeOffset.Now.AddMinutes(60));
            }
            else
            {
                if (FundDic.TryGetValue(fundId, out FundFullData))
                {
                    return FundFullData;
                }
                else
                {
                    FundDic.Add(fundId, CreateFundDetailsData(fundId, indicator));
                }
            }
            FundFullData = FundDic[fundId];
            return FundFullData;
        }

        private FundViewModel CreateFundDetailsData(string fundId, string indicator)
        {
            FundViewModel fundViewModel = new FundViewModel();
            if (indicator == nameof(FundEnum.D))
            {
                fundViewModel.FundBaseData = GetDomesticFundBasic(fundId);
                fundViewModel.FundAccordingStockHoldings = GetFundHoldingStockPercent(fundId);
            }
            else
            {
                fundViewModel.FundBaseData = GetOverseasFundBasic(fundId);
            }

            fundViewModel.TagsDic = GetTagsById(fundId);
            fundViewModel.FundCloseYearsNetValue = GetNetAssetValueWithCloseYear(fundId);
            fundViewModel.FundTypeRanks = GetSameTypeFundRank(fundId);
            fundViewModel.FundRateOfReturn = GetRateOfReturn(fundId);
            fundViewModel.FundThiryDaysNetValue = GetThrityDaysNetValue(fundId);
            fundViewModel.FundAnnunalRateOfReturn = GetAnnualRateOfReturn(fundId);
            fundViewModel.FundAccumulationRateOfReturn = GetAccumulationRateOfReturn(fundId);
            fundViewModel.FundDowJonesIndexs = GetDowJonesIndex(fundId);
            fundViewModel.FundStockHoldings = GetFundIndustryPercent(fundId);
            fundViewModel.FundStockAreaHoldings = GetFundAreaPercent(fundId);
            fundViewModel.FundTopTenStockHolding = GetTopTenHoldingFund(fundId, indicator);
            fundViewModel.FundRiskindicators = GetRiskindicators(fundId);
            fundViewModel.FundReturnCompare = GetRateOfReturnCompare(fundId, indicator);
            fundViewModel.FundYearRateOfReturn = GetYearRateOfReturnCompare(fundId);
            fundViewModel.FundDividendRecords = GetDividendRecord(fundId);
            fundViewModel.FundScaleRecords = GetScaleMove(fundId, indicator);
            fundViewModel.FeePostCollectionType = GetFeePostCollectionType(fundId);
            if (fundViewModel.FundBaseData != null)
            {
                fundViewModel.SameCompanyFunds = GetSameCompanyFunds(fundId, fundViewModel.FundBaseData.FundCompanyID);
                fundViewModel.FundBaseData.IndicatorIndexCode = GetIndicatorIndexCode(fundId, indicator);
            }
            return fundViewModel;
        }
        /// <summary>
        /// 取得國內外註記
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public string GetDometicOrOverseas(string fundId)
        {
            string FUND_BSC = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.FUND_BSC);
            string sql = $@"Select DomesticForeignFundIndicator From {FUND_BSC} (NOLOCK) Where  [BankProductCode] =@fundId";
            var para = new { fundId };
            string indicator = DbManager.Custom.Execute<string>(sql, para, commandType: System.Data.CommandType.Text);
            return indicator;
        }
        /// <summary>
        /// 取得前後收型
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public string GetFeePostCollectionType(string fundId)
        {
            string FUNDTRB = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.FUNDTRB);
            string sql = $@"Select FeePostCollectionType From {FUNDTRB} (NOLOCK) Where  [ProductCode] =@fundId";
            var para = new { fundId };
            string type = DbManager.Custom.Execute<string>(sql, para, commandType: System.Data.CommandType.Text);
            return type;
        }

        public Dictionary<FundTagEnum, List<string>> GetTagsById(string fundId)
        {
            FundTagRepository tagsRepository = new FundTagRepository();
            var dic = new Dictionary<FundTagEnum, List<string>>() {
                {FundTagEnum.DiscountTag, new List<string>() },
                {FundTagEnum.SortTag, new List<string>() }
            };
            var data = tagsRepository.GetFundTagData();
            if (data != null)
            {
                foreach (var item in data)
                {
                    if (item.FundTagType == FundTagEnum.KeywordTag)
                    {
                        continue;
                    }
                    if (item.ProductCodes.Contains(fundId))
                    {
                        dic[item.FundTagType].Add(item.TagName);
                    }
                }

            }
            return dic;
        }

        /// <summary>
        /// 取得國內基金基本資料
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public DomesticFundBase GetDomesticFundBasic(string fundId) => DbManager.Custom.Execute<DomesticFundBase>("sp_DomesticFundBasicData", new { fundId }, commandType: System.Data.CommandType.StoredProcedure);

        /// <summary>
        /// 取得海外基金基本資料
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public OverseasFundBase GetOverseasFundBasic(string fundId) => DbManager.Custom.Execute<OverseasFundBase>("sp_OverseasFundBasicData", new { fundId }, commandType: System.Data.CommandType.StoredProcedure);

        /// <summary>
        /// 取得最近一年淨值
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public List<FundCloseYearNetValue> GetNetAssetValueWithCloseYear(string fundId)
        {
            string sql = @"SELECT Format([Date],'yyyy-MM-dd') NetAssetValueDate,[NetAssetValue] 
                            FROM [Sysjust_FUNDNAV_HIS] WITH (NOLOCK) where [FirstBankCode]=@fundId and CAST([Date] AS date) >= CAST(DATEADD(year, -1, GETDATE())AS date) ORDER BY [Date] ";
            var para = new { fundId };
            List<FundCloseYearNetValue> fundCloseYearNetValue = DbManager.Custom.ExecuteIList<FundCloseYearNetValue>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();
            return fundCloseYearNetValue;
        }

        /// <summary>
        /// 取得同類基金list 國內
        /// </summary>
        /// <param name="fundId">基金id</param>
        /// <returns></returns>
        public List<string> GetSameTypeFundList(string fundId)
        {
            string Sysjust_Basic_Fund = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Basic_Fund);
            string WMS_DOC_RECM = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.WMS_DOC_RECM);
            string FUND_BSC = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.FUND_BSC);
            List<string> fundList = new List<string>();
            string planA_Sql = $@"DECLARE @var1 nvarchar(100), @var2 nvarchar(100), @var3 nvarchar(100);
                            SELECT @var1 =[FundTypeName] FROM  {FUND_BSC}  with (nolock) where BankProductCode = @fundId
                            SELECT @var2 = [InvestmentRegionName], @var3 = [PrimaryInvestmentRegion]
                            FROM {Sysjust_Basic_Fund} with (nolock) where FirstBankCode = @fundId

                            select FirstBankCode from {Sysjust_Basic_Fund} as a  with (nolock)
                            left join {FUND_BSC}  as b  with (nolock)  on a.FirstBankCode = b.BankProductCode
                            inner join {WMS_DOC_RECM}  as c  with (nolock) on a.FirstBankCode = c.ProductCode
                            where [FundTypeName] = @var1 and a.[InvestmentRegionName] = @var2 and a.[PrimaryInvestmentRegion] = @var3 and c.AvailabilityStatus = 'Y'";
            var para = new { fundId };
            fundList = DbManager.Custom.ExecuteIList<string>(planA_Sql, para, commandType: System.Data.CommandType.Text)?.ToList();
            //方案A有達6檔且不超過9檔
            if (fundList != null && fundList.Count >= 6 && fundList.Count <= 9)
            {
                return fundList;
            }
            //方案A超過9檔，執行方案B
            if (fundList != null && fundList.Count > 9)
            {

                string planB_Sql = $@"DECLARE @var1 nvarchar(100), @var2 nvarchar(100), @var3 nvarchar(100), @var4 nvarchar(100);
                            SELECT @var1 =[FundTypeName] FROM  {FUND_BSC}  with (nolock) where BankProductCode = @fundId
                            SELECT @var2 = [InvestmentRegionName], @var3 = [PrimaryInvestmentRegion], @var4 = [IndicatorIndexCode]
                            FROM {Sysjust_Basic_Fund} with (nolock) where FirstBankCode = @fundId

                            select FirstBankCode from {Sysjust_Basic_Fund} as a  with (nolock)
                            left join {FUND_BSC}  as b  with (nolock)  on a.FirstBankCode = b.BankProductCode
                            inner join {WMS_DOC_RECM}  as c  with (nolock) on a.FirstBankCode = c.ProductCode
                            where [FundTypeName] = @var1 and a.[InvestmentRegionName] = @var2 and a.[PrimaryInvestmentRegion] = @var3 and a.IndicatorIndexCode = @var4 and c.AvailabilityStatus = 'Y'";
                fundList = DbManager.Custom.ExecuteIList<string>(planB_Sql, para, commandType: System.Data.CommandType.Text)?.ToList();
                //方案B有達6檔
                if (fundList != null && fundList.Count >= 6)
                {
                    return fundList;
                }
            }
            //方案A未滿6檔，執行方案C
            else if (fundList != null && fundList.Count < 6)
            {
                string planC_Sql = $@"DECLARE @var1 nvarchar(100), @var2 nvarchar(100), @var3 nvarchar(100)
                            SELECT @var1 =[FundTypeName] FROM  {FUND_BSC}  with (nolock) where BankProductCode = @fundId
                            SELECT @var2 = [InvestmentRegionName], @var3 = [IndicatorIndexCode]
                            FROM {Sysjust_Basic_Fund} with (nolock) where FirstBankCode = @fundId

                            select FirstBankCode from {Sysjust_Basic_Fund} as a  with (nolock)
                            left join {FUND_BSC}  as b  with (nolock)  on a.FirstBankCode = b.BankProductCode
                            inner join {WMS_DOC_RECM}  as c  with (nolock) on a.FirstBankCode = c.ProductCode
                            where [FundTypeName] = @var1 and a.[InvestmentRegionName] = @var2 and a.[IndicatorIndexCode] = @var3 and c.AvailabilityStatus = 'Y'";
                fundList = DbManager.Custom.ExecuteIList<string>(planC_Sql, para, commandType: System.Data.CommandType.Text)?.ToList();

            }
            return fundList;
        }

        /// <summary>
        /// 取得同類基金list 境外
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public List<string> GetSameTypeFundListByOverseas(string fundId)
        {
            string FUND_BSC = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.FUND_BSC);
            string Sysjust_Basic_Fund_2 = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Basic_Fund_2);
            string WMS_DOC_RECM = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.WMS_DOC_RECM);
            string planA_Sql = $@"DECLARE @var1 nvarchar(100), @var2 nvarchar(100), @var3 nvarchar(100);
                            SELECT @var1 = FundTypeName FROM  {FUND_BSC}  with (nolock) where BankProductCode = @fundId
							 SELECT @var2 = InvestmentRegionName, @var3 = IndicatorIndexName FROM  {Sysjust_Basic_Fund_2} with (nolock)  where FirstBankCode = @fundId
							 SELECT FirstBankCode FROM {Sysjust_Basic_Fund_2} as a  with (nolock)
							 left join {FUND_BSC} as b  with (nolock)  on a.FirstBankCode = b.BankProductCode
							 inner join {WMS_DOC_RECM} as c with (nolock) on a.FirstBankCode = c.ProductCode
							 where A.InvestmentRegionName =@var2 and A.IndicatorIndexName = @var3 and B.FundTypeName =@var1 and c.AvailabilityStatus = 'Y'";
            var para = new { fundId };
            return DbManager.Custom.ExecuteIList<string>(planA_Sql, para, commandType: System.Data.CommandType.Text)?.ToList();
        }

        /// <summary>
        /// 取得同類基金排行資料
        /// </summary>
        /// <param name="fundIds">同類基金list</param>
        /// <param name="fundId">該基金</param>
        /// <returns></returns>
        public List<FundTypeRank> GetSameTypeFundListDatas(List<string> fundIds, string fundId)
        {
            string Sysjust_Nav_Fund = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Nav_Fund);
            string Sysjust_Return_Fund = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Return_Fund);
            string FUND_BSC = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.FUND_BSC);

            string sql = $@" ;WITH CTE AS
                              (
                             select top 6 A.FirstBankCode,B.FundName,A.SixMonthReturnOriginalCurrency,
                             ROW_NUMBER() OVER (order by SixMonthReturnOriginalCurrency desc) RowNumber
                             from {Sysjust_Return_Fund}  AS A with (nolock)
                             LEFT JOIN {FUND_BSC}  AS B  with (nolock) ON  A.FirstBankCode = B.BankProductCode
                             where A.FirstBankCode  IN @fundIds and BankProductCode <> '' and BankProductCode <> @fundId
                             )
                              , CTE2 AS
                             (
                             SELECT *,ROW_NUMBER() OVER ( order by NetAssetValueDate desc) DateRowNumber
                             FROM {Sysjust_Nav_Fund}  AS A with (nolock)
                             WHERE FirstBankCode in (select FirstBankCode from CTE AS B WHERE B.RowNumber < 7)  
                             AND NetAssetValueDate =(select max(NetAssetValueDate) from {Sysjust_Nav_Fund} as B with (nolock)
                             WHERE A.FirstBankCode=B.FirstBankCode)
                             )
 
                              SELECT 
                             A.FirstBankCode, 
                             A.FundName,
                             A.SixMonthReturnOriginalCurrency,
                             B.NetAssetValue,
                             B.NetAssetValueDate,
                             a.RowNumber
                             FROM CTE AS A 
                             LEFT JOIN CTE2 AS B ON A.FirstBankCode = B.FirstBankCode 
                             Order by RowNumber";
            var para = new { fundIds, fundId };
            return DbManager.Custom.ExecuteIList<FundTypeRank>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();
        }

        /// <summary>
        /// 取得同類型基金排行sp
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public List<FundTypeRank> GetSameTypeFundRank(string fundId) => DbManager.Custom.ExecuteIList<FundTypeRank>("sp_FundSameTypeRank", new { fundId }, commandType: System.Data.CommandType.StoredProcedure)?.ToList();
        /// <summary>
        /// 取得近30日淨值
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public List<FundThiryDays> GetThrityDaysNetValue(string fundId) => DbManager.Custom.ExecuteIList<FundThiryDays>("sp_CountFundNavForThirtyDays", new { fundId }, commandType: System.Data.CommandType.StoredProcedure)?.ToList();

        /// <summary>
        /// 取得報酬率
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public FundRateOfReturn GetRateOfReturn(string fundId) => DbManager.Custom.Execute<FundRateOfReturn>("sp_FundRateOfReturn", new { fundId }, commandType: System.Data.CommandType.StoredProcedure);

        /// <summary>
        /// 取得近五年報酬率
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>

        public List<FundAnnunalRateOfReturn> GetAnnualRateOfReturn(string fundId)
        {
            string Sysjust_Return_Fund_2 = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Return_Fund_2);
            var sql = $@"SELECT TOP(5)
                       [FirstBankCode]
                      ,[Year]
                      ,[AnnualReturnRateOriginalCurrency]
                      ,[AnnualReturnRateTWD]
                      ,[IndicatorIndexPriceChange]
                      ,[AnnualReturnRateOriginalCurrency] - [IndicatorIndexPriceChange] as Difference
                  FROM {Sysjust_Return_Fund_2} WITH (NOLOCK)
                  where [FirstBankCode] = @fundId 
                  ORDER BY [Year] DESC";
            var para = new { fundId };
            List<FundAnnunalRateOfReturn> fundAnnunalRateOfReturn = DbManager.Custom.ExecuteIList<FundAnnunalRateOfReturn>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();
            return fundAnnunalRateOfReturn;
        }
        /// <summary>
        /// 取得累積報酬率
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public FundAccumulationRateOfReturn GetAccumulationRateOfReturn(string fundId)
        {
            string Sysjust_Return_Fund = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Return_Fund);
            string sql = $@"SELECT [FirstBankCode]     
                                  ,[OneWeekReturnOriginalCurrency]
                                  ,[MonthtoDateReturnOriginalCurrency]
                                  ,[YeartoDateReturnOriginalCurrency]
                                  ,[OneMonthReturnOriginalCurrency]
                                  ,[ThreeMonthReturnOriginalCurrency]
                                  ,[SixMonthReturnOriginalCurrency]
                                  ,[OneYearReturnOriginalCurrency]
                                  ,[TwoYearReturnOriginalCurrency]
                                  ,[ThreeYearReturnOriginalCurrency]
                                  ,[FiveYearReturnOriginalCurrency]
                                  ,[DataDate]
                              FROM {Sysjust_Return_Fund} WITH (NOLOCK) where [FirstBankCode] = @fundId";
            var para = new { fundId };
            FundAccumulationRateOfReturn fundAccumulationRateOfReturn = DbManager.Custom.Execute<FundAccumulationRateOfReturn>(sql, para, System.Data.CommandType.Text);
            return fundAccumulationRateOfReturn;
        }

        /// <summary>
        /// 取得道瓊指數
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public List<FundDowJonesIndex> GetDowJonesIndex(string fundId)
        {
            string Sysjust_Return_Fund_3 = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Return_Fund_3);
            var sql = $@" SELECT TOP(12)
                          [FirstBankCode]
                          ,FORMAT(CAST([Date] as date),'yyyy/MM')  [Date]
                          ,[MonthlyReturnRate]
                          ,[IndicatorIndexPriceChange]
                          ,[MonthlyReturnRate] - [IndicatorIndexPriceChange] as Difference
                      FROM {Sysjust_Return_Fund_3} WITH (NOLOCK)
                      WHERE [FirstBankCode] = @fundId";
            var para = new { fundId };
            List<FundDowJonesIndex> fundDowJonesIndex = DbManager.Custom.ExecuteIList<FundDowJonesIndex>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();
            return fundDowJonesIndex;
        }

        /// <summary>
        /// 取得持有類股狀況-國內
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public List<FundStockHolding> GetFundHoldingStockPercent(string fundId)
        {
            string Sysjust_Holding_Fund_1 = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Holding_Fund_1);
            string sql = $@"SELECT  [FirstBankCode]
                                  ,Format([Date],'yyyy/MM/dd') Date
                                  ,[FundName]
                                  ,[StockName] Category
                                  ,[Shareholding] Holding
                              FROM {Sysjust_Holding_Fund_1} WITH (NOLOCK)
                              where [FirstBankCode] = @fundId and [StockName]!='合計'
                              order by [Shareholding] desc";
            var para = new { fundId };
            List<FundStockHolding> fundStockHolding = DbManager.Custom.ExecuteIList<FundStockHolding>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();
            return fundStockHolding;
        }

        /// <summary>
        /// 取得產業持股狀況
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public List<FundStockHolding> GetFundIndustryPercent(string fundId)
        {
            string Sysjust_Holding_Fund_3 = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Holding_Fund_3);
            string sql = $@"SELECT  [FirstBankCode]
                                  ,Format([Date],'yyyy/MM/dd') Date
                                  ,[FundName]
                                  ,[Sector] Category
                                  ,[HoldingSector] Holding
                                  ,[Currency]
                              FROM {Sysjust_Holding_Fund_3} WITH (NOLOCK)
                              where [FirstBankCode] = @fundId and Sector!='合計'
                              order by [HoldingSector] desc";
            var para = new { fundId };
            List<FundStockHolding> fundStockHolding = DbManager.Custom.ExecuteIList<FundStockHolding>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();
            return fundStockHolding;
        }
        /// <summary>
        /// 取得區域持股狀況
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public List<FundStockHolding> GetFundAreaPercent(string fundId)
        {
            string Sysjust_Holding_Fund_2 = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Holding_Fund_2);
            string sql = $@"SELECT [FirstBankCode]
                                  ,Format([Date],'yyyy/MM/dd') Date
                                  ,[FundName]
                                  ,[Area] Category
                                  ,[HoldingArea] Holding
                                  ,[Currency]
                              FROM {Sysjust_Holding_Fund_2} WITH (NOLOCK)
                              where [FirstBankCode] = @fundId and [Area]!='合計'
                              order by [HoldingArea] desc";
            var para = new { fundId };
            List<FundStockHolding> fundStockHolding = DbManager.Custom.ExecuteIList<FundStockHolding>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();
            return fundStockHolding;
        }

        /// <summary>
        /// 取得前十大持股
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public List<FundStockHolding> GetTopTenHoldingFund(string fundId, string domesticOroverseas)
        {
            string Sysjust_Holding_Fund_5 = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Holding_Fund_5);
            string Sysjust_Holding_Fund_4 = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Holding_Fund_4);
            string tableName = domesticOroverseas == nameof(FundEnum.D) ? Sysjust_Holding_Fund_5 : Sysjust_Holding_Fund_4;

            string sql = $@"SELECT TOP (10)
                                    Format([Date],'yyyy/MM/dd') Date
                                    ,[StockName] FundName
                                    ,[Shareholding] Holding
                                      FROM {tableName} WITH (NOLOCK)
                                     WHERE [FirstBankCode] = @fundId
                                     ORDER BY [Shareholding] DESC,[StockName]";
            var para = new { fundId };
            List<FundStockHolding> fundStockHolding = DbManager.Custom.ExecuteIList<FundStockHolding>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();
            return fundStockHolding;
        }

        /// <summary>
        /// 取得風險指標
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public FundRiskindicators GetRiskindicators(string fundId)
        {
            string Sysjust_Risk_Fund = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Risk_Fund);
            string sql = $@"SELECT  [FirstBankCode]
                      ,Format([Date],'yyyy/MM/dd') Date
                      ,[SixMonthStandardDeviation]
                      ,[OneYearStandardDeviation]
                      ,[ThreeYearStandardDeviation]
                      ,[FiveYearStandardDeviation]
                      ,[TenYearStandardDeviation]
                      ,[SixMonthSharpe]
                      ,[OneYearSharpe]
                      ,[ThreeYearSharpe]
                      ,[FiveYearSharpe]
                      ,[TenYearSharpe]
                      ,[SixMonthAlpha]
                      ,[OneYearAlpha]
                      ,[ThreeYearAlpha]
                      ,[FiveYearAlpha]
                      ,[TenYearAlpha]
                      ,[SixMonthBeta]
                      ,[OneYearBeta]
                      ,[ThreeYearBeta]
                      ,[FiveYearBeta]
                      ,[TenYearBeta]
                      ,[SixMonthRsquared]
                      ,[OneYearRsquared]
                      ,[ThreeYearRsquared]
                      ,[FiveYearRsquared]
                      ,[TenYearRsquared]
                      ,[SixMonthCorrelationCoefficientIndex]
                      ,[OneYearCorrelationCoefficientIndex]
                      ,[ThreeYearCorrelationCoefficientIndex]
                      ,[FiveYearCorrelationCoefficientIndex]
                      ,[TenYearCorrelationCoefficientIndex]
                      ,[SixMonthTrackingError]
                      ,[OneYearTrackingError]
                      ,[ThreeYearTrackingError]
                      ,[FiveYearTrackingError]
                      ,[TenYearTrackingError]
                      ,[SixMonthVariance]
                      ,[OneYearVariance]
                      ,[ThreeYearVariance]
                      ,[FiveYearVariance]
                      ,[TenYearVariance]
                  FROM {Sysjust_Risk_Fund} WITH (NOLOCK)
                WHERE  [FirstBankCode] = @fundId";
            var para = new { fundId };
            FundRiskindicators fundRiskindicators = DbManager.Custom.Execute<FundRiskindicators>(sql, para, System.Data.CommandType.Text);

            return fundRiskindicators;
        }

        /// <summary>
        /// 取得風險像線圖
        /// </summary>
        /// <param name="fundId"></param>
        /// <param name="select"></param>
        /// <returns></returns>
        public List<FundRiskGraph> GetRiskindicatorsGraph(string fundId, string selectType, string indicator)
        {
            var para = new { fundId, condition = selectType, indicator };
            List<FundRiskGraph> fundRiskGraphs = DbManager.Custom.ExecuteIList<FundRiskGraph>("sp_FundRiskindicatorsPicture", para, commandType: System.Data.CommandType.StoredProcedure)?.ToList();
            return fundRiskGraphs;
        }

        /// <summary>
        /// 取得報酬率比較
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public FundReturnCompare GetRateOfReturnCompare(string fundId, string domesticOroverseas)
        {
            string domestic = domesticOroverseas == nameof(FundEnum.D) ? "_Domestic" : string.Empty;
            var para = new { fundId };
            FundReturnCompare fundReturnCompare = DbManager.Custom.Execute<FundReturnCompare>("sp_FundReturnCompare" + domestic, para, System.Data.CommandType.StoredProcedure);
            return fundReturnCompare;
        }

        /// <summary>
        /// 取得年報酬率比較
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public List<FundYearRateOfReturn> GetYearRateOfReturnCompare(string fundId)
        {
            string Sysjust_Risk_Fund_3 = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Risk_Fund_3);
            var sql = $@"SELECT TOP(5)
                          [FirstBankCode]
                          ,[Date]
                          ,[AnnualizedStandardDeviation]
                          ,[Beta]
                          ,[Sharpe]
                          ,[InformationRatio]
                          ,[JensenIndex]
                          ,[TreynorIndex]
                      FROM {Sysjust_Risk_Fund_3} WITH (NOLOCK)
                      where [FirstBankCode]=@fundId ORDER BY [Date] DESC";
            var para = new { fundId };
            List<FundYearRateOfReturn> fundYearRateOfReturn = DbManager.Custom.ExecuteIList<FundYearRateOfReturn>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();
            return fundYearRateOfReturn;
        }

        /// <summary>
        /// 取得配息紀錄
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public List<FundDividendRecord> GetDividendRecord(string fundId)
        {
            string Sysjust_Dividend_Fund = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Dividend_Fund);
            var sql = $@" SELECT [FirstBankCode]
                              ,FORMAT([ExDividendDate],'yyyy/MM/dd') [ExDividendDate]
                              ,FORMAT([BaseDate],'yyyy/MM/dd')[BaseDate]
                              ,FORMAT([ReleaseDate],'yyyy/MM/dd')[ReleaseDate]
                              ,[Dividend]
                              ,[AnnualizedDividendRate]
                              ,[Currency]
                          FROM {Sysjust_Dividend_Fund} WITH (NOLOCK) WHERE [FirstBankCode]=@fundId
                          ORDER BY [ExDividendDate] DESC";
            var para = new { fundId };
            List<FundDividendRecord> fundDividendRecord = DbManager.Custom.ExecuteIList<FundDividendRecord>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();

            return fundDividendRecord;
        }

        /// <summary>
        /// 取得規模變動
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public List<FundScaleRecord> GetScaleMove(string fundId, string domesticOroverseas = nameof(FundEnum.D))
        {
            string Sysjust_Fundsize_Fund_2 = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Fundsize_Fund_2);
            string Sysjust_Fundsize_Fund_1 = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Fundsize_Fund_1);
            string tableName = domesticOroverseas == nameof(FundEnum.D) ? Sysjust_Fundsize_Fund_2 : Sysjust_Fundsize_Fund_1;
            var sql = $@" ;WITH CTE AS (
                             SELECT * FROM {tableName} WITH (NOLOCK)
                             WHERE [FirstBankCode]=@fundId
                            )
                            SELECT [FirstBankCode]
                                ,FORMAT([ScaleDate],'yyyy/MM/dd') [ScaleDate]
                                 , CONVERT(MONEY, [Scale]/1000) [Scale]
                                 ,[Currency]
                              FROM CTE
                              WHERE [ScaleDate] >= CAST(CAST(YEAR(DATEADD(YEAR,-2,(SELECT MAX([ScaleDate]) FROM CTE))) AS varchar(4))+'-01-01' AS smalldatetime)
                              ORDER BY [ScaleDate] DESC";
            var para = new { fundId };
            List<FundScaleRecord> fundScaleMove = DbManager.Custom.ExecuteIList<FundScaleRecord>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();
            return fundScaleMove;
        }

        public List<FundBase> GetSameCompanyFunds(string fundId, string companyId)
        {
            string FUND_BSC = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.FUND_BSC);
            string WMS_DOC_RECM = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.WMS_DOC_RECM);
            var sameCompanyFunds = new List<FundBase>();
            var sql = $@" 
                            SELECT A.BankProductCode as [ProductCode],A.FundName as [ProductName] FROM {FUND_BSC} AS A
                            inner join {WMS_DOC_RECM} as B on A.BankProductCode = B.ProductCode
                            where  FundCompanyID =@companyId
                            and BankProductCode is not null and BankProductCode != @fundId ORDER BY BankProductCode";
            var para = new { companyId, fundId };
            sameCompanyFunds = DbManager.Custom.ExecuteIList<FundBase>(sql, para, commandType: System.Data.CommandType.Text)
                ?.ToList();
            return sameCompanyFunds;
        }

        public string GetIndicatorIndexCode(string fundId, string indicator)
        {
            string Sysjust_Basic_Fund = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Basic_Fund);
            string Sysjust_Basic_Fund_2 = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_Basic_Fund_2);
            string tableName = indicator == nameof(FundEnum.D) ? Sysjust_Basic_Fund : Sysjust_Basic_Fund_2;
            string indexCode = string.Empty;
            var sql = $@" SELECT IndicatorIndexCode FROM {tableName} WHERE FirstBankCode=@fundId";
            var para = new { fundId };
            indexCode = DbManager.Custom.Execute<string>(sql, para, commandType: System.Data.CommandType.Text);
            return indexCode;
        }

        public FundViewModel GetDocLinks(string fundId, FundViewModel fundViewModel, string fundIndicator, DjMoneyApiRespository djMoneyApiRespository)
        {
            //嘗試撈取暫存
            FundViewModel temp = (FundViewModel)this._cache.Get(this.FundDetailTempCacheKey + "_" + fundId) ?? null;

            if (temp != null)
            {
                fundViewModel.OpenDoc = temp.OpenDoc;
                fundViewModel.FinancialReportDoc = temp.FinancialReportDoc;
                fundViewModel.EasyOpenDoc = temp.EasyOpenDoc;
                fundViewModel.MonthReportDoc = temp.MonthReportDoc;
                fundViewModel.InvestExclusiveDoc = temp.InvestExclusiveDoc;
                fundViewModel.InvestNomnalDoc = temp.InvestNomnalDoc;
                fundViewModel.BackLevelFee = temp.BackLevelFee;
            }
            else
            {
                if (fundIndicator == nameof(FundEnum.D))
                {
                    fundViewModel.OpenDoc = GetDocLink(djMoneyApiRespository, fundId, "1");
                    fundViewModel.FinancialReportDoc = GetDocLink(djMoneyApiRespository, fundId, "2");
                    fundViewModel.EasyOpenDoc = GetDocLink(djMoneyApiRespository, fundId, "4");
                    fundViewModel.MonthReportDoc = GetDocLink(djMoneyApiRespository, fundId, "5");
                }
                else
                {
                    fundViewModel.OpenDoc = GetDocLink(djMoneyApiRespository, fundId, "1");
                    fundViewModel.FinancialReportDoc = GetDocLink(djMoneyApiRespository, fundId, "2");
                    fundViewModel.MonthReportDoc = GetDocLink(djMoneyApiRespository, fundId, "5");
                    fundViewModel.InvestExclusiveDoc = GetDocLink(djMoneyApiRespository, fundId, "6");
                    fundViewModel.InvestNomnalDoc = GetDocLink(djMoneyApiRespository, fundId, "7");
                    //境外基金，為後收型別才需取api
                    if (!string.IsNullOrEmpty(fundViewModel.FeePostCollectionType) && fundViewModel.FeePostCollectionType.ToBoolean())
                    {
                        fundViewModel.BackLevelFee = GetDocLink(djMoneyApiRespository, fundId, "8");
                    }
                }

                temp = new FundViewModel
                {
                    OpenDoc = fundViewModel.OpenDoc,
                    FinancialReportDoc = fundViewModel.FinancialReportDoc,
                    EasyOpenDoc = fundViewModel.EasyOpenDoc,
                    MonthReportDoc = fundViewModel.MonthReportDoc,
                    InvestExclusiveDoc = fundViewModel.InvestExclusiveDoc,
                    InvestNomnalDoc = fundViewModel.InvestNomnalDoc,
                    BackLevelFee = fundViewModel.BackLevelFee
                };

                this._cache.Set(this.FundDetailTempCacheKey + "_" + fundId, temp, DateTimeOffset.Now.AddMinutes(600));
            }

            return fundViewModel;
        }

        public string GetDocLink(DjMoneyApiRespository djMoneyApiRespository, string fundId, string idx)
        {
            var resp = djMoneyApiRespository.GetDocLink2(fundId.ToUpper(), idx);

            if (resp != null
                && resp.ContainsKey("resultSet")
                && resp["resultSet"] != null
                && resp["resultSet"]["result"] != null
                && resp["resultSet"]["result"].Any()
                && resp["resultSet"]["result"][0]["v1"] != null)
            {
                return resp["resultSet"]["result"][0]["v1"].ToString();
            }

            return string.Empty;
        }
    }

}
