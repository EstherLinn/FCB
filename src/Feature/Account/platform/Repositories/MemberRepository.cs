using System;
using System.Data.SqlClient;
using Feature.Wealth.Account.Models.OAuth;
using Foundation.Wealth.Manager;
using log4net;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.Account.Repositories
{
    public class MemberRepository
    {
        private ILog Log { get; } = Logger.Account;

        public bool CheckUserExists(PlatFormEunm platForm, string id)
        {
            bool exists = false;
            string strSql = $"SELECT CAST(CASE WHEN EXISTS (SELECT 1 FROM [dbo].[FCB_Member] WHERE PlatForm=@platForm and PlatFormId = @id) THEN 1 ELSE 0 END as BIT)";
            var para = new { platForm = nameof(platForm), id = id };
            try
            {
                exists = DbManager.Custom.Execute<bool>(strSql, para, commandType: System.Data.CommandType.Text);
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return exists;
        }

        public bool CreateNewMember(FcbMemberModel fcbMemberModel)
        {
            bool success = false;
            if (fcbMemberModel == null)
            {
                return success;
            }

            string strSql = $"INSERT INTO [dbo].[FCB_Member] (PlatForm,PlatFormId,MemberName,MemberEmail,VideoInfoOpen,ArrivedInfoOpen,StockShowColor,UpdateTime) values " +
                $"(@PlatForm,@PlatFormId,@MemberName,@MemberEmail,@VideoInfoOpen,@ArrivedInfoOpen,@StockShowColor,@Datetime)";
            var para = new
            {
                PlatForm = nameof(fcbMemberModel.PlatForm),
                PlatFormId = fcbMemberModel.PlatFormId,
                VideoInfoOpen = fcbMemberModel.VideoInfoOpen,
                ArrivedInfoOpen = fcbMemberModel.VideoInfoOpen,
                StockShowColor = nameof(fcbMemberModel.StockShowColor),
                MemberName = fcbMemberModel.MemberName,
                MemberEmail = fcbMemberModel.MemberEmail,
                Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
            try
            {
                var affectedRows = DbManager.Custom.Execute<int>(strSql, para, commandType: System.Data.CommandType.Text);
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

        public bool BindWebBank(PlatFormEunm platForm, string platFormId, string webBankId)
        {
            bool success = false;
            try
            {
                string strSql = $"UPDATE  [dbo].[FCB_Member] Set WebBankId=@WebBankId WHERE [PlatForm]=@PlatForm and PlatFormId = @id";

                var para = new { WebBankId = webBankId, PlatForm = nameof(platForm), id = platFormId };
                var affectedRows = DbManager.Custom.Execute<int>(strSql, para, commandType: System.Data.CommandType.Text);
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

        public bool SetMemberEmail(string id, string email)
        {
            bool success = false;
            string strSql = $"UPDATE [dbo].[FCB_Member] Set MemberEmail=@email WHERE  PlatFormId = @id";
            var para = new { id = id, email = email };
            try
            {
                var affectedRows = DbManager.Custom.Execute<int>(strSql, para, commandType: System.Data.CommandType.Text);
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

        public bool SetVideoInfo(string id, bool open)
        {
            bool success = false;
            string strSql = $"UPDATE [dbo].[FCB_Member] Set VideoInfoOpen=@open WHERE  PlatFormId = @id";
            var para = new { id = id, open = Convert.ToInt32(open) };
            try
            {
                var affectedRows = DbManager.Custom.Execute<int>(strSql, para, commandType: System.Data.CommandType.Text);
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
        public bool SetArriedInfo(string id, bool open)
        {
            bool success = false;
            string strSql = $"UPDATE [dbo].[FCB_Member] Set ArrivedInfoOpen=@open WHERE  PlatFormId = @id";
            var para = new { id = id, open = open };
            try
            {
                var affectedRows = DbManager.Custom.Execute<int>(strSql, para, commandType: System.Data.CommandType.Text);
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
        public bool SetQuoteChangeColor(string id, string colorType)
        {
            bool success = false;
            try
            {
                var type = (QuoteChangeEunm)Enum.Parse(typeof(QuoteChangeEunm), colorType);
                string strSql = $"UPDATE [dbo].[FCB_Member] Set StockShowColor=@colorType WHERE  PlatFormId = @id";
                var para = new { id = id, colorType = Convert.ToInt32(type) };
                var affectedRows = DbManager.Custom.Execute<int>(strSql, para, commandType: System.Data.CommandType.Text);
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


    }
}
