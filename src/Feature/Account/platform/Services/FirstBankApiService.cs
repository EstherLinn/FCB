using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Account.Models.Api;
using Feature.Wealth.Account.Models.FundTrackList;
using Feature.Wealth.Account.Repositories;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.Account.Services
{
    public class FirstBankApiService
    {
        private readonly string _route = Settings.GetSetting("FocusApiRoute");

        public async Task<FocusListResp> GetTrackListFromIleo(string promotionCode)
        {
            FocusListResp result = new FocusListResp();
            if (string.IsNullOrEmpty(_route))
            {
                Logger.Api.Info($"關注清單API Function開始 ,取得promotionCode ={promotionCode},api route =null or empty");
                return result;
            }
            try
            {
                var routeWithParams = _route + string.Format("?promotionCode={0}&channel=wms", promotionCode);
                var reqObj = new { promotionCode = promotionCode, channel = "wms" };
                Logger.Api.Info($"關注清單API Function開始 ,取得promotionCode ={promotionCode},api route ={routeWithParams},帶入參數promotionCode={promotionCode},channel=wms");
                var request = await routeWithParams.
                    AllowAnyHttpStatus().
                    PostJsonAsync(reqObj);
                if (request.StatusCode < 300)
                {
                    var resp = await request.GetStringAsync();
                    if (!string.IsNullOrEmpty(resp))
                    {
                        result = JsonConvert.DeserializeObject<FocusListResp>(resp);
                    }
                    Logger.Api.Info($"ileo關注清單回覆 StatusCode={request.StatusCode}, 回覆內容={resp}");
                }
                else
                {
                    var error = await request.GetStringAsync();
                    Logger.Api.Info("ileo關注清單,StatusCode :" + request.StatusCode + "response :" + error);
                }
            }
            catch (FlurlHttpException ex)
            {
                Logger.Api.Info($"ileo關注清單,Error returned from {ex.Call.Request.Url} , Error Message : {ex.Message}");
            }
            catch (Exception ex)
            {
                Logger.Api.Info($"ileo關注清單,Error Message :{ex.Message}");
            }

            return result;

        }
        /// <summary>
        /// 同步關注清單項目到iLeo
        /// </summary>
        /// <param name="promotionCode"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task SyncTrackListToIleo(string promotionCode, string productId , string productType)
        {
            if (string.IsNullOrEmpty(_route))
            {
                Logger.Api.Info($"關注清單API Function開始 理財網同步回ileo,取得promotionCode ={promotionCode},api route =null or empty");
                return;
            }
            try
            {
                var type = string.Empty;
                switch (productType.ToLower())
                {
                    case "fund":
                        type = "F";
                        break;
                    case "etf":
                        type = "E";
                        break;
                    case "foreignstocks":
                        type = "G";
                        break;
                    case "foreignbonds":
                        type = "X";
                        break;
                }
                var routeWithParams = _route.
                    AppendQueryParam("promotionCode", promotionCode)
                   .AppendQueryParam("channel", "wms")
                   .AppendQueryParam("fundCode", productId)
                   .AppendQueryParam("fundType", type);

                var reqObj = new { promotionCode = promotionCode, channel = "wms", fundCode = productId , fundType  = type};
                Logger.Api.Info($"關注清單API Function開始 理財網同步回ileo,取得promotionCode ={promotionCode},api route ={routeWithParams},帶入參數promotionCode={promotionCode},channel=wms,fundCode={productId}");
                var request = await routeWithParams.
                    AllowAnyHttpStatus().
                    PostJsonAsync(reqObj);
                if (request.StatusCode < 300)
                {
                    var resp = await request.GetStringAsync();
                    Logger.Api.Info($"ileo關注清單回覆 理財網同步回ileo, StatusCode={request.StatusCode}, 回覆內容={resp}");
                }
                else
                {
                    var error = await request.GetStringAsync();
                    Logger.Api.Info("ileo關注清單,StatusCode :" + request.StatusCode + "response :" + error);
                }
            }
            catch (FlurlHttpException ex)
            {
                Logger.Api.Info($"ileo關注清單 理財網同步回ileo,Error returned from {ex.Call.Request.Url} , Error Message : {ex.Message}");
            }
            catch (Exception ex)
            {
                Logger.Api.Info($"ileo關注清單 理財網同步回ileo,Error Message :{ex.Message}");
            }
        }
        /// <summary>
        /// 同步關注清單回理財網
        /// </summary>
        /// <param name="focusListResp"></param>
        public void SyncTrackListFormIleo(FocusListResp focusListResp)
        {
            if (focusListResp.rt == "0000" && focusListResp.TrackList != null)
            {
                MemberRepository _memberRepository = new MemberRepository();
                var originData = _memberRepository.GetTrackListFromDb(FcbMemberHelper.GetMemberPlatFormId());
                if (!originData.Any())
                {
                    var type = string.Empty;
                    foreach (var item in focusListResp.TrackList)
                    {
                        switch (item.fundType)
                        {
                            case "F":
                                type = "Fund";
                                break;
                            case "E":
                                type = "ETF";
                                break;
                            case "G":
                                type = "ForeignStocks";
                                break;
                            case "X":
                                type = "ForeignBonds";
                                break;
                        }
                        originData.Add(new TrackListModel()
                        {
                            Id = item.fundCode,
                            Type = type
                        });
                    }
                }
                else
                {
                    foreach (var item in focusListResp.TrackList)
                    {
                        if (!originData.Exists(x => x.Id == item.fundCode))
                        {
                            var type = string.Empty;
                            foreach (var item2 in focusListResp.TrackList)
                            {
                                switch (item.fundType)
                                {
                                    case "F":
                                        type = "Fund";
                                        break;
                                    case "E":
                                        type = "ETF";
                                        break;
                                    case "G":
                                        type = "ForeignStocks";
                                        break;
                                    case "X":
                                        type = "ForeignBonds";
                                        break;
                                }
                                originData.Add(new TrackListModel()
                                {
                                    Id = item.fundCode,
                                    Type = type
                                });
                            }
                            originData.Add(new TrackListModel()
                            {
                                Id = item.fundCode,
                                Type = type
                            });
                        }
                    }
                }
                _memberRepository.InSertTrackList(originData);
            }
        }
    }
}
