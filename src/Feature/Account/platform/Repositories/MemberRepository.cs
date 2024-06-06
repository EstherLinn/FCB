using System;
using System.Collections.Generic;
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

        public bool BindWebBank(PlatFormEunm platForm, string platFormId, string webBankId)
        {
            bool success = false;
            try
            {
                string strSql = $"UPDATE [FCB_Member] Set WebBankId=@WebBankId,UpdateTime=@Time WHERE [PlatForm]=@PlatForm and PlatFormId = @id";

                var para = new { WebBankId = webBankId, Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), PlatForm = platForm.ToString(), id = platFormId };
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
            var strSql = @$"  Select a.CIF_CUST_NAME,a.CIF_E_MAIL_ADDRESS,a.CIF_EMP_RISK,a.CIF_AO_EMPNO,b.EmployeeName as CIF_AO_EMPName,C.PROMOTION_CODE as CIF_PROMO_CODE FROM [CIF]  as a
                        left join [HRIS] as b on CIF_AO_EMPNO = SUBSTRING(EmployeeCode, 3, len(EmployeeCode - 3))
                        left join [CFMBSEL] as C on CIF_ID_NO = CUST_ID
                        WHERE CIF_ID_NO = id ";
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
            var strSql = @$"  Select a.CIF_CUST_NAME,a.CIF_E_MAIL_ADDRESS,a.CIF_EMP_RISK,a.CIF_AO_EMPNO,b.EmployeeName as CIF_AO_EMPName,C.PROMOTION_CODE as CIF_PROMO_CODE FROM [CIF]  as a
                        left join [HRIS] as b on CIF_AO_EMPNO = SUBSTRING(EmployeeCode, 3, len(EmployeeCode - 3))
                        left join [CFMBSEL] as C on CIF_ID_NO = CUST_ID
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
            var strSql = $"  Select A.*,B.CIF_EMP_RISK as Risk,B.CIF_ESTABL_BIRTH_DATE as Birthday,C.EmployeeName as Advisror FROM[FCB_Member] as A" +
                              " left join [CIF] as B on B.CIF_ID_NO = (SELECT CUST_ID FROM CFMBSEL WHERE PROMOTION_CODE = A.WebBankId)" +
                              " left join [HRIS] as C on CIF_AO_EMPNO = SUBSTRING(EmployeeCode, 3, len(EmployeeCode - 3))  " +
                              " WHERE PlatForm = @Platform and ";
            if (platFormEunm == PlatFormEunm.WebBank)
            {
                strSql += "PlatFormId =(SELECT PROMOTION_CODE FROM CFMBSEL WHERE CUST_ID = @id)";
            }
            else
            {
                strSql += "PlatFormId = @id";
            }
            var para = new { Platform = platFormEunm.ToString(), id = id };
            fcbMemberModel = DbManager.Custom.Execute<FcbMemberModel>(strSql, para, commandType: System.Data.CommandType.Text);

            return fcbMemberModel;
        }

        public FcbMemberModel GetAppMemberInfo(PlatFormEunm platFormEunm, string promotionCode)
        {
            FcbMemberModel fcbMemberModel = null;
            var strSql = $"  Select A.*,B.CIF_EMP_RISK as Risk,B.CIF_ESTABL_BIRTH_DATE as Birthday,C.EmployeeName as Advisror FROM[FCB_Member] as A" +
                              " left join [CIF] as B on B.CIF_ID_NO = (SELECT CUST_ID FROM CFMBSEL WHERE PROMOTION_CODE = A.WebBankId)" +
                              " left join [HRIS] as C on CIF_AO_EMPNO = SUBSTRING(EmployeeCode, 3, len(EmployeeCode - 3))  " +
                              " WHERE PlatForm = @Platform and PlatFormId = @promotionCode ";

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

        public (bool, CommonFuncrionsResp) SetCommonTools(string itemId, bool isActive)
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
            try
            {
                var jsonStr = JsonConvert.SerializeObject(tools);
                var strSql = @$"UPDATE [FCB_Member] Set CommonFunction=@jsonStr,UpdateTime=@Time WHERE  PlatFormId = @PlatFormId;";
                var para = new { jsonStr = jsonStr, Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), PlatFormId = id };
                var affectedRows = DbManager.Custom.ExecuteNonQuery(strSql, para, commandType: System.Data.CommandType.Text);
                success = affectedRows != 0;

                commons.Body = GetCommonToolsInfo(tools);
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return (success, commons);
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
    }
}
