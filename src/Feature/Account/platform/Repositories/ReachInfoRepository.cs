using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Account.Models.ReachInfo;
using Foundation.Wealth.Manager;
using log4net;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.Account.Repositories
{
    public class ReachInfoRepository
    {
        private ILog Log { get; } = Logger.Account;
        public List<ReachInfo> GetProductReachInfosByType(string platFormId, string type)
        {
            string sql = $@" Select * FROM MemberReachInfo Where PlatFormId COLLATE Latin1_General_CS_AS = @id and InvestType =@type";
            var para = new { id = FcbMemberHelper.GetMemberPlatFormId(), type = type };
            var results = DbManager.Custom.ExecuteIList<ReachInfo>(sql, para, CommandType.Text)?.ToList();
            return results;
        }

        public List<ReachInfo> GetProductReachInfo(string platFormId, string type, string id)
        {
            string sql = $@" SELECT PlatFormId,InvestType,InvestId,PriceValue,InfoType,ReachValue,RiseValue,RisePercent,FallValue,FallPercent,
                Format(InfoStartDate,'yyyy/MM/dd')InfoStartDate,Format(InfoEndDate,'yyyy/MM/dd')InfoEndDate,Format(SetDateTime,'yyyy/MM/dd')SetDateTime,OpenInfo
                from MemberReachInfo  where PlatFormId COLLATE Latin1_General_CS_AS=@platFormId and InvestType=@type and InvestId=@id";
            var para = new { platFormId = platFormId, type = type, id = id };
            var results = DbManager.Custom.ExecuteIList<ReachInfo>(sql, para, CommandType.Text)?.ToList();
            return results;
        }

        public bool SetReachInfo(ReachInfo reachInfo)
        {
            bool success = false;
            string strSql = @$"MERGE MemberReachInfo AS target 
USING (SELECT @PlatFormId COLLATE Latin1_General_CS_AS,@InvestId,@InfoType ) AS source (PlatFormId,InvestId,InfoType) 
ON (target.PlatFormId = source.PlatFormId and target.InvestId = source.InvestId and target.InfoType = source.InfoType) 
WHEN MATCHED THEN UPDATE SET PriceValue=@PriceValue, ReachValue = @ReachValue,RiseValue =@RiseValue,RisePercent =@RisePercent,FallValue=@FallValue,FallPercent=@FallPercent,
InfoStartDate=@InfoStartDate,InfoEndDate=@InfoEndDate,SetDateTime=@SetDateTime,OpenInfo=@OpenInfo
WHEN NOT MATCHED BY TARGET THEN INSERT (PlatFormId,InvestType,InvestId,PriceValue,InfoType,ReachValue,RiseValue,RisePercent,FallValue,FallPercent,
                InfoStartDate,InfoEndDate,SetDateTime,OpenInfo) values 
               (@PlatFormId,@InvestType,@InvestId,@PriceValue,@InfoType,@ReachValue,@RiseValue,@RisePercent,@FallValue,@FallPercent,
            @InfoStartDate,@InfoEndDate,@SetDateTime,@OpenInfo);";
            var para = new ReachInfo()
            {
                PlatFormId = FcbMemberHelper.GetMemberPlatFormId(),
                InvestType = reachInfo.InvestType,
                InvestId = reachInfo.InvestId,
                PriceValue = reachInfo.PriceValue,
                InfoType = reachInfo.InfoType,
                ReachValue = reachInfo.ReachValue,
                RiseValue = reachInfo.RiseValue,
                RisePercent = reachInfo.RisePercent,
                FallValue = reachInfo.FallValue,
                FallPercent = reachInfo.FallPercent,
                InfoStartDate = reachInfo.InfoStartDate,
                InfoEndDate = reachInfo.InfoEndDate,
                SetDateTime = DateTime.Now,
                OpenInfo = reachInfo.OpenInfo,
            };
            try
            {
                var affectedRows = DbManager.Custom.ExecuteNonQuery(strSql, para, commandType: System.Data.CommandType.Text);
                success = affectedRows != 0;
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return success;
        }

        public void DeleteCancelFocusRenchInfo(string investId)
        {
            string sql = $@"Delete From MemberReachInfo Where InvestId =@InvestId and PlatFormId COLLATE Latin1_General_CS_AS = @PlatFormId";
            var para = new { InvestId = investId, PlatFormId = FcbMemberHelper.GetMemberPlatFormId() };
            DbManager.Custom.ExecuteNonQuery(sql, para, CommandType.Text);
        }
    }
}
