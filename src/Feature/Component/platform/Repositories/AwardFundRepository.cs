using Flurl;
using System;
using Flurl.Http;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Sitecore.Configuration;
using System.Collections.Generic;
using Foundation.Wealth.Manager;
using static Feature.Wealth.Component.Models.AwardFund.AwardFundModel;

namespace Feature.Wealth.Component.Repositories
{
    public class AwardFundRepository
    {
        public List<Funds> GetFundData(string code)
        {
            List<Funds> fundItems = new List<Funds>();
            var param = new { Code = code };

            string sql = "SELECT * FROM [FCB_sitecore_Custom].[dbo].[vw_BasicFund] WHERE ProductCode =" + "'" + code + "'";
            var results = DbManager.Custom.ExecuteIList<Funds>(sql, param, CommandType.Text);

            foreach (var item in results)
            {
                ProcessFundFilterDatas(item);
                fundItems.Add(item);
            }

            return fundItems;
        }

        private void ProcessFundFilterDatas(Funds item)
        {
            item.NetAssetValue = decimal.Round(item.NetAssetValue, 4);
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
                var content = await _route.AppendPathSegment(_url)
                                .SetQueryParam("x", "FundAward")
                                .SetQueryParam("c", "firstbank")
                                .SetQueryParam("d", 0)
                                .SetQueryParam("A", "晨星基金獎")
                                .SetQueryParam("B", "2024年")
                                .GetAsync()
                                .ReceiveString()
        ;
                dynamic jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
                awardFundList.AddRange(ProcessJsonData(jsonData));

                var content2 = await _route.AppendPathSegment(_url)
                               .SetQueryParam("x", "FundAward")
                               .SetQueryParam("c", "firstbank")
                               .SetQueryParam("d", 0)
                               .SetQueryParam("A", "Smart智富台灣基金獎")
                               .SetQueryParam("B", "2024年")
                               .GetAsync()
                               .ReceiveString()
       ;
                dynamic jsonData2 = Newtonsoft.Json.JsonConvert.DeserializeObject(content2);
                awardFundList.AddRange(ProcessJsonData(jsonData2));

                var content3 = await _route.AppendPathSegment(_url)
                               .SetQueryParam("x", "FundAward")
                               .SetQueryParam("c", "firstbank")
                               .SetQueryParam("d", 0)
                               .SetQueryParam("A", "晨星基金獎")
                               .SetQueryParam("B", "2023年")
                               .GetAsync()
                               .ReceiveString()
       ;
                dynamic jsonData3 = Newtonsoft.Json.JsonConvert.DeserializeObject(content3);
                awardFundList.AddRange(ProcessJsonData(jsonData3));

                var content4 = await _route.AppendPathSegment(_url)
                               .SetQueryParam("x", "FundAward")
                               .SetQueryParam("c", "firstbank")
                               .SetQueryParam("d", 0)
                               .SetQueryParam("A", "Smart智富台灣基金獎")
                               .SetQueryParam("B", "2023年")
                               .GetAsync()
                               .ReceiveString()
       ;
                dynamic jsonData4 = Newtonsoft.Json.JsonConvert.DeserializeObject(content4);
                awardFundList.AddRange(ProcessJsonData(jsonData4));

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
