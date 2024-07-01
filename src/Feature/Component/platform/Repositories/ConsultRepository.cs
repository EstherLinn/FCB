using Dapper;
using Feature.Wealth.Component.Models.Consult;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Feature.Wealth.Component.Repositories
{
    public class ConsultRepository
    {
        private readonly IDbConnection _dbConnection = DbManager.Custom.DbConnection();
        private readonly DjMoneyApiRespository _djMoneyApiRespository = new DjMoneyApiRespository();

        public IList<ConsultSchedule> GetConsultScheduleList()
        {
            string sql = @"SELECT
                           [ScheduleID],[EmployeeID],[EmployeeName]
                           ,[CustomerID],[CustomerName],[ScheduleDate]
                           ,[StartTime],[EndTime],[DNIS]
                           ,[EmployeeURL],[CustomerURL],[DepartmentCode]
                           ,[BranchCode],[BranchName],[BranchPhone]
                           ,[Type],[Phone],[Mail]
                           ,[Subject],[Description]
                           ,IIF([StatusCode] = '1' AND GETDATE() > (CAST([ScheduleDate] AS varchar) + ' ' + [EndTime]), '2', [StatusCode]) [StatusCode]
                           ,[CreatedOn],[ModifiedOn]
                           ,IIF([ScheduleDate] = CAST(GETDATE() AS DATE), 1, 0) [Comming]
                           ,IIF(GETDATE() BETWEEN (CAST([ScheduleDate] AS varchar) + ' ' + [StartTime]) AND (CAST([ScheduleDate] AS varchar) + ' ' + [EndTime]) , 1, 0) [Start]
                           FROM [ConsultSchedule] WITH (NOLOCK)
                           ORDER BY [ScheduleDate],[StartTime]";

            var result = this._dbConnection.Query<ConsultSchedule>(sql)?.ToList() ?? new List<ConsultSchedule>();

            if (result != null && result.Any())
            {
                for (int i = 0; i < result.Count; i++)
                {
                    result[i].ScheduleDateString = result[i].ScheduleDate.ToString("yyyy-MM-dd");
                }
            }

            return result;
        }

        public ConsultSchedule GetConsultSchedule(string scheduleID)
        {
            string sql = @"SELECT
                           [ScheduleID],[EmployeeID],[EmployeeName]
                           ,[CustomerID],[CustomerName],[ScheduleDate]
                           ,[StartTime],[EndTime],[DNIS]
                           ,[EmployeeURL],[CustomerURL],[DepartmentCode]
                           ,[BranchCode],[BranchName],[BranchPhone]
                           ,[Type],[Phone],[Mail]
                           ,[Subject],[Description]
                           ,IIF([StatusCode] = '1' AND GETDATE() > (CAST([ScheduleDate] AS varchar) + ' ' + [EndTime]), '2', [StatusCode]) [StatusCode]
                           ,[CreatedOn],[ModifiedOn]
                           ,IIF([ScheduleDate] = CAST(GETDATE() AS DATE), 1, 0) [Comming]
                           ,IIF(GETDATE() BETWEEN (CAST([ScheduleDate] AS varchar) + ' ' + [StartTime]) AND (CAST([ScheduleDate] AS varchar) + ' ' + [EndTime]) , 1, 0) [Start]
                           FROM [ConsultSchedule] WITH (NOLOCK)
                           WHERE ScheduleID = @ScheduleID";

            var result = this._dbConnection.Query<ConsultSchedule>(sql, new { ScheduleID = scheduleID })?.FirstOrDefault() ?? new ConsultSchedule();

            return result;
        }

        public void InsertConsultSchedule(ConsultSchedule consultSchedule)
        {
            string sql = @"INSERT INTO [ConsultSchedule]
                           ([ScheduleID],[EmployeeID],[EmployeeName]
                           ,[CustomerID],[CustomerName],[ScheduleDate]
                           ,[StartTime],[EndTime],[DNIS]
                           ,[EmployeeURL],[CustomerURL],[DepartmentCode]
                           ,[BranchCode],[BranchName],[BranchPhone]
                           ,[Type],[Phone],[Mail]
                           ,[Subject],[Description],[StatusCode]
                           ,[CreatedOn],[ModifiedOn])
                           VALUES
                           (@ScheduleID,@EmployeeID,@EmployeeName
                           ,@CustomerID,@CustomerName,@ScheduleDate
                           ,@StartTime,@EndTime,@DNIS
                           ,@EmployeeURL,@CustomerURL,@DepartmentCode
                           ,@BranchCode,@BranchName,@BranchPhone
                           ,@Type,@Phone,@Mail
                           ,@Subject,@Description,@StatusCode
                           ,GETDATE(),GETDATE())";

            this._dbConnection.Execute(sql, consultSchedule);
        }

        public void UpdateConsultSchedule(ConsultSchedule consultSchedule)
        {
            string sql = @"UPDATE [ConsultSchedule] SET
                           EmployeeID = @EmployeeID
                           ,EmployeeName = @EmployeeName
                           ,CustomerID = @CustomerID
                           ,CustomerName = @CustomerName
                           ,ScheduleDate = @ScheduleDate
                           ,StartTime = @StartTime
                           ,EndTime = @EndTime
                           ,DNIS = @DNIS
                           ,EmployeeURL = @EmployeeURL
                           ,CustomerURL = @CustomerURL
                           ,DepartmentCode = @DepartmentCode
                           ,BranchCode = @BranchCode
                           ,BranchName = @BranchName
                           ,BranchPhone = @BranchPhone
                           ,Type = @Type
                           ,Phone = @Phone
                           ,Mail = @Mail
                           ,Subject = @Subject
                           ,Description = @Description
                           ,StatusCode = @StatusCode
                           ,ModifiedOn = GETDATE()
                           WHERE ScheduleID = @ScheduleID";

            this._dbConnection.Execute(sql, consultSchedule);
        }

        public IList<Calendar> GetCalendar()
        {
            string sql = @"SELECT TOP 30
                           [Date]
                           ,[Week]
                           ,[IsHoliday]
                           ,[Description]
                           ,CONVERT(char(10), [RealDate], 126) [RealDate]
                           FROM [Calendar] WITH (NOLOCK) WHERE [RealDate] > GETDATE()";

            var result = this._dbConnection.Query<Calendar>(sql)?.ToList() ?? new List<Calendar>();

            return result;
        }

        public string GetDNIS(ConsultSchedule consultSchedule)
        {
            var dnis = new List<string>
            {
                "1003",
                "1004",
                "1005",
                "1006",
                "1007"
            };

            var usedDNIS = GetConsultScheduleList()
                .Where(c => c.ScheduleDate == consultSchedule.ScheduleDate
                && c.StartTime == consultSchedule.StartTime
                && c.StatusCode != "3").Select(c => c.DNIS).ToList();

            if (usedDNIS != null && usedDNIS.Count > 0)
            {
                foreach (string d in usedDNIS)
                {
                    if (dnis.Contains(d))
                    {
                        dnis.Remove(d);
                    }
                }
            }

            return dnis.Count > 0 ? dnis[0] : null;
        }

        public bool CheckEmployeeSchedule(ConsultSchedule consultSchedule)
        {
            return GetConsultScheduleList()
                .Any(c => c.ScheduleDate == consultSchedule.ScheduleDate
                && c.StartTime == consultSchedule.StartTime
                && c.EmployeeID == consultSchedule.EmployeeID);
        }
    }
}
