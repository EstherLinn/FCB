using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Account.Models.Api;
using Feature.Wealth.Account.Models.FundTrackList;
using Feature.Wealth.Account.Repositories;
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
                return result;
            }
            try
            {
                var reqObj = new { promotionCode = promotionCode, channel = "wms" };
                var request = await _route.
                    AllowAnyHttpStatus().
                    PostJsonAsync(reqObj);
                if (request.StatusCode < 300)
                {
                    var resp = await request.GetStringAsync();
                    if (!string.IsNullOrEmpty(resp))
                    {
                        result = JsonConvert.DeserializeObject<FocusListResp>(resp);
                    }
                    Logger.Api.Info($"ileo關注清單response: {resp}");
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

        public void SyncTrackList(FocusListResp focusListResp)
        {
            if (focusListResp.rt == "0000" && focusListResp.TrackList != null)
            {
                MemberRepository _memberRepository = new MemberRepository();
                var originData = _memberRepository.GetTrackListFromDb(FcbMemberHelper.GetMemberPlatFormId());
                if (!originData.Any())
                {
                    foreach (var item in focusListResp.TrackList)
                    {
                        if (item.fundType == "1" || item.fundType == "2" || item.fundType == "E" || item.fundType == "S")
                        {
                            string getType = string.Empty;
                            switch (item.fundType)
                            {
                                case "1":
                                    getType = "fund";
                                    break;
                                case "2":
                                    getType = "fund";
                                    break;
                                case "E":
                                    getType = "etf";
                                    break;
                                case "S":
                                    getType = "foreignstocks";
                                    break;
                            }

                            originData.Add(new TrackListModel()
                            {
                                Id = item.fundCode,
                                Type = getType
                            });
                        }
                    }

                }
                else
                {
                    foreach (var item in focusListResp.TrackList)
                    {
                        if (!originData.Any(x => x.Id == item.fundCode))
                        {
                            if (item.fundType == "1" || item.fundType == "2" || item.fundType == "E" || item.fundType == "S")
                            {
                                string getType = string.Empty;
                                switch (item.fundType)
                                {
                                    case "1":
                                        getType = "fund";
                                        break;
                                    case "2":
                                        getType = "fund";
                                        break;
                                    case "E":
                                        getType = "etf";
                                        break;
                                    case "S":
                                        getType = "foreignstocks";
                                        break;
                                }

                                originData.Add(new TrackListModel()
                                {
                                    Id = item.fundCode,
                                    Type = getType
                                });
                            }
                        }
                    }
                }
                _memberRepository.InSertTrackList(originData);
            }
        }
    }
}
