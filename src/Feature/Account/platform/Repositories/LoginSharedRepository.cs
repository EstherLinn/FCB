using Foundation.Wealth.Manager;
using Sitecore.Collections;
using Sitecore.Configuration;
using System;
using System.Threading.Tasks;

namespace Feature.Wealth.Account.Repositories
{
    public class LoginSharedRepository
    {
        public async Task<bool> RecordTansaction(string queueId, string txReqId)
        {
            int affectedRow = 0;
            string strSql = $@"INSERT INTO [LoginShared] (QueueId,TxReqId,TansactionTime) values (@QueueId,@TxReqId,@TansactionTime)";
            var para = new
            {
                QueueId = queueId,
                TxReqId = txReqId,
                TansactionTime = DateTime.Now
            };
            affectedRow = await DbManager.Custom.ExecuteNonQueryAsync(strSql, para, commandType: System.Data.CommandType.Text);
            return affectedRow > 0;
        }

        public void UpdateUserId(string txReqId, string userId)
        {
            string strSql = $@" UPDATE [LoginShared] Set UserId = @UserId where TxReqId =@TxReqId";
            var para = new
            {
                UserId = userId,
                TxReqId = txReqId,
            };
            DbManager.Custom.ExecuteNonQuery(strSql, para, commandType: System.Data.CommandType.Text);
        }

        public string GetUserIdByTansaction(string queueId)
        {
            string strSql = $@" Select UserId  From [LoginShared]  where QueueId =@queueId";
            var para = new
            {
                QueueId = queueId,
            };
            var userId = DbManager.Custom.Execute<string>(strSql, para, commandType: System.Data.CommandType.Text);
            DeleteTansactionRecordByQueueId(queueId);
            return userId;
        }

        public bool IsTansactionTimeExpired(string txReqId)
        {
            var tansactionLimit = string.IsNullOrEmpty(Settings.GetSetting("LoginTansactionLimit")) ? "10" : Settings.GetSetting("LoginTansactionLimit");
            string strSql = $@" Select TansactionTime From [LoginShared]  where TxReqId =@TxReqId";
            var para = new
            {
                TxReqId = txReqId,
            };
            var tansactionTime = DbManager.Custom.Execute<DateTime?>(strSql, para, commandType: System.Data.CommandType.Text);
            if (!tansactionTime.HasValue)
            {
                DeleteTansactionRecordByTxReqId(txReqId);
                return true;
            }
            else
            {
                tansactionTime.Value.AddMinutes(Convert.ToInt16(tansactionLimit));
                DateTime currentTime = DateTime.Now;
                int result = DateTime.Compare(currentTime, (DateTime)tansactionTime);
                if (result < 0 || result == 0)
                {
                    return false;
                }
                else
                {
                    DeleteTansactionRecordByTxReqId(txReqId);
                    return true;
                }
            }
        }

        private void DeleteTansactionRecordByQueueId(string queueId)
        {
            string strSql = $@" Delete FROM [LoginShared] where QueueId =@queueId";
            var para = new
            {
                QueueId = queueId,
            };
            DbManager.Custom.ExecuteNonQuery(strSql, para, commandType: System.Data.CommandType.Text);
        }
        private void DeleteTansactionRecordByTxReqId(string txReqId)
        {
            string strSql = $@" Delete FROM [LoginShared] where TxReqId =@txReqId";
            var para = new
            {
                TxReqId = txReqId,
            };
            DbManager.Custom.ExecuteNonQuery(strSql, para, commandType: System.Data.CommandType.Text);
        }

    }
}
