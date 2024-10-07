using Foundation.Wealth.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Feature.Wealth.Account.Models.OAuth
{
    public class UserMark
    {
        protected string ApplicationName = "mark";

        public Dictionary<string, MarkInfo> MarkInfos { get; set; }
        public UserMark()
        {
            this.MarkInfos = HttpContext.Current.Application[ApplicationName] as Dictionary<string, MarkInfo> ?? new Dictionary<string, MarkInfo>();
        }

        public void AddMarkInfo(MarkInfo markInfo)
        {
            this.MarkInfos.TryAdd(markInfo.TxReqId, markInfo);
        }
        public void RemoveMarkInfo(MarkInfo markInfo)
        {
            this.MarkInfos.RemoveByKey(markInfo.TxReqId);
        }
        public void UpdateMarkInfo(string txReqId, string userId)
        {
            MarkInfo tmpMarkInfo;
            if (this.MarkInfos.TryGetValue(txReqId, out tmpMarkInfo))
            {
                tmpMarkInfo.UserId = userId;             
            }
        }
        public string GetUserIdByQueueId(string queueId) => this.MarkInfos.Values.Where(user => user.QueueId == queueId).FirstOrDefault()?.UserId;

    }

    public class MarkInfo
    {
        public string QueueId { get; set; }

        public string TxReqId { get; set; }

        public string UserId { get; set; }

    }
}
