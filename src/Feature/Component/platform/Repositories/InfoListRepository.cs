using Feature.Wealth.Account.Helpers;
using Feature.Wealth.ScheduleAgent.Models.Mail;
using Foundation.Wealth.Manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Repositories
{
    public class InfoListRepository
    {
        public IEnumerable<MailRecord> GetAllInfoByMember()
        {
            List<MailRecord> mailRecords = new List<MailRecord>();
            var sql = "SELECT  * FROM MailRecord WHERE PlatFormId = @id order by [InfoDateTime] desc";
            var parmas = new { id = FcbMemberHelper.GetMemberPlatFormId() };
            mailRecords = DbManager.Custom.ExecuteIList<MailRecord>(sql, parmas, commandType: CommandType.Text)?.ToList();
            return mailRecords;
        }

        public IEnumerable<MailRecord> GetTopFiveInfoByMember()
        {
            List<MailRecord> mailRecords = new List<MailRecord>();
            var sql = "SELECT TOP (5) * FROM MailRecord WHERE PlatFormId = @id order by [InfoDateTime] desc";
            var parmas = new { id = FcbMemberHelper.GetMemberPlatFormId() };
            mailRecords = DbManager.Custom.ExecuteIList<MailRecord>(sql, parmas, commandType: CommandType.Text)?.ToList();
            return mailRecords;
        }

        public bool SetInfoHaveReadByMember(string type,int num)
        {
            bool success = false;
            var sql = "UPDATE MailRecord SET HaveRead = @HaveRead WHERE PlatFormId = @id and  MailInfoType=@type and RecordNumber=@num ";
            var parmas = new { HaveRead = true, id = FcbMemberHelper.GetMemberPlatFormId(), type = type , num = num };
            var affectedRows = DbManager.Custom.ExecuteNonQuery(sql, parmas, commandType: CommandType.Text);
            success = affectedRows != 0;
            return success;
        }
        public bool SetAllInfoHaveReadByMember()
        {
           bool success = false;
            var sql = "UPDATE MailRecord SET HaveRead = @HaveRead WHERE PlatFormId = @id AND HaveRead=0";
            var parmas = new { HaveRead = true, id = FcbMemberHelper.GetMemberPlatFormId()};
            var affectedRows = DbManager.Custom.ExecuteNonQuery(sql, parmas, commandType: CommandType.Text);
            success = affectedRows != 0;
            return success;
        }
    }
}
