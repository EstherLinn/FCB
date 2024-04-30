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


namespace Feature.Wealth.Component.Repositories
{
    public class AwardFundRepository
    {
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

            foreach (var item in results)
            {
                ProcessFundFilterDatas(item);
                fundItems.Add(item);
            }

            return fundItems;
        }

        private void ProcessFundFilterDatas(Funds item)
        {
            item.NetAssetValue = NumberExtensions.RoundingValue(item.NetAssetValue);
        }

        /// <summary>
        /// 一銀得獎基金API
        /// https://wms.firstbank.com.tw/jsondata/fundjsondata.xdjjson?x=FundAward&c=firstbank&d=0&A="得獎名稱"&B="年度:2023年"
        /// </summary>

        private readonly string _route = Settings.GetSetting("FirstbankApiRoute");
        private readonly string _url = Settings.GetSetting("FirstbankApiUrl");

        public async Task<List<Funds>> JsonPostAsync()
        {
            List<Funds> awardFundList = new List<Funds>();


            try
            {
                for (int year = DateTime.Now.Year - 1; year <= DateTime.Now.Year; year++)
                {
                    var content = await _route.AppendPathSegment(_url)
                                              .SetQueryParam("x", "FundAward")
                                              .SetQueryParam("c", "firstbank")
                                              .SetQueryParam("d", 0)
                                              .SetQueryParam("A", "晨星基金獎")
                                              .SetQueryParam("B", $"{year}年")
                                              .GetAsync()
                                              .ReceiveString();

                    dynamic jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
                    awardFundList.AddRange(ProcessJsonData(jsonData));

                    var content2 = await _route.AppendPathSegment(_url)
                                               .SetQueryParam("x", "FundAward")
                                               .SetQueryParam("c", "firstbank")
                                               .SetQueryParam("d", 0)
                                               .SetQueryParam("A", "Smart智富台灣基金獎")
                                               .SetQueryParam("B", $"{year}年")
                                               .GetAsync()
                                               .ReceiveString();

                    dynamic jsonData2 = Newtonsoft.Json.JsonConvert.DeserializeObject(content2);
                    awardFundList.AddRange(ProcessJsonData(jsonData2));
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

            var resultsArray = jsonData.ResultSet.Result as JArray;
            if (resultsArray != null)
            {
                foreach (var item in resultsArray)
                {
                    var productcode = item["V8"].ToString();

                    List<Funds> foundFunds = GetFundData(productcode);

                    if (foundFunds != null && foundFunds.Count > 0)
                    {
                        foreach (var fund in foundFunds)
                        {
                            fund.Year = item["V2"].ToString();
                            fund.FundTypeNameByAPI = item["V9"].ToString();
                            fund.AwardName = item["V4"].ToString();
                        }

                        awardFundList.AddRange(foundFunds);
                    }
                }
            }

            return awardFundList;
        }

    }
}
