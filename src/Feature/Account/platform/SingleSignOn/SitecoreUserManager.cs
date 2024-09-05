using Feature.Wealth.Account.Models.OAuth;
using Foundation.Wealth.Manager;
using log4net;
using Sitecore.Security.Accounts;
using Sitecore.SecurityModel;
using System;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.Account.SingleSignOn
{
    public class SitecoreUserManager
    {
        private ILog Log { get; } = Logger.Account;
        public User CreateUser(string userName, string password, string roleName)
        {
            // 創建用戶
            using (new SecurityDisabler())
            {
                User user = User.Create(userName, password);
                return user;
            }
        }

        public void AddUserToRole(string userName, string roleName)
        {
            // 獲取用戶
            User user = User.FromName(userName, false);
            if (user == null)
            {
                throw new Exception($"用戶 '{userName}' 不存在.");
            }

            // 獲取角色
            Role role = Role.FromName(roleName);
            if (role == null)
            {
                throw new Exception($"角色 '{roleName}' 不存在.");
            }

            // 檢查用戶是否已經在角色中
            if (!RolesInRolesManager.IsUserInRole(user, role, true))
            {
                // 將用戶添加到角色
                user.Roles.Add(role);
                user.Profile.Save();
                Sitecore.Diagnostics.Log.Info($"用戶 '{userName}' 已成功添加到角色 '{roleName}'.", this);
            }
            else
            {
                Sitecore.Diagnostics.Log.Info($"用戶 '{userName}' 已經在角色 '{roleName}' 中.", this);
            }
        }

        public void DisableUser(string userName)
        {
            // 獲取用戶
            User user = User.FromName(userName, false);
            if (user == null)
            {
                throw new Exception($"用戶 '{userName}' 不存在.");
            }

            // 檢查用戶是否為系統管理員
            if (user.IsAdministrator)
            {
                throw new Exception("不能停用系統管理員帳號.");
            }

            // 檢查用戶是否為特定帳號
            if (userName.Equals("sitecore\\specificAccount", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("不能停用特定帳號.");
            }

            // 停用用戶
            using (new SecurityDisabler())
            {
                //user.IsApproved = false;
                user.Profile.Save();
                Sitecore.Diagnostics.Log.Info($"用戶 '{userName}' 已成功停用.", this);
            }
        }

        public Employee GetEmployeeInfo(string id)
        {
            Employee member = null;
            var strSql = @$" SELECT TOP (1) [EmployeeCode]
                  ,[EmployeeName]
                  ,[EmployeeID]
                  ,[SubsidiaryCode]
                  ,[BusinessGroup]
                  ,[BusinessName]
                  ,[OfficeOrBranchCode]
                  ,[OfficeOrBranchName]
                  ,[DepartmentCode]
                  ,[DepartmentName]
                  ,[PositionCode]
                  ,[PositionName]
                  ,[JobTitleCode]
                  ,[JobTitleName]
                  ,[JobPositionCode]
                  ,[JobPositionName]
                  ,[Supervisor]
                  ,[AppointmentDate]
                  ,[Attendancebranch]
                  ,[SupervisorCode]
                  ,[SupervisorName]
                  ,[HaveTrustLicense]
                  ,[HaveInsurancseLicense]
                  ,[InvestmentQualifications]
                  ,[DerivativeFinancialOrStructuredQualifications]
                  ,[PersonalFinanceBusinessPersonnelCategory]
                  ,[RegionalCenterBusinessGroup]
                  ,[SalaryLevel]
                  ,[SalaryScale]
              FROM [dbo].[HRIS] WITH (NOLOCK)
            WHERE EmployeeCode = @id ";
            try
            {
                member = DbManager.Custom.Execute<Employee>(strSql, new { id }, commandType: System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return member;

        }
    }
}