using Dapper;
using Feature.Wealth.Component.Models.GlobalIndex;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Manager;
using Foundation.Wealth.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Caching;

namespace Feature.Wealth.Component.Repositories
{
    public class GlobalIndexRepository
    {
        private readonly IDbConnection _dbConnection = DbManager.Custom.DbConnection();
        private readonly DjMoneyApiRespository _djMoneyApiRespository = new DjMoneyApiRespository();

        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly string GlobalIndexHistoryCacheKey = $"Fcb_GlobalIndexHistoryCache";

        public IList<GlobalIndex> GetGlobalIndexList()
        {
            string Sysjust_GlobalIndex = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_GlobalIndex);
            string sql = $@"SELECT
                           [IndexCode]
                           ,[IndexName]
                           ,[IndexCategoryID]
                           ,[IndexCategoryName]
                           ,REPLACE(CONVERT(char(10), [DataDate], 126), '-', '/') [DataDate]
                           ,[MarketPrice]
                           ,CONVERT(nvarchar, CONVERT(MONEY, [MarketPrice]), 1) [MarketPriceText]
                           ,CONVERT(nvarchar, CONVERT(MONEY, [Change]), 1) [Change]
                           ,CONVERT(nvarchar, CONVERT(decimal(16,2), [ChangePercentage])) + '%' [ChangePercentage]
                           ,CONVERT(bit, IIF([Change] >= 0, 1, 0)) [UpOrDown]
                           FROM {Sysjust_GlobalIndex} WITH (NOLOCK)
                           ORDER BY [IndexCode]";

            var globalIndexs = this._dbConnection.Query<GlobalIndex>(sql)?.ToList() ?? new List<GlobalIndex>();

            return globalIndexs;
        }

        public IList<GlobalIndex> GetCommonGlobalIndexList()
        {
            string Sysjust_GlobalIndex = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_GlobalIndex);
            string sql = $@"SELECT
                           [IndexCode]
                           ,[IndexName]
                           ,[IndexCategoryID]
                           ,[IndexCategoryName]
                           ,REPLACE(CONVERT(char(10), [DataDate], 126), '-', '/') [DataDate]
                           ,[MarketPrice]
                           ,CONVERT(nvarchar, CONVERT(MONEY, [MarketPrice]), 1) [MarketPriceText]
                           ,CONVERT(nvarchar, CONVERT(MONEY, [Change]), 1) [Change]
                           ,CONVERT(nvarchar, CONVERT(decimal(16,2), [ChangePercentage])) + '%' [ChangePercentage]
                           ,CONVERT(bit, IIF([Change] >= 0, 1, 0)) [UpOrDown]
                           FROM {Sysjust_GlobalIndex} WITH (NOLOCK)
                           WHERE [IndexCode] IN
                           (
                           'EB09999','AI000040','AI000041','AI000220',
                           'AI000545','AI000030','AI000410','AI000070',
                           'AI000800','AI000010','AI000020','AI000140',
                           'AI000280','AI000360','AI000170','AI000515'
                           )
                           ORDER BY [IndexCode]";

            var globalIndexs = this._dbConnection.Query<GlobalIndex>(sql)?.ToList() ?? new List<GlobalIndex>();

            return globalIndexs;
        }

        public List<float> GetGlobalIndexHistoryList(string indexCode)
        {
            var result = (List<float>)this._cache.Get(this.GlobalIndexHistoryCacheKey + "_" + indexCode) ?? new List<float>();

            if (result.Any())
            {
                return result;
            }
            else
            {
                try
                {
                    var priceDatas = GetGlobalInedxPriceData(indexCode, "D");

                    priceDatas = priceDatas.OrderByDescending(c => c.date).ToList();

                    foreach (var priceData in priceDatas)
                    {
                        if (result.Count == 30)
                        {
                            break;
                        }

                        result.Insert(0, (float)priceData.close);
                    }
                }
                catch (Exception ex)
                {

                }

                this._cache.Set(this.GlobalIndexHistoryCacheKey + "_" + indexCode, result, DateTimeOffset.Now.AddMinutes(600));
            }

            return result;
        }

        public GlobalIndexDetail GetGlobalIndexDetail(string indexCode)
        {
            string Sysjust_GlobalIndex = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_GlobalIndex);
            string Sysjust_GlobalIndex_ROI = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.Sysjust_GlobalIndex_ROI);
            string sql = $@"SELECT TOP 1
                           A.[IndexCode]
                           ,A.[IndexName]
                           ,A.[IndexCategoryID]
                           ,A.[IndexCategoryName]
                           ,REPLACE(CONVERT(char(10), A.[DataDate],126), '-', '/') [DataDate]
                           ,A.[MarketPrice]
                           ,CONVERT(nvarchar, CONVERT(MONEY, A.[MarketPrice]), 1) [MarketPriceText]
                           ,CONVERT(nvarchar, CONVERT(MONEY, A.[Change]), 1) [Change]
                           ,CONVERT(nvarchar, CONVERT(decimal(16,2), A.[ChangePercentage])) + '%' [ChangePercentage]
                           ,CONVERT(bit, IIF(A.[Change] >= 0, 1, 0)) [UpOrDown]
                           ,CONVERT(nvarchar, CONVERT(decimal(16,2), C.[DailyReturn])) + '%' [DailyReturn]
                           ,CONVERT(nvarchar, CONVERT(decimal(16,2), C.[WeeklyReturn])) + '%' [WeeklyReturn]
                           ,CONVERT(nvarchar, CONVERT(decimal(16,2), C.[OneMonthReturn])) + '%' [OneMonthReturn]
                           ,CONVERT(nvarchar, CONVERT(decimal(16,2), C.[ThreeMonthReturn])) + '%' [ThreeMonthReturn]
                           ,CONVERT(nvarchar, CONVERT(decimal(16,2), C.[SixMonthReturn])) + '%' [SixMonthReturn]
                           ,CONVERT(nvarchar, CONVERT(decimal(16,2), C.[YeartoDateReturn])) + '%' [YeartoDateReturn]
                           ,CONVERT(nvarchar, CONVERT(decimal(16,2), C.[OneYearReturn])) + '%' [OneYearReturn]
                           ,CONVERT(nvarchar, CONVERT(decimal(16,2), C.[ThreeYearReturn])) + '%' [ThreeYearReturn]
                           FROM {Sysjust_GlobalIndex} A WITH (NOLOCK)
                           LEFT JOIN {Sysjust_GlobalIndex_ROI} C WITH (NOLOCK) ON A.[IndexCode] = C.[IndexID]
                           WHERE A.[IndexCode] = @IndexCode
                           ORDER BY C.[DataDate] DESC";

            var globalIndexDetail = this._dbConnection.Query<GlobalIndexDetail>(sql, new { IndexCode = indexCode })?.FirstOrDefault() ?? new GlobalIndexDetail();

            return globalIndexDetail;
        }

        public decimal? Round2(decimal? value)
        {
            if (value != null)
            {
                value = decimal.Round((decimal)value, 2, MidpointRounding.AwayFromZero);
            }
            return value;
        }

        public double GetDoubleOrZero(string value)
        {
            if (double.TryParse(value, out double reslut))
            {
                return reslut;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 取得指數K線資料
        /// </summary>
        /// <param name="indexCode">指數代碼</param>
        /// <param name="cycle">D、W、M</param>
        /// <returns></returns>
        public List<PriceData> GetGlobalInedxPriceData(string indexCode, string cycle)
        {
            var priceDatas = new List<PriceData>();

            var resp = this._djMoneyApiRespository.GetGlobalInedxPriceData(indexCode, cycle);

            if (resp != null
                && resp.ContainsKey("resultSet")
                && resp["resultSet"] != null
                && resp["resultSet"]["result"] != null
                && resp["resultSet"]["result"].Any())
            {
                var data = resp["resultSet"]["result"][0];

                var dates = data["v1"]?.ToString().Split(',').ToList() ?? new List<string>();
                var opens = data["v2"]?.ToString().Split(',').ToList() ?? new List<string>();
                var highs = data["v3"]?.ToString().Split(',').ToList() ?? new List<string>();
                var lows = data["v4"]?.ToString().Split(',').ToList() ?? new List<string>();
                var closes = data["v5"]?.ToString().Split(',').ToList() ?? new List<string>();
                var values = data["v6"]?.ToString().Split(',').ToList() ?? new List<string>();

                int count = dates.Count;

                for (int i = 0; i < count; i++)
                {
                    var priceData = new PriceData();

                    if (DateTime.TryParse(dates[i], out var date))
                    {
                        // 轉成 javascript 時間給 highcharts 用
                        priceData.date = date.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                    }
                    else
                    {
                        // 沒日期跳下一筆
                        continue;
                    }

                    priceData.open = GetDoubleOrZero(opens.ElementAtOrDefault(i));
                    priceData.high = GetDoubleOrZero(highs.ElementAtOrDefault(i));
                    priceData.low = GetDoubleOrZero(lows.ElementAtOrDefault(i));
                    priceData.close = GetDoubleOrZero(closes.ElementAtOrDefault(i));
                    priceData.value = GetDoubleOrZero(values.ElementAtOrDefault(i));

                    priceDatas.Add(priceData);
                }
            }

            return priceDatas;
        }
    }
}