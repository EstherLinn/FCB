using Dapper;
using Feature.Wealth.Component.Models.FundDetail;
using Feature.Wealth.Component.Repositories;
using Flurl;
using Flurl.Http;
using Foundation.Wealth.Extensions;
using Foundation.Wealth.Manager;
using Newtonsoft.Json.Linq;
using Sitecore.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.Logging;
using static Feature.Wealth.Service.Models.FundRankApi.FundRankApiModel;

namespace Feature.Wealth.Service.Repositories
{
    public class FundRankApiRepository
    {
        private readonly VisitCountRepository _repository = new VisitCountRepository();
        private readonly MemoryCache _cache = MemoryCache.Default;
        private static readonly object _lock = new object();
        private readonly string _route = Settings.GetSetting("MoneyDjApiRoute");
        private readonly string _token = Settings.GetSetting("MoneyDjToken");
        private readonly string _genWebUrlString = Settings.GetSetting("WebSubscriptionSingle");
        private readonly string _domainUrlString = Settings.GetSetting("JwtIssuer");
        private readonly string AwardFundApiCacheKey = $"Fcb_AwardFundApiCache";

        // 取得所有基金資料
        public IList<Funds> GetOrSetFundDataCache(string data)
        {
            lock (_lock)
            {
                string returnColumn = data switch
                {
                    "1" => "OneMonthReturnOriginalCurrency",
                    "3" => "ThreeMonthReturnOriginalCurrency",
                    "6" => "SixMonthReturnOriginalCurrency",
                    "12" => "OneYearReturnOriginalCurrency",
                    _ => "SixMonthReturnOriginalCurrency"
                };
                string fundDataCacheKey = $"Fcb_FundDataApiCache_{returnColumn}";
                var results = (IList<Funds>)_cache.Get(fundDataCacheKey);
                if (results == null || !results.Any())
                {
                    string sql = $"""
                             SELECT *
                             FROM [vw_BasicFund]
                             ORDER BY {returnColumn} DESC, ProductCode
                             """;

                    results = DbManager.Custom.ExecuteIList<Funds>(sql, null, CommandType.Text);
                    if (results != null && results.Any())
                    {
                        _cache.Set(fundDataCacheKey, results, DateTimeOffset.Now.AddMinutes(30));
                    }
                }
                return results;
            }
        }

        // 根據產品代碼取得基金資料
        public IList<Funds> GetFundDataByProductCode(string code)
        {
            lock (_lock)
            {
                string fundDataByProducCodeCacheKey = $"Fcb_fundDataByProducCodeCacheKey_{code}";

                var results = (IList<Funds>)_cache.Get(fundDataByProducCodeCacheKey);

                if (results == null || !results.Any())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@ProductCode", code, DbType.String, ParameterDirection.Input, code.Length);

                    string sql = """
                    SELECT *
                    FROM [vw_BasicFund]
                    WHERE ProductCode = @ProductCode
                    """;

                    results = DbManager.Custom.ExecuteIList<Funds>(sql, parameters, CommandType.Text);
                    if (results != null && results.Any())
                    {
                        _cache.Set(fundDataByProducCodeCacheKey, results, DateTimeOffset.Now.AddMinutes(30));
                    }
                }
                return results;
            }
        }

        // 取得人氣基金資料
        public List<ReturnFund> GetPopularityFunds(string data)
        {
            return GetPopularityFundsDatas(data).Select(fund => ConvertFormatFund(fund, data)).ToList();
        }

        // 根據基金國內外取得績效排名資料
        public List<ReturnFund> GetPerformanceFundRank(string dff, string data)
        {
            var funds = string.IsNullOrEmpty(dff)
                ? GetOrSetFundDataCache(data).Take(10)
                : GetOrSetFundDataCache(data).Where(f => f.DomesticForeignFundIndicator == dff).Take(10);

            return funds.Select(fund => ConvertFormatFund(fund, data)).ToList();
        }

        // 取得人氣基金資料（根據訪問記錄）
        public List<Funds> GetPopularityFundsDatas(string data)
        {
            var queryitem = FundRelatedSettingModel.GetFundDetailPageItem();
            var query = queryitem.ID.ToGuid();

            var funds = _repository.GetVisitRecords(query, "id");

            if (funds == null || !funds.Any())
            {
                return new List<Funds>();
            }

            var fundData = funds
                .OrderByDescending(x => x.VisitCount)
                .Take(10)
                .Select(x => new
                {
                    Id = x.QueryStrings.ContainsKey("id") ? x.QueryStrings["id"] : null,
                    ViewCount = x.VisitCount
                })
                .ToList();

            var allFunds = GetOrSetFundDataCache(data);

            var result = allFunds
                .Select(fund => new
                {
                    Fund = fund,
                    fundData.FirstOrDefault(fd => fd.Id == fund.ProductCode)?.ViewCount
                })
                .Where(x => x.ViewCount.HasValue)
                .OrderBy(x => fundData.FindIndex(fd => fd.Id == x.Fund.ProductCode))
                .Select(x =>
                {
                    x.Fund.ViewCount = x.ViewCount;
                    return x.Fund;
                })
                .ToList();

            return result;

        }

        // 非同步取得得獎基金資料
        private List<Funds> JsonPostAsync()
        {
            // 從快取中獲取得獎基金資料
            var awardFundData = (List<Funds>)_cache.Get(AwardFundApiCacheKey) ?? new List<Funds>();

            if (!awardFundData.Any())
            {
                List<Funds> awardFundList = new List<Funds>();

                try
                {
                    for (int year = DateTime.Now.Year - 1; year <= DateTime.Now.Year; year++)
                    {
                        string content1 = string.Empty;
                        string content2 = string.Empty;


                        Task<string> task1 = RequestAwardFundAsync("晨星基金獎", year);
                        Task<string> task2 = RequestAwardFundAsync("Smart智富台灣基金獎", year);
                        Task.WaitAll(task1, task2);

                        content1 = task1.Result;
                        content2 = task2.Result;


                        if (!string.IsNullOrEmpty(content1))
                        {
                            dynamic jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(content1);
                            awardFundList.AddRange(ProcessJsonData(jsonData));
                        }

                        if (!string.IsNullOrEmpty(content2))
                        {
                            dynamic jsonData2 = Newtonsoft.Json.JsonConvert.DeserializeObject(content2);
                            awardFundList.AddRange(ProcessJsonData(jsonData2));
                        }
                    }

                    _cache.Set(AwardFundApiCacheKey, awardFundList, DateTimeOffset.Now.AddMinutes(30));

                }
                catch (Exception ex)
                {
                    Logger.Api.Info($"An error occurred: {ex.Message}");
                }

                awardFundData = awardFundList;
            }

            return awardFundData;
        }

        // 非同步請求得獎基金資料
        private Task<string> RequestAwardFundAsync(string awardName, int year)
        {
            return Task.Run(() =>
            {
                return _route.
                    AppendPathSegments("api", "fund", "FundAward").
                    SetQueryParam("awardName", awardName).
                    SetQueryParam("awardYear", $"{year}年").
                    WithOAuthBearerToken(_token).
                    AllowAnyHttpStatus().
                    GetAsync().
                    ReceiveString();
            });
        }

        // 處理 JSON 數據並提取基金資料
        private List<Funds> ProcessJsonData(dynamic jsonData)
        {
            List<Funds> awardFundList = new List<Funds>();

            foreach (var item in jsonData.resultSet?.result as JArray ?? new JArray())
            {
                var productcode = item["v8"].ToString();

                var foundFunds = GetFundDataByProductCode(productcode);

                if (foundFunds != null && foundFunds.Count > 0)
                {
                    awardFundList.AddRange(foundFunds);
                }
            }
            return awardFundList.ToList();
        }

        // 取得得獎基金資料(依績效遞減排序取前10筆)
        public List<ReturnFund> GetAwardFunds(string data)
        {
            return JsonPostAsync().Select(fund => ConvertFormatFund(fund, data)).OrderByDescending(fund => fund.Tnav).ThenBy(fund => fund.FundCode).Take(10).ToList();
        }

        // 將基金資料轉換為輸出格式
        public ReturnFund ConvertFormatFund(Funds funds, string timePeriod)
        {
            string tnavValue;
            switch (timePeriod)
            {
                case "1":
                    tnavValue = funds.OneMonthReturnOriginalCurrency.FormatDecimalNumber(4, false, false);
                    break;
                case "3":
                    tnavValue = funds.ThreeMonthReturnOriginalCurrency.FormatDecimalNumber(4, false, false);
                    break;
                case "6":
                    tnavValue = funds.SixMonthReturnOriginalCurrency.FormatDecimalNumber(4, false, false);
                    break;
                case "12":
                    tnavValue = funds.OneYearReturnOriginalCurrency.FormatDecimalNumber(4, false, false);
                    break;
                default:
                    tnavValue = funds.SixMonthReturnOriginalCurrency.FormatDecimalNumber(4, false, false);
                    break;
            }

            var result = new ReturnFund()
            {
                Tnav = tnavValue,
                GenWebUrl = funds.AvailabilityStatus == "Y" &&
                            (funds.OnlineSubscriptionAvailability == "Y" || string.IsNullOrEmpty(funds.OnlineSubscriptionAvailability)) ? Regex.Replace(_genWebUrlString, @"\{\}", funds.ProductCode) : "N",
                WmsWenUrl = _domainUrlString + FundRelatedSettingModel.GetFundDetailsUrl() + "?id=" + funds.ProductCode,
                FundCode = funds.ProductCode,
                FundName = funds.FundName?.Normalize(NormalizationForm.FormKC),
                RiskType = funds.RiskRewardLevel,
                Date = funds.NetAssetValueDate.ToString("yyyyMMdd"),
                RiskTypeName = GetRiskTypeName(funds.RiskRewardLevel),
                Fcur = funds.CurrencyName
            };
            return result;
        }

        // 根據風險類型代碼取得風險類型名稱
        private string GetRiskTypeName(string riskType)
        {
            switch (riskType)
            {
                case "RR1":
                    return "低度屬性";
                case "RR2":
                    return "中低度屬性";
                case "RR3":
                    return "中度屬性";
                case "RR4":
                    return "中高度屬性";
                case "RR5":
                    return "高度屬性";
                default:
                    return "";
            }
        }
    }
}