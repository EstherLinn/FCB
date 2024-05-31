using Feature.Wealth.Component.Models.FundDetail;
using Foundation.Wealth.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Feature.Wealth.Component.Repositories
{
    public class FundRepository
    {
        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly string FundDetailsCacheKey = $"Fcb_FundDetailsCache";

        public FundViewModel GetOrSetFundDetailsCache(string fundId, string indicator)
        {
          var FundDic = (Dictionary<string, FundViewModel>)_cache.Get(FundDetailsCacheKey) ?? new Dictionary<string, FundViewModel>();
            FundViewModel FundFullData = null;
            if (!FundDic.Any())
            {
                FundDic.Add(fundId,CreateFundDetailsData(fundId, indicator));
                _cache.Set(FundDetailsCacheKey, FundDic, DateTimeOffset.Now.AddMinutes(60));
            }
            else
            {
                if (FundDic.TryGetValue(fundId,out FundFullData))
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
                fundViewModel.FundBaseData = this.GetDomesticFundBasic(fundId);
                fundViewModel.FundAccordingStockHoldings = this.GetFundHoldingStockPercent(fundId);
            }
            else
            {
                fundViewModel.FundBaseData = this.GetOverseasFundBasic(fundId);
            }

            fundViewModel.TagsDic = this.GetTagsById(fundId);
            fundViewModel.FundCloseYearsNetValue = this.GetNetAssetValueWithCloseYear(fundId);
            fundViewModel.FundTypeRanks = this.GetSameTypeFundRank(fundId);
            fundViewModel.FundRateOfReturn = this.GetRateOfReturn(fundId);
            fundViewModel.FundThiryDaysNetValue = this.GetThrityDaysNetValue(fundId);
            fundViewModel.FundAnnunalRateOfReturn = this.GetAnnualRateOfReturn(fundId);
            fundViewModel.FundAccumulationRateOfReturn = this.GetAccumulationRateOfReturn(fundId);
            fundViewModel.FundDowJonesIndexs = this.GetDowJonesIndex(fundId);
            fundViewModel.FundStockHoldings = this.GetFundIndustryPercent(fundId);
            fundViewModel.FundStockAreaHoldings = this.GetFundAreaPercent(fundId);
            fundViewModel.FundTopTenStockHolding = this.GetTopTenHoldingFund(fundId, indicator);
            fundViewModel.FundRiskindicators = this.GetRiskindicators(fundId);
            fundViewModel.FundReturnCompare = this.GetRateOfReturnCompare(fundId, indicator);
            fundViewModel.FundYearRateOfReturn = this.GetYearRateOfReturnCompare(fundId);
            fundViewModel.FundDividendRecords = this.GetDividendRecord(fundId);
            fundViewModel.FundScaleRecords = this.GetScaleMove(fundId, indicator);
            return fundViewModel;
        }
        /// <summary>
        /// 取得國內外註記
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public string GetDometicOrOverseas(string fundId)
        {
            string indicator = string.Empty;
            string sql = @"Select DomesticForeignFundIndicator From [FUND_BSC] (NOLOCK) Where  [BankProductCode] =@fundId";
            var para = new { fundId = fundId };
            indicator = DbManager.Custom.Execute<string>(sql, para, commandType: System.Data.CommandType.Text);
            return indicator;
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
        public DomesticFundBase GetDomesticFundBasic(string fundId)
        {
            DomesticFundBase domesticFundBase = null;
            var para = new { fundId = fundId };
            domesticFundBase = DbManager.Custom.Execute<DomesticFundBase>("sp_DomesticFundBasicData", para, commandType: System.Data.CommandType.StoredProcedure);
            return domesticFundBase;
        }

        /// <summary>
        /// 取得海外基金基本資料
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public OverseasFundBase GetOverseasFundBasic(string fundId)
        {
            OverseasFundBase overseasFundBase = null; 
            var para = new { fundId = fundId };
            overseasFundBase = DbManager.Custom.Execute<OverseasFundBase>("sp_OverseasFundBasicData", para, commandType: System.Data.CommandType.StoredProcedure);
            return overseasFundBase;
        }


        /// <summary>
        /// 取得最近一年淨值
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public List<FundCloseYearNetValue> GetNetAssetValueWithCloseYear(string fundId)
        {
            List<FundCloseYearNetValue> fundCloseYearNetValue = new List<FundCloseYearNetValue>();
            string sql = @"SELECT Format([Date],'yyyy-MM-dd') NetAssetValueDate,[NetAssetValue] 
                            FROM [Sysjust_FUNDNAV_HIS] where [FirstBankCode]=@fundid and Date >= DATEADD(year, -1, GETDATE()) ORDER BY [Date] ";
            var para = new { fundid = fundId };
            fundCloseYearNetValue = DbManager.Custom.ExecuteIList<FundCloseYearNetValue>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();
            return fundCloseYearNetValue;
        }
        /// <summary>
        /// 取得同類型基金排行
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public List<FundTypeRank> GetSameTypeFundRank(string fundId)
        {
            List<FundTypeRank> fundTypeRank = new List<FundTypeRank>();
            var para = new { fundid = fundId };
            fundTypeRank = DbManager.Custom.ExecuteIList<FundTypeRank>("sp_FundSameTypeRank", para, commandType: System.Data.CommandType.StoredProcedure)?.ToList();
            return fundTypeRank;
        }
        /// <summary>
        /// 取得近30日淨值
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>

        public List<FundThiryDays> GetThrityDaysNetValue(string fundId)
        {
            List<FundThiryDays> fundThiryDays = new List<FundThiryDays>();
           
            var para = new { fundid = fundId };
            fundThiryDays = DbManager.Custom.ExecuteIList<FundThiryDays>("sp_CountFundNavForThirtyDays", para, commandType: System.Data.CommandType.StoredProcedure)?.ToList();
            return fundThiryDays;
        }
        /// <summary>
        /// 取得報酬率
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public FundRateOfReturn GetRateOfReturn(string fundId)
        {
            FundRateOfReturn fundRateOfReturn = null;
            var para = new { fundId = fundId };
            fundRateOfReturn = DbManager.Custom.Execute<FundRateOfReturn>("sp_FundRateOfReturn", para, commandType: System.Data.CommandType.StoredProcedure);
            return fundRateOfReturn;
        }
        /// <summary>
        /// 取得近五年報酬率
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>

        public List<FundAnnunalRateOfReturn> GetAnnualRateOfReturn(string fundId)
        {
            List<FundAnnunalRateOfReturn> fundAnnunalRateOfReturn = new List<FundAnnunalRateOfReturn>();
            var sql = @"SELECT TOP(5)
                       [FirstBankCode]
                      ,[Year]
                      ,[AnnualReturnRateOriginalCurrency]
                      ,[AnnualReturnRateTWD]
                      ,[IndicatorIndexPriceChange]
                      ,[AnnualReturnRateOriginalCurrency] - [IndicatorIndexPriceChange] as Difference
                  FROM [Sysjust_Return_Fund_2] 
                  where [FirstBankCode] = @fundId 
                  ORDER BY [Year] DESC";
            var para = new { fundid = fundId };
            fundAnnunalRateOfReturn = DbManager.Custom.ExecuteIList<FundAnnunalRateOfReturn>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();
            return fundAnnunalRateOfReturn;
        }
        /// <summary>
        /// 取得累積報酬率
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public FundAccumulationRateOfReturn GetAccumulationRateOfReturn(string fundId)
        {
            FundAccumulationRateOfReturn fundAccumulationRateOfReturn = null;
            string sql = @"SELECT [FirstBankCode]     
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
                              FROM [Sysjust_Return_Fund] where [FirstBankCode] = @fundid";
            var para = new { fundid = fundId };
            fundAccumulationRateOfReturn = DbManager.Custom.Execute<FundAccumulationRateOfReturn>(sql, para,System.Data.CommandType.Text);
            return fundAccumulationRateOfReturn;
        }

        /// <summary>
        /// 取得道瓊指數
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public List<FundDowJonesIndex> GetDowJonesIndex(string fundId)
        {
            List<FundDowJonesIndex> fundDowJonesIndex = new List<FundDowJonesIndex>();
            var sql = @" SELECT TOP(12)
                          [FirstBankCode]
                          ,FORMAT(CAST([Date] as date),'yyyy/MM')  [Date]
                          ,[MonthlyReturnRate]
                          ,[IndicatorIndexPriceChange]
                          ,[MonthlyReturnRate] - [IndicatorIndexPriceChange] as Difference
                      FROM [Sysjust_Return_Fund_3]
                      WHERE [FirstBankCode] = @fundid";
            var para = new { fundid = fundId };
            fundDowJonesIndex = DbManager.Custom.ExecuteIList<FundDowJonesIndex>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();
            return fundDowJonesIndex;
        }

        /// <summary>
        /// 取得持有類股狀況-國內
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public List<FundStockHolding> GetFundHoldingStockPercent(string fundId)
        {
            List<FundStockHolding> fundStockHolding = new List<FundStockHolding>();
            string sql = @"SELECT  [FirstBankCode]
                                  ,Format([Date],'yyyy/MM/dd') Date
                                  ,[FundName]
                                  ,[StockName] Category
                                  ,[Shareholding] Holding
                              FROM [Sysjust_Holding_Fund_1]
                              where [FirstBankCode] = @fundid and [StockName]!='合計'
                              order by [Shareholding] desc";
            var para = new { fundid = fundId };
            fundStockHolding = DbManager.Custom.ExecuteIList<FundStockHolding>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();
            return fundStockHolding;
        }

        /// <summary>
        /// 取得產業持股狀況
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public List<FundStockHolding> GetFundIndustryPercent(string fundId)
        {
            List<FundStockHolding> fundStockHolding = new List<FundStockHolding>();
            string sql = @"SELECT  [FirstBankCode]
                                  ,Format([Date],'yyyy/MM/dd') Date
                                  ,[FundName]
                                  ,[Sector] Category
                                  ,[HoldingSector] Holding
                                  ,[Currency]
                              FROM [Sysjust_Holding_Fund_3]
                              where [FirstBankCode] = @fundid and Sector!='合計'
                              order by [HoldingSector] desc";
            var para = new { fundid = fundId };
            fundStockHolding = DbManager.Custom.ExecuteIList<FundStockHolding>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();
            return fundStockHolding;
        }
        /// <summary>
        /// 取得區域持股狀況
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public List<FundStockHolding> GetFundAreaPercent(string fundId)
        {
            List<FundStockHolding> fundStockHolding = new List<FundStockHolding>();
            string sql = $@"SELECT [FirstBankCode]
                                  ,Format([Date],'yyyy/MM/dd') Date
                                  ,[FundName]
                                  ,[Area] Category
                                  ,[HoldingArea] Holding
                                  ,[Currency]
                              FROM [Sysjust_Holding_Fund_2]
                              where [FirstBankCode] = @fundid and [Area]!='合計'
                              order by [HoldingArea] desc";
            var para = new { fundid = fundId };
            fundStockHolding = DbManager.Custom.ExecuteIList<FundStockHolding>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();
            return fundStockHolding;
        }

        /// <summary>
        /// 取得前十大持股
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public  List<FundStockHolding> GetTopTenHoldingFund(string fundId , string domesticOroverseas)
        {
            string tableName = domesticOroverseas == nameof(FundEnum.D) ? "[Sysjust_Holding_Fund_5]" : "[Sysjust_Holding_Fund_4]";

            List <FundStockHolding> fundStockHolding = new List<FundStockHolding>();
            string sql = $@"SELECT TOP (10)
                                    Format([Date],'yyyy/MM/dd') Date
                                    ,[StockName] FundName
                                    ,[Shareholding] Holding
                                      FROM {tableName}
                                     WHERE [FirstBankCode] = @fundid
                                     ORDER BY [Shareholding] DESC";
            var para = new { fundid = fundId };
            fundStockHolding = DbManager.Custom.ExecuteIList<FundStockHolding>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();
            return fundStockHolding;
        }

        /// <summary>
        /// 取得風險指標
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public FundRiskindicators GetRiskindicators(string fundId)
        {
            FundRiskindicators fundRiskindicators = null;
            string sql = @"SELECT  [FirstBankCode]
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
                  FROM [Sysjust_Risk_Fund]
                WHERE  [FirstBankCode] = @fundid";
            var para = new { fundid = fundId };
            fundRiskindicators = DbManager.Custom.Execute<FundRiskindicators>(sql, para, System.Data.CommandType.Text);

            return fundRiskindicators;
        }

        /// <summary>
        /// 取得風險像線圖
        /// </summary>
        /// <param name="fundId"></param>
        /// <param name="select"></param>
        /// <returns></returns>
        public List<FundRiskGraph> GetRiskindicatorsGraph(string fundId,string selectType)
        {

            List<FundRiskGraph> fundRiskGraphs = new List<FundRiskGraph>();
            var para = new { fundId = fundId, condition = selectType };
            fundRiskGraphs = DbManager.Custom.ExecuteIList<FundRiskGraph>("sp_FundRiskindicatorsPicture", para, commandType: System.Data.CommandType.StoredProcedure)?.ToList();
            return fundRiskGraphs;
        }

        /// <summary>
        /// 取得報酬率比較
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public FundReturnCompare GetRateOfReturnCompare(string fundId,string domesticOroverseas)
        {
            string domestic = domesticOroverseas == nameof(FundEnum.D) ? "_Domestic" : string.Empty;
            FundReturnCompare fundReturnCompare = null;
            var para = new { fundid = fundId};
            fundReturnCompare = DbManager.Custom.Execute<FundReturnCompare>("sp_FundReturnCompare"+ domestic, para, System.Data.CommandType.StoredProcedure);
            return fundReturnCompare;
        }

        /// <summary>
        /// 取得年報酬率比較
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public List<FundYearRateOfReturn> GetYearRateOfReturnCompare(string fundId)
        {
            List<FundYearRateOfReturn> fundYearRateOfReturn = new List<FundYearRateOfReturn>();
            var sql = @"SELECT TOP(5)
                          [FirstBankCode]
                          ,[Date]
                          ,[AnnualizedStandardDeviation]
                          ,[Beta]
                          ,[Sharpe]
                          ,[InformationRatio]
                          ,[JensenIndex]
                          ,[TreynorIndex]
                      FROM [Sysjust_Risk_Fund_3] 
                      where [FirstBankCode]=@fundid ORDER BY [Date] DESC";
            var para = new { fundid = fundId };
            fundYearRateOfReturn = DbManager.Custom.ExecuteIList<FundYearRateOfReturn>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();
            return fundYearRateOfReturn;
        }

        /// <summary>
        /// 取得配息紀錄
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public List<FundDividendRecord> GetDividendRecord(string fundId)
        {
            List<FundDividendRecord> fundDividendRecord = new List<FundDividendRecord>();
            var sql = @";WITH CTE AS (
                         SELECT * FROM [Sysjust_Dividend_Fund] (NOLOCK)
                         WHERE [FirstBankCode]=@fundid
                        )
                        SELECT [FirstBankCode]
                              ,FORMAT([ExDividendDate],'yyyy/MM/dd') [ExDividendDate]
                              ,FORMAT([BaseDate],'yyyy/MM/dd')[BaseDate]
                              ,FORMAT([ReleaseDate],'yyyy/MM/dd')[ReleaseDate]
                              ,[Dividend]
                              ,[AnnualizedDividendRate]
                              ,[Currency]
                          FROM CTE
                          WHERE ExDividendDate >= CAST(CAST(YEAR(DATEADD(YEAR,-2,(SELECT MAX(ExDividendDate) FROM CTE))) AS varchar(4))+'-01-01' AS smalldatetime)
                          ORDER BY [ExDividendDate] DESC";
            var para = new { fundid = fundId };
            fundDividendRecord = DbManager.Custom.ExecuteIList<FundDividendRecord>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();

            return fundDividendRecord;
        }

        /// <summary>
        /// 取得規模變動
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public List<FundScaleRecord> GetScaleMove(string fundId, string domesticOroverseas = nameof(FundEnum.D))
        {
            string tableName = domesticOroverseas == nameof(FundEnum.D) ? "[Sysjust_Fundsize_Fund_2]" : "[Sysjust_Fundsize_Fund_1]";
            List<FundScaleRecord> fundScaleMove = new List<FundScaleRecord>();
            var sql = $@" ;WITH CTE AS (
                             SELECT * FROM {tableName} (NOLOCK)
                             WHERE [FirstBankCode]=@fundid
                            )
                            SELECT [FirstBankCode]
                                ,FORMAT([ScaleDate],'yyyy/MM/dd') [ScaleDate]
                                 , CONVERT(MONEY, [Scale]/1000) [Scale]
                                 ,[Currency]
                              FROM CTE
                              WHERE [ScaleDate] >= CAST(CAST(YEAR(DATEADD(YEAR,-2,(SELECT MAX([ScaleDate]) FROM CTE))) AS varchar(4))+'-01-01' AS smalldatetime)
                              ORDER BY [ScaleDate] DESC";
            var para = new { fundid = fundId };
            fundScaleMove = DbManager.Custom.ExecuteIList<FundScaleRecord>(sql, para, commandType: System.Data.CommandType.Text)?.ToList();
            return fundScaleMove;
        }


    }

}
