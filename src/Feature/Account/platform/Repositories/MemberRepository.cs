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
using Dapper;

namespace Feature.Wealth.Account.Repositories
{
    public class MemberRepository
    {
        private ILog Log { get; } = Logger.Account;

        /// <summary>
        /// 檢查會員是否存在
        /// </summary>
        /// <param name="platForm"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckUserExists(PlatFormEunm platForm, string id)
        {
            bool exists = false;
            string strSql = string.Empty;
            int idLength = 100;
            if (platForm == PlatFormEunm.WebBank)
            {
                strSql = @$" Declare @@platForm varchar(10) = @platForm, @@id varchar(33) = @id
                    SELECT CAST(CASE WHEN EXISTS (SELECT 1 FROM [FCB_Member] WHERE PlatForm=@@platForm
                 and PlatFormId = (SELECT PROMOTION_CODE FROM CFMBSEL WHERE CUST_ID = @@id )) THEN 1 ELSE 0 END as BIT)";
                idLength = 33;
            }
            else
            {
                strSql = @$" Declare @@platForm varchar(10) = @platForm, @@id varchar(100) = @id
                SELECT CAST(CASE WHEN EXISTS (SELECT 1 FROM [FCB_Member] WHERE PlatForm=@@platForm
                 and PlatFormId = @@id ) THEN 1 ELSE 0 END as BIT)
                ";

            }
            var para = new
            {
                platForm = new DbString() { Value = platForm.ToString(), Length = 10 },
                id = new DbString() { Value = id.PadRight(idLength == 33 ? 33 : 0), IsAnsi = true, Length = idLength }
            };

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

        /// <summary>
        /// 檢查App會員是否存在,app使用promotionCode
        /// </summary>
        /// <param name="platForm"></param>
        /// <param name="promotionCode"></param>
        /// <returns></returns>
        public bool CheckAppUserExists(PlatFormEunm platForm, string promotionCode)
        {
            bool exists = false;
            string strSql = @$" Declare @@platForm varchar(10) = @platForm, @@promotionCode varchar(100) = @promotionCode
                 SELECT CAST(CASE WHEN EXISTS (SELECT 1 FROM [FCB_Member] WHERE PlatForm=@@platForm
                 and PlatFormId = @@promotionCode) THEN 1 ELSE 0 END as BIT)";
            var para = new
            {
                platForm = new DbString() { Value = platForm.ToString(), Length = 10 },
                promotionCode = new DbString() { Value = promotionCode, Length = 100 }
            };
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

        /// <summary>
        /// 創建會員
        /// </summary>
        /// <param name="fcbMemberModel"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 綁定網銀
        /// </summary>
        /// <param name="platForm"></param>
        /// <param name="platFormId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool BindWebBank(PlatFormEunm platForm, string platFormId, string id)
        {
            bool success = false;
            try
            {
                string strSql = $@" Declare @@WebBankId varchar(33) = @WebBankId,
                                            @@Time datetime = @Time,
                                            @@platForm varchar(10) = @platForm,
                                            @@platFormId varchar(100) = @platFormId
                                    UPDATE [FCB_Member] Set WebBankId=(Select PROMOTION_CODE From CFMBSEL WHERE CUST_ID = @@WebBankId),
                                    UpdateTime=@@Time WHERE [PlatForm]=@@PlatForm and PlatFormId = @@platFormId";

                var para = new
                {
                    WebBankId = new DbString() { Value = id.PadRight(33), IsAnsi = true, Length = 33 },
                    Time = new DbString() { Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Length = 30 },
                    PlatForm = new DbString() { Value = platForm.ToString(), Length = 10 },
                    platFormId = new DbString() { Value = platFormId, Length = 100 }
                };
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

        /// <summary>
        /// 取得個網CIF資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CIFMember GetWebBankUserInfo(string id)
        {
            CIFMember member = null;
            var strSql = @$" Declare @@id varchar(33) = @id
                            Select
                            a.CIF_CUST_NAME,
                            a.CIF_E_MAIL_ADDRESS,
                            a.CIF_ESTABL_BIRTH_DATE,
                            SUBSTRING(a.CIF_EMP_RISK,1,1) as CIF_EMP_RISK,
                            a.CIF_AO_EMPNO,
                            b.EmployeeName as CIF_AO_EMPName,
                            b.EmployeeCode as HRIS_EmployeeCode,
                            c.PROMOTION_CODE as CIF_PROMO_CODE
                            FROM [CIF] as a
                            left join [HRIS] as b on CIF_AO_EMPNO = SUBSTRING(EmployeeCode, len(EmployeeCode) -len( CIF_AO_EMPNO) +1 , len(CIF_AO_EMPNO))
                            left join [CFMBSEL] as c on CIF_ID = CUST_ID
                            WHERE CIF_ID = @@id ";

            var para = new
            {
                id = new DbString() { Value = id, IsAnsi = true, Length = 33 },
            };
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

        /// <summary>
        /// 取得APP CIF資料,App使用promotionCode
        /// </summary>
        /// <param name="promotionCode"></param>
        /// <returns></returns>
        public CIFMember GetAppUserInfo(string promotionCode)
        {
            CIFMember member = null;
            var strSql = @$" Declare @@promotionCode varchar(24) = @promotionCode
                            Select
                            a.CIF_CUST_NAME,
                            a.CIF_E_MAIL_ADDRESS,
                            a.CIF_ESTABL_BIRTH_DATE,
                            SUBSTRING(a.CIF_EMP_RISK,1,1) as CIF_EMP_RISK,
                            a.CIF_AO_EMPNO,
                            b.EmployeeName as CIF_AO_EMPName,
                            b.EmployeeCode as HRIS_EmployeeCode,
                            C.PROMOTION_CODE as CIF_PROMO_CODE
                            FROM [CIF] as a
                            left join [HRIS] as b on CIF_AO_EMPNO = SUBSTRING(EmployeeCode, len(EmployeeCode) -len( CIF_AO_EMPNO) +1 , len(CIF_AO_EMPNO))
                            left join [CFMBSEL] as C on CIF_ID = CUST_ID
                            WHERE C.PROMOTION_CODE = @@promotionCode ";

            var para = new
            {
                promotionCode = new DbString() { Value = promotionCode, Length = 24 },
            };

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

        /// <summary>
        /// 取得會員資料
        /// </summary>
        /// <param name="platFormEunm"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public FcbMemberModel GetMemberInfo(PlatFormEunm platFormEunm, string id)
        {
            FcbMemberModel fcbMemberModel = null;
            int idLength = 100;
            var strSql = @$" Declare @@platForm varchar(10) = @platForm, @@id varchar(100) = @id
                            Select
                            A.*,
                            SUBSTRING(B.CIF_EMP_RISK,1,1) as Risk,
                            B.CIF_ESTABL_BIRTH_DATE as Birthday,
                            C.EmployeeName as Advisror,
                            C.EmployeeCode as AdvisrorID
                            FROM [FCB_Member] as A
                            left join [CIF] as B on B.CIF_ID = (SELECT CUST_ID FROM CFMBSEL WHERE PROMOTION_CODE = A.WebBankId)
                            left join [HRIS] as C on CIF_AO_EMPNO = SUBSTRING(EmployeeCode, len(EmployeeCode) -len( CIF_AO_EMPNO) +1 , len(CIF_AO_EMPNO))
                            WHERE PlatForm = @@Platform and ";

            if (platFormEunm == PlatFormEunm.WebBank)
            {
                idLength = 33;
                strSql += "PlatFormId = (SELECT PROMOTION_CODE FROM CFMBSEL WHERE CUST_ID = @@id)";
                strSql.Replace("varchar(100)", "varchar(33)");
                id = id.PadRight(idLength);
            }
            else
            {
                strSql += "PlatFormId = @@id";
            }
            var para = new
            {
                Platform = new DbString() { Value = platFormEunm.ToString(), Length = 10 },
                id = new DbString() { Value = id, IsAnsi = true, Length = idLength },
            };
            fcbMemberModel = DbManager.Custom.Execute<FcbMemberModel>(strSql, para, commandType: System.Data.CommandType.Text);

            return fcbMemberModel;
        }

        /// <summary>
        /// 重整會員資料
        /// </summary>
        /// <param name="platFormEunm"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public FcbMemberModel GetRefreshMemberInfo(PlatFormEunm platFormEunm, string id)
        {
            FcbMemberModel fcbMemberModel = null;
            var strSql = @$" Declare @@platForm varchar(10) = @platForm, @@id varchar(100) = @id
                            Select
                            A.*,
                            SUBSTRING(B.CIF_EMP_RISK,1,1) as Risk,
                            B.CIF_ESTABL_BIRTH_DATE as Birthday,
                            C.EmployeeName as Advisror,
                            C.EmployeeCode as AdvisrorID
                            FROM [FCB_Member] as A
                            left join [CIF] as B on B.CIF_ID = (SELECT CUST_ID FROM CFMBSEL WHERE PROMOTION_CODE = A.WebBankId)
                            left join [HRIS] as C on CIF_AO_EMPNO = SUBSTRING(EmployeeCode, len(EmployeeCode) -len( CIF_AO_EMPNO) +1 , len(CIF_AO_EMPNO))
                            WHERE PlatForm = @@Platform and PlatFormId = @@id";

            var para = new
            {
                Platform = new DbString() { Value = platFormEunm.ToString(), Length = 10 },
                id = new DbString() { Value = id, IsAnsi = true, Length = 100 },
            };
            fcbMemberModel = DbManager.Custom.Execute<FcbMemberModel>(strSql, para, commandType: System.Data.CommandType.Text);

            return fcbMemberModel;
        }

        /// <summary>
        /// 取得會員資料,App使用promotionCode
        /// </summary>
        /// <param name="platFormEunm"></param>
        /// <param name="promotionCode"></param>
        /// <returns></returns>
        public FcbMemberModel GetAppMemberInfo(PlatFormEunm platFormEunm, string promotionCode)
        {
            FcbMemberModel fcbMemberModel = null;
            var strSql = @$" Declare @@platForm varchar(10) = @platForm, @@promotionCode varchar(100) = @promotionCode
                            Select
                            A.*,
                            SUBSTRING(B.CIF_EMP_RISK,1,1) as Risk,
                            B.CIF_ESTABL_BIRTH_DATE as Birthday,
                            C.EmployeeName as Advisror,
                            C.EmployeeCode as AdvisrorID
                            FROM [FCB_Member] as A
                            left join [CIF] as B on B.CIF_ID = (SELECT CUST_ID FROM CFMBSEL WHERE PROMOTION_CODE = A.WebBankId)
                            left join [HRIS] as C on CIF_AO_EMPNO = SUBSTRING(EmployeeCode, len(EmployeeCode) -len( CIF_AO_EMPNO) +1 , len(CIF_AO_EMPNO))
                            WHERE PlatForm = @@Platform and PlatFormId = @@promotionCode";

            var para = new
            {
                Platform = new DbString() { Value = platFormEunm.ToString(), Length = 10 },
                promotionCode = new DbString() { Value = promotionCode, IsAnsi = true, Length = 100 },
            };
            fcbMemberModel = DbManager.Custom.Execute<FcbMemberModel>(strSql, para, commandType: System.Data.CommandType.Text);

            return fcbMemberModel;
        }

        /// <summary>
        /// 取得理財網關注清單
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<TrackListModel> GetTrackListFromDb(string id)
        {
            List<TrackListModel> trackLists = new List<TrackListModel>();
            var jsonStr = string.Empty;
            var strSql = $"SELECT TrackList FROM TrackList WHERE PlatFormId= @id";
            var para = new { id };
            jsonStr = DbManager.Custom.Execute<string>(strSql, para, commandType: System.Data.CommandType.Text);
            if (string.IsNullOrEmpty(jsonStr))
            {
                jsonStr = "[]";
            }
            trackLists = JsonConvert.DeserializeObject<List<TrackListModel>>(jsonStr);
            return trackLists;

        }
        /// <summary>
        /// 加入關注清單
        /// </summary>
        /// <param name="trackLists"></param>
        /// <returns></returns>
        public bool InSertTrackList(List<TrackListModel> trackLists)
        {
            var success = false;
            try
            {
                var jsonStr = JsonConvert.SerializeObject(trackLists);
                var strSql = @$"Merge TrackList as target  using (select @id) as source (PlatFormId) on (target.PlatFormId = source.PlatFormId)
                      WHEN MATCHED THEN UPDATE SET TrackList = @jsonStr 
                      WHEN NOT MATCHED BY TARGET THEN INSERT (PlatFormId , TrackList ) VALUES (@id, @jsonStr);";
                var para = new { id = FcbMemberHelper.GetMemberPlatFormId(), jsonStr };
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
        /// <summary>
        /// 設定會員Email
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool SetMemberEmail(string id, string email)
        {
            bool success = false;
            string strSql = $"UPDATE [FCB_Member] Set MemberEmail=@email,UpdateTime =@time WHERE  PlatFormId = @id";
            var para = new { id, email, time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
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
        /// <summary>
        /// 設定理財視訊通知
        /// </summary>
        /// <param name="id"></param>
        /// <param name="open"></param>
        /// <returns></returns>
        public bool SetVideoInfo(string id, bool open)
        {
            bool success = false;
            string strSql = $"UPDATE [FCB_Member] Set VideoInfoOpen=@open,UpdateTime =@time WHERE  PlatFormId = @id";
            var para = new { id, open, time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
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
        /// <summary>
        /// 設定到價通知
        /// </summary>
        /// <param name="id"></param>
        /// <param name="open"></param>
        /// <returns></returns>
        public bool SetArriedInfo(string id, bool open)
        {
            bool success = false;
            string strSql = $"UPDATE [FCB_Member] Set ArrivedInfoOpen=@open,UpdateTime =@time WHERE  PlatFormId = @id";
            var para = new { id, open, time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
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
        /// <summary>
        /// 設定漲跌顏色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="colorType"></param>
        /// <returns></returns>
        public bool SetQuoteChangeColor(string id, string colorType)
        {
            bool success = false;
            try
            {
                var type = (QuoteChangeEunm)Enum.Parse(typeof(QuoteChangeEunm), colorType);
                string strSql = $"UPDATE [FCB_Member] Set StockShowColor=@colorType,UpdateTime =@time WHERE  PlatFormId = @id";
                var para = new { id, colorType = type.ToString(), time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
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
        /// <summary>
        /// 設定常用功能
        /// </summary>
        /// <param name="id"></param>
        /// <param name="commons"></param>
        /// <returns></returns>
        public bool SetCommonFunctions(string id, List<string> commons)
        {
            bool success = false;
            try
            {
                var jsonStr = JsonConvert.SerializeObject(commons);
                var strSql = @$"UPDATE [FCB_Member] Set CommonFunction=@jsonStr,UpdateTime=@Time WHERE  PlatFormId = @PlatFormId ;";
                var para = new { jsonStr, Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), PlatFormId = id };
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
        /// <summary>
        /// 取得常用功能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CommonFuncrionsResp GetCommonFunctions(string id)
        {
            CommonFuncrionsResp commonResp = new();
            try
            {
                var strSql = @$"Select CommonFunction from FCB_Member where PlatFormId=@id ";
                var para = new { id };
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

        /// <summary>
        /// 取得常用功能資訊(頁面名稱、頁面Url、頁面Guid)
        /// </summary>
        /// <param name="commonFunctions"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 取得常用功能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CommonToolsRespResp GetCommonTools(string id)
        {
            CommonToolsRespResp commonResp = new();
            try
            {
                var strSql = @$"Select CommonFunction from FCB_Member where PlatFormId=@id";
                var para = new { id };
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
        /// <summary>
        /// 檢查常用功能
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public bool CheckCommonTools(string itemId)
        {
            var commonFuncItem = ItemUtils.GetItem(Templates.CommonFunction.Root.ToString());
            var settings = commonFuncItem?.GetFieldValue(Templates.CommonFunction.Fields.CommonFunctionList)?.Split('|') ?? Array.Empty<string>();
            return settings.Contains(itemId);
        }
        /// <summary>
        /// 設定常用功能
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 取得常用功能資訊(頁面名稱、頁面Url、頁面Guid)
        /// </summary>
        /// <param name="itemTools"></param>
        /// <returns></returns>
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

    }
}
