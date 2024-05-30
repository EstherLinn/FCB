using Flurl;
using System;
using Dapper;
using Flurl.Http;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Sitecore.Configuration;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using static Feature.Wealth.Component.Models.AwardFund.AwardFundModel;
using Foundation.Wealth.Extensions;
using System.Runtime.Caching;
using System.Linq;
using System.Text;
using Feature.Wealth.Component.Models.FundDetail;

namespace Feature.Wealth.Component.Repositories
{
    public class AwardFundRepository
    {
        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly string AwardFundCacheKey = $"Fcb_AwardFundCache";

        public List<Funds> GetFundData(string code)
        {
            List<Funds> fundItems = new List<Funds>();

            var parameters = new DynamicParameters();
            parameters.Add("@ProductCode", code, DbType.String, ParameterDirection.Input, code.Length);

            string sql = """
                SELECT *
                FROM [vw_BasicFund]
                WHERE ProductCode = @ProductCode
                """;


            var results = DbManager.Custom.ExecuteIList<Funds>(sql, parameters, CommandType.Text);
            var _tagsRepository = new TagsRepository();
            var tags = _tagsRepository.GetFundTagData();
            foreach (var item in results)
            {
                ProcessFundFilterDatas(item);
                item.Tags = [];
                item.Tags.AddRange(from tagModel in tags.Where(t => t.FundTagType == FundTagEnum.DiscountTag)
                                   where tagModel.ProductCodes.Contains(item.ProductCode)
                                   select tagModel.TagName);
                fundItems.Add(item);
            }

            return fundItems;
        }

        private void ProcessFundFilterDatas(Funds item)
        {
            item.ProductName = item.ProductName.Normalize(NormalizationForm.FormKC);
            item.NetAssetValue = NumberExtensions.RoundingValue(item.NetAssetValue);
        }

        private readonly string _route = Settings.GetSetting("MoneyDjApiRoute");
        private readonly string _token = Settings.GetSetting("MoneyDjToken");

        public List<Funds> GetOrSetAwardFundCache()
        {
            var awardFundData = (List<Funds>)_cache.Get(AwardFundCacheKey) ?? new List<Funds>();

            if (!awardFundData.Any())
            {
                awardFundData = JsonPostAsync();
                _cache.Set(AwardFundCacheKey, awardFundData, DateTimeOffset.Now.AddMinutes(60));
            }

            return awardFundData;
        }

        private List<Funds> JsonPostAsync()
        {
            List<Funds> awardFundList = new List<Funds>();

            try
            {
                for (int year = DateTime.Now.Year - 1; year <= DateTime.Now.Year; year++)
                {
                    Task<string> task1 = Task.Run(() =>
                    {
                        return _route.
                            AppendPathSegments("api", "fund", "FundAward").
                            SetQueryParam("awardName", "晨星基金獎").
                            SetQueryParam("awardYear", $"{year}年").
                            WithOAuthBearerToken(_token).
                            AllowAnyHttpStatus().
                            GetAsync().
                            ReceiveString();
                    });

                    Task<string> task2 = Task.Run(() =>
                    {
                        return _route.
                            AppendPathSegments("api", "fund", "FundAward").
                            SetQueryParam("awardName", "Smart智富台灣基金獎").
                            SetQueryParam("awardYear", $"{year}年").
                            WithOAuthBearerToken(_token).
                            AllowAnyHttpStatus().
                            GetAsync().
                            ReceiveString();
                    });

                    Task.WaitAll(task1, task2);

                    string content1 = task1.Result;
                    string content2 = task2.Result;

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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return awardFundList;
        }

        private List<Funds> ProcessJsonData(dynamic jsonData)
        {
            List<Funds> awardFundList = new List<Funds>();

            foreach (var item in jsonData.resultSet?.result as JArray ?? new JArray())
            {
                var productcode = item["v8"].ToString();

                List<Funds> foundFunds = GetFundData(productcode);

                if (foundFunds != null && foundFunds.Count > 0)
                {
                    foreach (var fund in foundFunds)
                    {
                        fund.Year = item["v2"].ToString();
                        fund.FundTypeNameByAPI = item["v9"].ToString();
                        fund.AwardName = item["v4"].ToString();
                    }

                    awardFundList.AddRange(foundFunds);
                }
            }


            return awardFundList;
        }

    }
}
