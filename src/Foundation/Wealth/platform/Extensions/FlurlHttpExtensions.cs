using Flurl.Http;
using System;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Foundation.Wealth.Extensions
{
    public static class FlurlHttpExtensions
    {
        public static async Task<IFlurlResponse> LogIfError(this Task<IFlurlResponse> res, string functionName, string requsetUrl)
        {
            var response =  await res.ConfigureAwait(false);
            if (response.StatusCode > 300)
            {
                var error = response.ResponseMessage?.ToString();
                Logger.Api.Error($"[Function] {functionName} {Environment.NewLine} [Request Url] {requsetUrl} {Environment.NewLine} [StatusCode] {response.StatusCode} {Environment.NewLine} [ErrorMessage] {error}");
            }
            return response;
        }
    }
}
