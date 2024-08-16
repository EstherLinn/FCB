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
        public async Task SyncTrackListToIleo(string promotionCode, string productId)
        {
            if (string.IsNullOrEmpty(_route))
            {
                Logger.Api.Info($"關注清單API Function開始 理財網同步回ileo,取得promotionCode ={promotionCode},api route =null or empty");
                return;
            }
            try
            {

                var routeWithParams = _route.
                    AppendQueryParam("promotionCode", promotionCode)
                   .AppendQueryParam("channel", "wms")
                   .AppendQueryParam("fundCode", productId);

                var reqObj = new {  promotionCode, channel = "wms", fundCode = productId };
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
                try
                {
                    MemberRepository _memberRepository = new MemberRepository();
                    var originData = _memberRepository.GetTrackListFromDb(FcbMemberHelper.GetMemberPlatFormId());
                    if (!originData.Any())
                    {
                        foreach (var item in focusListResp.TrackList)
                        {
                            originData.Add(new TrackListModel()
                            {
                                Id = item.fundCode,
                            });
                        }
                    }
                    else
                    {
                        var tmpData = new List<TrackListModel>(originData);
                        //清除ileo已取消關注資料
                        foreach (var item in originData)
                        {
                                //不存在ileo但理財網還在，刪除
                                if (!focusListResp.TrackList.Exists(x => x.fundCode == item.Id))
                                {
                                    tmpData.RemoveAll(x => x.Id == item.Id);
                                }
                        }
                        originData = tmpData;
                        //加入新的ileo資料
                        foreach (var item in focusListResp.TrackList)
                        {
                            if (!originData.Exists(x => x.Id == item.fundCode))
                            {
                                originData.Add(new TrackListModel()
                                {
                                    Id = item.fundCode,
                                });
                            }
                        }

                    }
                    _memberRepository.InSertTrackList(originData);
                }
                catch (Exception ex)
                {
                    Logger.Api.Info($"ileo關注清單 ileo同步關注清單回理財網,Error Message :{ex.ToString()}");
                }
            }
        }
    }
}
