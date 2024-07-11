using Feature.Wealth.ScheduleAgent.Models.News.Content;
using Feature.Wealth.ScheduleAgent.Models.News.List;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using Sitecore.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.ScheduleAgent.Services
{
    internal class NewsService
    {
        private readonly string Route = Settings.GetSetting("MoneyDjApiRoute");
        private readonly string Token = Settings.GetSetting("MoneyDjToken");
        private readonly ILoggerService _logger;

        public NewsService(ILoggerService logger)
        {
            this._logger = logger;
        }

        public async Task<List<NewsListResult>> GetNewsListData(NewsListRequest req)
        {
            if (req == null)
            {
                req = new NewsListRequest();
            }

            if (string.IsNullOrEmpty(req.Id))
            {
                req.Id = "1"; // initial value
            }

            if (req.Count == 0)
            {
                req.Count = 50; // initial value
            }

            List<NewsListResult> result = null;
            string response = string.Empty;

            try
            {
                response = await Route.AppendPathSegments("api", "News", "kmdjnews", "type", req.Id, req.Count.Value)
                   .SetQueryParams(new { StartDatetime = req.StartDateTime, EndDatetime = req.EndDateTime })
                   .WithOAuthBearerToken(Token)
                   .GetAsync()
                   .ReceiveString();
            }
            catch (FlurlHttpException ex)
            {
                var status = ex.StatusCode;
                var resp = await ex.GetResponseStringAsync();
                this._logger.Error($"Error returned from {ex.Call.Request.Url} {Environment.NewLine}[Message] {ex.Message} {Environment.NewLine}[StatusCode] {status}{Environment.NewLine}[Response] {resp}");
            }
            catch (Exception ex)
            {
                this._logger.Error("新聞列表API", ex);
            }

            try
            {
                var newsResponse = JsonConvert.DeserializeObject<NewsListResponse>(response);
                if (newsResponse == null || newsResponse.ResultSet == null || newsResponse.ResultSet.Result == null)
                {
                    return result;
                }

                result = newsResponse.ResultSet.Result.ToList();
            }
            catch (Exception ex)
            {
                this._logger.Error("新聞列表資料轉換", ex);
            }

            return result;
        }

        public async Task<List<NewsContentResult>> GetNewsContentData(NewsContentRequest req)
        {
            if (req == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(req.NewsSerialNumber))
            {
                this._logger.Info("新聞序號為空");
                return null;
            }

            List<NewsContentResult> result = null;
            string response = string.Empty;

            try
            {
                response = await Route.AppendPathSegments("api", "News", "kmdjnews", "content", req.NewsSerialNumber)
                   .WithOAuthBearerToken(Token)
                   .GetAsync()
                   .ReceiveString();
            }
            catch (FlurlHttpException ex)
            {
                var status = ex.StatusCode;
                var resp = await ex.GetResponseStringAsync();
                this._logger.Error($"Error returned from {ex.Call.Request.Url} {Environment.NewLine}[Message] {ex.Message} {Environment.NewLine}[StatusCode] {status}{Environment.NewLine}[Response] {resp}");
            }
            catch (Exception ex)
            {
                this._logger.Error("新聞內容API", ex);
            }

            try
            {
                var newsResponse = JsonConvert.DeserializeObject<NewsContentResponse>(response);
                if (newsResponse == null || newsResponse.ResultSet == null || newsResponse.ResultSet.Result == null)
                {
                    return result;
                }

                result = newsResponse.ResultSet.Result.ToList();
            }
            catch (Exception ex)
            {
                this._logger.Error("新聞內容資料轉換", ex);
            }

            return result;
        }
    }
}