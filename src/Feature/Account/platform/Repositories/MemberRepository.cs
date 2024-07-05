using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using Feature.Wealth.Account.Helpers;
using Feature.Wealth.Account.Models.Api;
using Feature.Wealth.Account.Models.FundTrackList;
using Feature.Wealth.Account.Models.OAuth;
using Foundation.Wealth.Manager;
using log4net;
using Newtonsoft.Json;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Account.Repositories
{
    public class MemberRepository
    {
        private ILog Log { get; } = Logger.Account;

        public bool CheckUserExists(PlatFormEunm platForm, string id)
        {
            bool exists = false;
            string strSql = string.Empty;
            if (platForm == PlatFormEunm.WebBank)
            {
                strSql = @$"SELECT CAST(CASE WHEN EXISTS (SELECT 1 FROM [FCB_Member] WHERE PlatForm=@platForm
                 and PlatFormId = (SELECT PROMOTION_CODE FROM CFMBSEL WHERE CUST_ID = @id)) THEN 1 ELSE 0 END as BIT)";
            }
            else
            {
                strSql = @$"SELECT CAST(CASE WHEN EXISTS (SELECT 1 FROM [FCB_Member] WHERE PlatForm=@platForm
                 and PlatFormId = @id ) THEN 1 ELSE 0 END as BIT)";
            }

            var para = new { platForm = platForm.ToString(), id = id };
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

        public bool CheckAppUserExists(PlatFormEunm platForm, string promotionCOde)
        {
            bool exists = false;
            string strSql = @$"SELECT CAST(CASE WHEN EXISTS (SELECT 1 FROM [FCB_Member] WHERE PlatForm=@platForm
                 and PlatFormId = @promotionCOde) THEN 1 ELSE 0 END as BIT)";
            var para = new { platForm = platForm.ToString(), promotionCOde = promotionCOde };
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

            string strSql = $"INSERT INTO [FCB_Member] (WebBankId,PlatForm,PlatFormId,MemberName,MemberEmail,VideoInfoOpen,ArrivedInfoOpen,StockShowColor,CreateTime) values " +
                $"(@WebBankId,@PlatForm,@PlatFormId,@MemberName,@MemberEmail,@VideoInfoOpen,@ArrivedInfoOpen,@StockShowColor,@Datetime)";
            var para = new
            {
                WebBankId = fcbMemberModel.WebBankId,
                PlatForm = fcbMemberModel.PlatForm.ToString(),
                PlatFormId = fcbMemberModel.PlatFormId,
                VideoInfoOpen = fcbMemberModel.VideoInfoOpen,
                ArrivedInfoOpen = fcbMemberModel.VideoInfoOpen,
                StockShowColor = fcbMemberModel.StockShowColor.ToString(),
                MemberName = fcbMemberModel.MemberName,
                MemberEmail = fcbMemberModel.MemberEmail,
                Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
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

        public bool BindWebBank(PlatFormEunm platForm, string platFormId, string id)
        {
            bool success = false;
            try
            {
                string strSql = $"UPDATE [FCB_Member] Set WebBankId=(Select PROMOTION_CODE From CFMBSEL WHERE CUST_ID = @WebBankId),UpdateTime=@Time WHERE [PlatForm]=@PlatForm and PlatFormId = @platFormId";
                var para = new { WebBankId = id, Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), PlatForm = platForm.ToString(), platFormId = platFormId };
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

        public CIFMember GetWebBankUserInfo(string id)
        {
            CIFMember member = null;
            var strSql = @$"Select
                            a.CIF_CUST_NAME,
                            a.CIF_E_MAIL_ADDRESS,
                            a.CIF_EMP_RISK,
                            a.CIF_AO_EMPNO,
                            b.EmployeeName as CIF_AO_EMPName,
                            b.EmployeeCode as HRIS_EmployeeCode,
                            c.PROMOTION_CODE as CIF_PROMO_CODE
                            FROM [CIF] as a
                            left join [HRIS] as b on CIF_AO_EMPNO = SUBSTRING(EmployeeCode, len(EmployeeCode) -len( CIF_AO_EMPNO) +1 , len(CIF_AO_EMPNO))
                            left join [CFMBSEL] as c on CIF_ID = CUST_ID
                            WHERE CIF_ID = @id ";

            var para = new { id = id };

            try
            {
                member = DbManager.Custom.Execute<CIFMember>(strSql, para, commandType: System.Data.CommandType.Text);
            }
            catch (SqlException ex)
            {

                Log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return member;

        }

        public CIFMember GetAppUserInfo(string promotioncode)
        {
            CIFMember member = null;
            var strSql = @$"Select
                            a.CIF_CUST_NAME,
                            a.CIF_E_MAIL_ADDRESS,
                            a.CIF_EMP_RISK,
                            a.CIF_AO_EMPNO,
                            b.EmployeeName as CIF_AO_EMPName,
                            b.EmployeeCode as HRIS_EmployeeCode,
                            C.PROMOTION_CODE as CIF_PROMO_CODE
                            FROM [CIF] as a
                            left join [HRIS] as b on CIF_AO_EMPNO = SUBSTRING(EmployeeCode, len(EmployeeCode) -len( CIF_AO_EMPNO) +1 , len(CIF_AO_EMPNO))
                            left join [CFMBSEL] as C on CIF_ID = CUST_ID
                            WHERE C.PROMOTION_CODE = @promotioncode ";

            var para = new { promotioncode = promotioncode };

            try
            {
                member = DbManager.Custom.Execute<CIFMember>(strSql, para, commandType: System.Data.CommandType.Text);
            }
            catch (SqlException ex)
            {

                Log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return member;

        }

        public FcbMemberModel GetMemberInfo(PlatFormEunm platFormEunm, string id)
        {
            FcbMemberModel fcbMemberModel = null;
            var strSql = @$"Select
                            A.*,
                            B.CIF_EMP_RISK as Risk,
                            B.CIF_ESTABL_BIRTH_DATE as Birthday,
                            C.EmployeeName as Advisror,
                            C.EmployeeCode as AdvisrorID
                            FROM [FCB_Member] as A
                            left join [CIF] as B on B.CIF_ID = (SELECT CUST_ID FROM CFMBSEL WHERE PROMOTION_CODE = A.WebBankId)
                            left join [HRIS] as C on CIF_AO_EMPNO = SUBSTRING(EmployeeCode, len(EmployeeCode) -len( CIF_AO_EMPNO) +1 , len(CIF_AO_EMPNO))
                            WHERE PlatForm = @Platform and ";

            if (platFormEunm == PlatFormEunm.WebBank)
            {
                strSql += "PlatFormId = (SELECT PROMOTION_CODE FROM CFMBSEL WHERE CUST_ID = @id)";
            }
            else
            {
                strSql += "PlatFormId = @id";
            }

            var para = new { Platform = platFormEunm.ToString(), id = id };
            fcbMemberModel = DbManager.Custom.Execute<FcbMemberModel>(strSql, para, commandType: System.Data.CommandType.Text);

            return fcbMemberModel;
        }


        public FcbMemberModel GetRefreshMemberInfo(PlatFormEunm platFormEunm, string id)
        {
            FcbMemberModel fcbMemberModel = null;
            var strSql = @$"Select
                            A.*,
                            B.CIF_EMP_RISK as Risk,
                            B.CIF_ESTABL_BIRTH_DATE as Birthday,
                            C.EmployeeName as Advisror,
                            C.EmployeeCode as AdvisrorID
                            FROM [FCB_Member] as A
                            left join [CIF] as B on B.CIF_ID = (SELECT CUST_ID FROM CFMBSEL WHERE PROMOTION_CODE = A.WebBankId)
                            left join [HRIS] as C on CIF_AO_EMPNO = SUBSTRING(EmployeeCode, len(EmployeeCode) -len( CIF_AO_EMPNO) +1 , len(CIF_AO_EMPNO))
                            WHERE PlatForm = @Platform and PlatFormId = @id";

            var para = new { Platform = platFormEunm.ToString(), id = id };
            fcbMemberModel = DbManager.Custom.Execute<FcbMemberModel>(strSql, para, commandType: System.Data.CommandType.Text);

            return fcbMemberModel;
        }

        public FcbMemberModel GetAppMemberInfo(PlatFormEunm platFormEunm, string promotionCode)
        {
            FcbMemberModel fcbMemberModel = null;
            var strSql = @$"Select
                            A.*,
                            B.CIF_EMP_RISK as Risk,
                            B.CIF_ESTABL_BIRTH_DATE as Birthday,
                            C.EmployeeName as Advisror,
                            C.EmployeeCode as AdvisrorID
                            FROM [FCB_Member] as A
                            left join [CIF] as B on B.CIF_ID = (SELECT CUST_ID FROM CFMBSEL WHERE PROMOTION_CODE = A.WebBankId)
                            left join [HRIS] as C on CIF_AO_EMPNO = SUBSTRING(EmployeeCode, len(EmployeeCode) -len( CIF_AO_EMPNO) +1 , len(CIF_AO_EMPNO))
                            WHERE PlatForm = @Platform and PlatFormId = @promotionCode";

            var para = new { Platform = platFormEunm.ToString(), promotionCode = promotionCode };
            fcbMemberModel = DbManager.Custom.Execute<FcbMemberModel>(strSql, para, commandType: System.Data.CommandType.Text);

            return fcbMemberModel;
        }

        public List<TrackListModel> GetTrackListFromDb(string id)
        {
            List<TrackListModel> trackLists = new List<TrackListModel>();
            var jsonStr = string.Empty;
            var strSql = $"SELECT TrackList FROM TrackList WHERE PlatFormId= @id";
            var para = new { id = id };
            jsonStr = DbManager.Custom.Execute<string>(strSql, para, commandType: System.Data.CommandType.Text);
            if (string.IsNullOrEmpty(jsonStr))
            {
                jsonStr = "[]";
            }
            trackLists = JsonConvert.DeserializeObject<List<TrackListModel>>(jsonStr);
            return trackLists;

        }

        public bool InSertTrackList(List<TrackListModel> trackLists)
        {
            var success = false;
            try
            {
                var jsonStr = JsonConvert.SerializeObject(trackLists);
                var strSql = @$"Merge TrackList as target  using (select @id) as source (PlatFormId) on (target.PlatFormId = source.PlatFormId)
                      WHEN MATCHED THEN UPDATE SET TrackList = @jsonStr 
                      WHEN NOT MATCHED BY TARGET THEN INSERT (PlatFormId , TrackList ) VALUES (@id, @jsonStr);";
                var para = new { id = FcbMemberHelper.GetMemberPlatFormId(), jsonStr = jsonStr };
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
        public bool SetMemberEmail(string id, string email)
        {
            bool success = false;
            string strSql = $"UPDATE [FCB_Member] Set MemberEmail=@email,UpdateTime =@time WHERE  PlatFormId = @id";
            var para = new { id = id, email = email, time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
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

        public bool SetVideoInfo(string id, bool open)
        {
            bool success = false;
            string strSql = $"UPDATE [FCB_Member] Set VideoInfoOpen=@open,UpdateTime =@time WHERE  PlatFormId = @id";
            var para = new { id = id, open = Convert.ToInt32(open), time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
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
        public bool SetArriedInfo(string id, bool open)
        {
            bool success = false;
            string strSql = $"UPDATE [FCB_Member] Set ArrivedInfoOpen=@open,UpdateTime =@time WHERE  PlatFormId = @id";
            var para = new { id = id, open = open, time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
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
        public bool SetQuoteChangeColor(string id, string colorType)
        {
            bool success = false;
            try
            {
                var type = (QuoteChangeEunm)Enum.Parse(typeof(QuoteChangeEunm), colorType);
                string strSql = $"UPDATE [FCB_Member] Set StockShowColor=@colorType,UpdateTime =@time WHERE  PlatFormId = @id";
                var para = new { id = id, colorType = type.ToString(), time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
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

        public bool SetCommonFunctions(string id, List<string> commons)
        {
            bool success = false;
            try
            {
                var jsonStr = JsonConvert.SerializeObject(commons);
                var strSql = @$"UPDATE [FCB_Member] Set CommonFunction=@jsonStr,UpdateTime=@Time WHERE  PlatFormId = @PlatFormId ;";
                var para = new { jsonStr = jsonStr, Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), PlatFormId = id };
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

        public CommonFuncrionsResp GetCommonFunctions(string id)
        {
            CommonFuncrionsResp commonResp = new();
            try
            {
                var strSql = @$"Select CommonFunction from FCB_Member where PlatFormId=@id ";
                var para = new { id = id };
                var jsonStr = DbManager.Custom.Execute<string>(strSql, para, commandType: System.Data.CommandType.Text);
                if (jsonStr != null)
                {
                    List<string> CommonFunctions = JsonConvert.DeserializeObject<List<string>>(jsonStr);
                    commonResp.Body = GetCommonFunctionsInfo(CommonFunctions);
                }
                else
                {
                    commonResp.Body = Enumerable.Empty<CommonFunctionsModel>().ToList();
                }
            }
            catch (SqlException ex)
            {
                commonResp.StatusCode = HttpStatusCode.InternalServerError;
                commonResp.Message = ex.Message;
                Log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                commonResp.StatusCode = HttpStatusCode.InternalServerError;
                commonResp.Message = ex.Message;
                Log.Error(ex.Message);
            }


            return commonResp;
        }

        public List<CommonFunctionsModel> GetCommonFunctionsInfo(List<string> commonFunctions)
        {
            List<CommonFunctionsModel> commonFunctionsModels = new();

            foreach (var item in commonFunctions)
            {
                CommonFunctionsModel commonFunctionsModel = new();
                var pageItem = ItemUtils.GetItem(item);
                commonFunctionsModel.PageName = pageItem.Fields["Navigation Title"].Value;
                commonFunctionsModel.PageUrl = pageItem.Url();
                commonFunctionsModel.PageGuid = item;
                commonFunctionsModels.Add(commonFunctionsModel);
            }

            return commonFunctionsModels;
        }

        public CommonToolsRespResp GetCommonTools(string id)
        {
            CommonToolsRespResp commonResp = new();
            try
            {
                var strSql = @$"Select CommonFunction from FCB_Member where PlatFormId=@id";
                var para = new { id = id };
                var jsonStr = DbManager.Custom.Execute<string>(strSql, para, commandType: System.Data.CommandType.Text);
                if (jsonStr != null)
                {
                    List<string> CommonFunctions = JsonConvert.DeserializeObject<List<string>>(jsonStr);
                    commonResp.Body = GetCommonFunctionsInfo(CommonFunctions);
                }
                else
                {
                    commonResp.Body = Enumerable.Empty<CommonFunctionsModel>().ToList();
                }
            }
            catch (SqlException ex)
            {
                commonResp.StatusCode = HttpStatusCode.InternalServerError;
                commonResp.Message = ex.Message;
                Log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                commonResp.StatusCode = HttpStatusCode.InternalServerError;
                commonResp.Message = ex.Message;
                Log.Error(ex.Message);
            }

            return commonResp;
        }

        public bool CheckCommonTools(string itemId)
        {
            var commonFuncItem = ItemUtils.GetItem(Templates.CommonFunction.Root.ToString());
            var settings = commonFuncItem?.GetFieldValue(Templates.CommonFunction.Fields.CommonFunctionList)?.Split('|') ?? Array.Empty<string>();
            return settings.Contains(itemId);
        }

        public (bool, bool, CommonFuncrionsResp) SetCommonTools(string itemId, bool isActive)
        {
            var id = FcbMemberHelper.GetMemberPlatFormId();
            var commons = GetCommonFunctions(id);
            var tools = commons.Body.Select(c => c.PageGuid);
            if (isActive && ID.IsID(itemId))
            {
                tools = tools.Append(itemId).Distinct();
            }
            else
            {
                tools = tools.RemoveWhere(i => i == itemId);
            }
            bool success = false;
            bool limit = tools.Count() > 7;
            try
            {
                if (!limit)
                {
                    var strSql = @$"UPDATE [FCB_Member] Set CommonFunction=@jsonStr,UpdateTime=@Time WHERE  PlatFormId = @PlatFormId;";
                    var para = new { jsonStr = JsonConvert.SerializeObject(tools), Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), PlatFormId = id };
                    var affectedRows = DbManager.Custom.ExecuteNonQuery(strSql, para, commandType: System.Data.CommandType.Text);
                    success = affectedRows != 0;
                    commons.Body = GetCommonToolsInfo(tools);
                }
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return (success, limit, commons);
        }

        public IEnumerable<CommonFunctionsModel> GetCommonToolsInfo(IEnumerable<string> itemTools)
        {
            foreach (var item in itemTools)
            {
                var pageItem = ItemUtils.GetItem(item);
                yield return new CommonFunctionsModel
                {
                    PageName = pageItem?.Fields["Navigation Title"].Value,
                    PageUrl = pageItem?.Url(),
                    PageGuid = item
                };
            }
        }

        /// <summary>
        /// 檢查紅綠燈　綠燈:table可以讀取
        /// </summary>
        /// <returns></returns>
        public bool CheckEDHStatus()
        {
            string sql = "SELECT Status_code FROM EDHStatus where Status_item='CDC'";
            string connString = ConfigurationManager.ConnectionStrings["cif"].ConnectionString;

            bool status = false;

            Log.Info("同步Oracle開始　檢查紅綠燈　table : EDHStatus,connection start");
            using (var connection = new OdbcConnection(connString))
            {
                try
                {
                    connection.Open();
                    Log.Info("同步Oracle開始　檢查紅綠燈　table : EDHStatus,command start");
                    using (OdbcCommand command = new OdbcCommand(sql, connection))
                    {
                        Log.Info("同步Oracle開始　檢查紅綠燈　table : EDHStatus,command ExecuteScalar start");
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            string value = result.ToString();
                            status = value == "G";
                        }
                        Log.Info($"同步Oracle開始　檢查紅綠燈　table : EDHStatus,command ExecuteScalar end staus={status}");
                    }
                }
                catch (Exception ex)
                {
                    Log.Info("同步Oracle開始　檢查紅綠燈　table : EDHStatus, exception message:" + ex.ToString());
                }
            }
            return status;
        }

        /// <summary>
        /// 寫入cif資料from oracle
        /// </summary>
        /// <param name="id"></param>
        public void InsertCifFormOracle(string id)
        {
            string odbcConnString = ConfigurationManager.ConnectionStrings["cif"].ConnectionString;
            string sqlConnString = ConfigurationManager.ConnectionStrings["custom"].ConnectionString;

            Log.Info("同步Oracle開始　寫入cif　table : WEA_ODS_CIF_VIEW,Connection start");
            using (var odbcConn = new OdbcConnection(odbcConnString))
            {
                try
                {
                    odbcConn.Open();
                    string query = "SELECT * FROM WEA_ODS_CIF_VIEW WHERE CIF_ID = ?";
                    using (OdbcCommand command = new OdbcCommand(query, odbcConn))
                    {
                        command.Parameters.Add("param", OdbcType.NVarChar).Value = id;
                        using (OdbcDataReader reader = command.ExecuteReader())
                        {
                            Log.Info("同步Oracle開始　寫入cif　table : WEA_ODS_CIF_VIEW, SqlConnection start");
                            using (SqlConnection sqlConnection = new SqlConnection(sqlConnString))
                            {
                                sqlConnection.Open();
                                string insertQuery = $@"INSERT INTO CIF (CIF_ID, CIF_CUST_NAME, CIF_ESTABL_BIRTH_DATE,CIF_CUST_ATTR,
                                                        CIF_TEL_NO1,CIF_TEL_NO3,CIF_E_MAIL_ADDRESS,CIF_CHN_BU,
                                                        CIF_CHN_CR,CIF_AO_EMPNO,CIF_MAIN_BRANCH,CIF_EMP_RISK,
                                                        CIF_EMP_PI_RISK_ATTR,KYC_EXPIR_DATE,CIF_VIP_CODE,
                                                        CIF_RECCONSENT_TYPE,CIF_UNHEALTH_TYPE,CIF_SAL_FLAG,
                                                        CIF_HIGH_ASSET_FLAG,CIF_EXT_DATE)
                                                 VALUES (@CIF_ID, @CIF_CUST_NAME, @CIF_ESTABL_BIRTH_DATE,@CIF_CUST_ATTR,
                                                        @CIF_TEL_NO1,@CIF_TEL_NO3,@CIF_E_MAIL_ADDRESS,@CIF_CHN_BU,
                                                        @CIF_CHN_CR,@CIF_AO_EMPNO,@CIF_MAIN_BRANCH,@CIF_EMP_RISK,
                                                        @CIF_EMP_PI_RISK_ATTR,@KYC_EXPIR_DATE,@CIF_VIP_CODE,
                                                        @CIF_RECCONSENT_TYPE,@CIF_UNHEALTH_TYPE,@CIF_SAL_FLAG,
                                                        @CIF_HIGH_ASSET_FLAG,@CIF_EXT_DATE)";
                                Log.Info("同步Oracle開始　寫入cif　table : WEA_ODS_CIF_VIEW, SqlCommand start");
                                using (SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnection))
                                {
                                    Log.Info("同步Oracle開始　寫入cif　table : WEA_ODS_CIF_VIEW, Sqlreader start");
                                    while (reader.Read())
                                    {
                                        insertCommand.Parameters.AddWithValue("@CIF_ID", reader["CIF_ID"]);
                                        insertCommand.Parameters.AddWithValue("@CIF_CUST_NAME", reader["CIF_CUST_NAME"]);
                                        insertCommand.Parameters.AddWithValue("@CIF_ESTABL_BIRTH_DATE", reader.GetDateTime(reader.GetOrdinal("CIF_ESTABL_BIRTH_DATE")));
                                        insertCommand.Parameters.AddWithValue("@CIF_CUST_ATTR", reader["CIF_CUST_ATTR"]);
                                        insertCommand.Parameters.AddWithValue("@CIF_TEL_NO1", reader["CIF_TEL_NO1"]);
                                        insertCommand.Parameters.AddWithValue("@CIF_TEL_NO3", reader["CIF_TEL_NO3"]);
                                        insertCommand.Parameters.AddWithValue("@CIF_E_MAIL_ADDRESS", reader["CIF_E_MAIL_ADDRESS"]);
                                        insertCommand.Parameters.AddWithValue("@CIF_CHN_BU", reader["CIF_CHN_BU"]);
                                        insertCommand.Parameters.AddWithValue("@CIF_CHN_CR", reader["CIF_CHN_CR"]);
                                        insertCommand.Parameters.AddWithValue("@CIF_AO_EMPNO", reader["CIF_AO_EMPNO"]);
                                        insertCommand.Parameters.AddWithValue("@CIF_MAIN_BRANCH", reader["CIF_MAIN_BRANCH"]);
                                        insertCommand.Parameters.AddWithValue("@CIF_EMP_RISK", reader["CIF_EMP_RISK"]);
                                        insertCommand.Parameters.AddWithValue("@CIF_EMP_PI_RISK_ATTR", reader["CIF_EMP_PI_RISK_ATTR"]);
                                        insertCommand.Parameters.AddWithValue("@KYC_EXPIR_DATE", reader["KYC_EXPIR_DATE"]);
                                        insertCommand.Parameters.AddWithValue("@CIF_VIP_CODE", reader["CIF_VIP_CODE"]);
                                        insertCommand.Parameters.AddWithValue("@CIF_RECCONSENT_TYPE", reader["CIF_RECCONSENT_TYPE"]);
                                        insertCommand.Parameters.AddWithValue("@CIF_UNHEALTH_TYPE", reader["CIF_UNHEALTH_TYPE"]);
                                        insertCommand.Parameters.AddWithValue("@CIF_SAL_FLAG", reader["CIF_SAL_FLAG"]);
                                        insertCommand.Parameters.AddWithValue("@CIF_HIGH_ASSET_FLAG", reader["CIF_HIGH_ASSET_FLAG"]);
                                        insertCommand.Parameters.AddWithValue("@CIF_EXT_DATE", reader["CIF_EXT_DATE"]);
                                        insertCommand.ExecuteNonQuery();
                                        Log.Info($"同步Oracle開始　寫入cif　table : WEA_ODS_CIF_VIEW, Sqlreader data={reader["CIF_ID"]},{reader["CIF_CUST_NAME"]},{reader.GetDateTime(reader.GetOrdinal("CIF_ESTABL_BIRTH_DATE"))}");
                                    }

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Info("同步Oracle table : WEA_ODS_CIF_VIEW To SQL table : CIF, exception message:" + ex.ToString());
                }
            }
        }

        /// <summary>
        /// 寫入數存推薦人資料from oracle
        /// </summary>
        /// <param name="id">身分證號或六碼</param>
        /// <param name="loginBy">個網或app登入</param>
        /// <returns></returns>
        public string InsertCFMBSELFormOracle(string id, string loginBy)
        {
            string odbcConnString = ConfigurationManager.ConnectionStrings["cif"].ConnectionString;
            string sqlConnString = ConfigurationManager.ConnectionStrings["custom"].ConnectionString;
            string userId = string.Empty;
            Log.Info("同步Oracle開始　寫入CFMBSEL　table : CFMBSEL_STG, OdbcConnection start");
            using (var odbcConn = new OdbcConnection(odbcConnString))
            {
                try
                {
                    odbcConn.Open();
                    //個網回傳身分證號
                    string query = "SELECT * FROM CFMBSEL_STG WHERE CUST_ID = ?";
                    if (loginBy.Equals("app", StringComparison.OrdinalIgnoreCase))
                    {
                        //app回傳6碼
                        query = "SELECT * FROM CFMBSEL_STG WHERE PROMOTION_CODE = ?";
                    }
                    using (OdbcCommand command = new OdbcCommand(query, odbcConn))
                    {
                        command.Parameters.Add("param", OdbcType.NVarChar).Value = id;
                        using (OdbcDataReader reader = command.ExecuteReader())
                        {
                            Log.Info("同步Oracle開始　寫入CFMBSEL　table : CFMBSEL_STG, SqlConnection start");
                            using (SqlConnection sqlConnection = new SqlConnection(sqlConnString))
                            {
                                sqlConnection.Open();
                                string insertQuery = "INSERT INTO CFMBSEL (EXT_DATE, CUST_ID, TELLER_CODE,PROMOTION_CODE,LOAD_DATE) VALUES (@Param1, @Param2, @Param3, @Param4, @Param5)";
                                using (SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnection))
                                {
                                    while (reader.Read())
                                    {
                                        insertCommand.Parameters.AddWithValue("@Param1", reader.GetDateTime(reader.GetOrdinal("EXT_DATE")));
                                        insertCommand.Parameters.AddWithValue("@Param2", reader["CUST_ID"]);
                                        insertCommand.Parameters.AddWithValue("@Param3", reader["TELLER_CODE"]);
                                        insertCommand.Parameters.AddWithValue("@Param4", reader["PROMOTION_CODE"]);
                                        insertCommand.Parameters.AddWithValue("@Param5", reader.GetDateTime(reader.GetOrdinal("LOAD_DATE")));
                                        insertCommand.ExecuteNonQuery();
                                        userId = reader["CUST_ID"].ToString();
                                        Log.Info($"同步Oracle開始　寫入CFMBSEL　table : CFMBSEL_STG, Sqlreader data={reader.GetDateTime(reader.GetOrdinal("EXT_DATE"))},{reader["CUST_ID"]},{reader["TELLER_CODE"]},{reader["PROMOTION_CODE"]},{reader.GetDateTime(reader.GetOrdinal("LOAD_DATE"))");
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Info("同步Oracle table : CFMBSEL_STG To SQL table : CFMBSEL, exception message:" + ex.ToString());
                }

            }

            return userId;
        }

    }
}
