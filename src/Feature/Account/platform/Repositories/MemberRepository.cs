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
using Feature.Wealth.Account.Models.MemberLog;
using Foundation.Wealth.Helper;
using Foundation.Wealth.Models;
using Feature.Wealth.Account.Models.MemberCard;
using Microsoft.Owin;
using System.Globalization;
using Xcms.Sitecore.Foundation.Basic.Extensions;

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
            string CFMBSEL = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.CFMBSEL);
            bool exists = false;
            string strSql = string.Empty;
            int idLength = 100;
            if (platForm == PlatFormEunm.WebBank)
            {
                strSql = @$" Declare @@platForm varchar(10) = @platForm, @@id varchar(33) = @id
                    SELECT CAST(CASE WHEN EXISTS (SELECT TOP 1 PlatFormId FROM [FCB_Member] WITH (NOLOCK) WHERE PlatForm=@@platForm
                 and PlatFormId COLLATE Latin1_General_CS_AS = (SELECT TOP 1 PROMOTION_CODE FROM {CFMBSEL} WITH (NOLOCK) WHERE CUST_ID = @@id )) THEN 1 ELSE 0 END as BIT)";
                idLength = 33;
            }
            else
            {
                strSql = @$" Declare @@platForm varchar(10) = @platForm, @@id varchar(100) = @id
                SELECT CAST(CASE WHEN EXISTS (SELECT TOP 1 PlatFormId FROM [FCB_Member] WITH (NOLOCK) WHERE PlatForm=@@platForm
                 and PlatFormId COLLATE Latin1_General_CS_AS = @@id ) THEN 1 ELSE 0 END as BIT)
                ";

            }
            var para = new
            {
                platForm = new DbString() { Value = platForm.ToString(), Length = 10 },
                id = new DbString() { Value = id, IsAnsi = true, Length = idLength }
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
                 SELECT CAST(CASE WHEN EXISTS (SELECT TOP 1 PlatFormId FROM [FCB_Member] WITH (NOLOCK) WHERE PlatForm=@@platForm
                 and PlatFormId COLLATE Latin1_General_CS_AS = @@promotionCode) THEN 1 ELSE 0 END as BIT)";
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
        /// 確認CFMBSEL Table User是否超過一人
        /// </summary>
        /// <param name="PcOrApp">個網或app</param>
        /// <param name="promotionCodeOrId">6碼或id</param>
        /// <returns></returns>
        public bool CheckCFMBSELTableMoreThanOneUser(string PcOrApp, string promotionCodeOrId)
        {
            bool moreThanOne = false;

            string CFMBSEL = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.CFMBSEL);
            //個網登入用id
            if (PcOrApp.ToLower() == "pc")
            {
                string strSql = $@" Declare @@id varchar(33) = @id
                                     Select COUNT(PROMOTION_CODE) From {CFMBSEL} WITH (NOLOCK) WHERE CUST_ID = @@id";
                var para = new
                {
                    id = new DbString() { Value = promotionCodeOrId, IsAnsi = true, Length = 33 },
                };
                var count = DbManager.Custom.ExecuteScalar<int>(strSql, para, commandType: System.Data.CommandType.Text);
                moreThanOne = count > 1;
            }
            else
            {
                //App用6碼
                string strSql = $@" Declare @@promotionCode varchar(24) = @promotionCode
                                     Select COUNT(CUST_ID) From {CFMBSEL} WITH (NOLOCK)  WHERE PROMOTION_CODE COLLATE Latin1_General_CS_AS = @@promotionCode";
                var para = new
                {
                    promotionCode = new DbString() { Value = promotionCodeOrId, IsAnsi = true, Length = 24 },
                };
                var count = DbManager.Custom.ExecuteScalar<int>(strSql, para, commandType: System.Data.CommandType.Text);
                moreThanOne = count > 1;
            }
            return moreThanOne;
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
                                    UpdateTime=@@Time WHERE [PlatForm]=@@PlatForm and PlatFormId  COLLATE Latin1_General_CS_AS = @@platFormId";

                var para = new
                {
                    WebBankId = new DbString() { Value = id, IsAnsi = true, Length = 33 },
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

            string CIF = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.CIF);
            string HRIS = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.HRIS);
            string CFMBSEL = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.CFMBSEL);

            var strSql = @$"DECLARE @@id varchar(33) = @id
                            SELECT
                            A.CIF_CUST_NAME,
                            A.CIF_CUST_ATTR,
                            A.CIF_E_MAIL_ADDRESS,
                            A.CIF_ESTABL_BIRTH_DATE,
                            A.CIF_SAL_FLAG,
                            SUBSTRING(A.CIF_EMP_RISK,1,1) AS CIF_EMP_RISK,
                            A.CIF_AO_EMPNO,
                            A.CIF_KYC_EXPIR_DATE,
                            A.CIF_EMP_PI_RISK_ATTR,
                            A.CIF_HIGH_ASSET_FLAG,
                            A.CIF_HIGH_ASSET_VAL_DATE,
                            B.EmployeeName AS CIF_AO_EMPName,
                            B.EmployeeCode AS HRIS_EmployeeCode,
                            C.PROMOTION_CODE AS CIF_PROMO_CODE,
                            A.CIF_ID,
                            A.CIF_MAIN_BRANCH
                            FROM {CIF} AS A WITH (NOLOCK)
                            LEFT JOIN {HRIS} AS B WITH (NOLOCK) ON RIGHT(REPLICATE('0', 8) + CAST(A.[CIF_AO_EMPNO] AS VARCHAR(8)),8) = B.EmployeeCode
                            LEFT JOIN {CFMBSEL} AS C WITH (NOLOCK) ON CIF_ID = CUST_ID
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

            if (member != null && string.IsNullOrEmpty(member.CIF_ID) == false)
            {
                var employeeID = member.CIF_ID.Substring(0, 10);
                var employee = GetEmployeeInfoByEmployeeID(employeeID);

                if (employee != null)
                {
                    member.IsEmployee = employee.IsEmployee;
                    member.IsManager = employee.IsManager;

                    if (employee.IsEmployee || employee.IsManager)
                    {
                        member.CIF_AO_EMPNO = employee.EmployeeCode;
                        member.CIF_AO_EMPName = employee.EmployeeName;
                    }
                }
            }
            return CheckUserIdentity(member);
        }

        /// <summary>
        /// 取得APP CIF資料,App使用promotionCode
        /// </summary>
        /// <param name="promotionCode"></param>
        /// <returns></returns>
        public CIFMember GetAppUserInfo(string promotionCode)
        {
            CIFMember member = null;
            string CIF = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.CIF);
            string HRIS = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.HRIS);
            string CFMBSEL = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.CFMBSEL);
            var strSql = @$"DECLARE @@promotionCode varchar(24) = @promotionCode
                            SELECT
                            A.CIF_CUST_NAME,
                            A.CIF_CUST_ATTR,
                            A.CIF_E_MAIL_ADDRESS,
                            A.CIF_ESTABL_BIRTH_DATE,
                            A.CIF_SAL_FLAG,
                            SUBSTRING(A.CIF_EMP_RISK,1,1) AS CIF_EMP_RISK,
                            A.CIF_AO_EMPNO,
                            A.CIF_KYC_EXPIR_DATE,
                            A.CIF_EMP_PI_RISK_ATTR,
                            A.CIF_HIGH_ASSET_FLAG,
                            A.CIF_HIGH_ASSET_VAL_DATE,
                            B.EmployeeName AS CIF_AO_EMPName,
                            B.EmployeeCode AS HRIS_EmployeeCode,
                            C.PROMOTION_CODE AS CIF_PROMO_CODE,
                            A.CIF_ID,
                            A.CIF_MAIN_BRANCH
                            FROM {CIF} AS A WITH (NOLOCK)
                            LEFT JOIN {HRIS} AS B WITH (NOLOCK) ON RIGHT(REPLICATE('0', 8) + CAST(A.[CIF_AO_EMPNO] AS VARCHAR(8)),8) = B.EmployeeCode
                            LEFT JOIN {CFMBSEL} AS C WITH (NOLOCK) ON CIF_ID = CUST_ID
                            WHERE C.PROMOTION_CODE COLLATE Latin1_General_CS_AS = @@promotionCode ";

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

            if (member != null && string.IsNullOrEmpty(member.CIF_ID) == false)
            {
                var employeeID = member.CIF_ID.Substring(0, 10);
                var employee = GetEmployeeInfoByEmployeeID(employeeID);

                if (employee != null)
                {
                    member.IsEmployee = employee.IsEmployee;
                    member.IsManager = employee.IsManager;

                    if (employee.IsEmployee || employee.IsManager)
                    {
                        member.CIF_AO_EMPNO = employee.EmployeeCode;
                        member.CIF_AO_EMPName = employee.EmployeeName;
                    }
                }
            }
            //return member;
            return CheckUserIdentity(member);

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
            string CIF = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.CIF);
            string HRIS = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.HRIS);
            var strSql = @$"DECLARE @@platForm varchar(10) = @platForm, @@id varchar(100) = @id
                            SELECT
                            A.*,
                            SUBSTRING(B.CIF_EMP_RISK,1,1) AS Risk,
                            B.CIF_ESTABL_BIRTH_DATE AS Birthday,
                            B.CIF_CUST_ATTR AS Gender,
                            B.CIF_SAL_FLAG AS SalFlag,
                            C.EmployeeName AS Advisror,
                            C.EmployeeCode AS AdvisrorID,
                            B.CIF_ID,
                            B.CIF_MAIN_BRANCH AS MainBranchCode,
                            B.CIF_KYC_EXPIR_DATE,
                            B.CIF_EMP_PI_RISK_ATTR,
                            B.CIF_HIGH_ASSET_FLAG,
                            B.CIF_HIGH_ASSET_VAL_DATE
                            FROM [FCB_Member]  AS A WITH (NOLOCK)
                            LEFT JOIN {CIF} AS B WITH (NOLOCK) ON B.CIF_ID = (SELECT TOP 1 CUST_ID FROM CFMBSEL WHERE PROMOTION_CODE COLLATE Latin1_General_CS_AS = A.WebBankId)
                            LEFT JOIN {HRIS} AS C WITH (NOLOCK) ON RIGHT(REPLICATE('0', 8) + CAST(B.[CIF_AO_EMPNO] AS VARCHAR(8)),8) = C.EmployeeCode
                            WHERE PlatForm = @@Platform AND ";

            if (platFormEunm == PlatFormEunm.WebBank)
            {
                idLength = 33;
                strSql += "PlatFormId COLLATE Latin1_General_CS_AS = (SELECT TOP 1 PROMOTION_CODE FROM CFMBSEL WHERE CUST_ID = @@id)";
                strSql.Replace("varchar(100)", "varchar(33)");
            }
            else
            {
                strSql += "PlatFormId COLLATE Latin1_General_CS_AS = @@id";
            }

            var para = new
            {
                Platform = new DbString() { Value = platFormEunm.ToString(), Length = 10 },
                id = new DbString() { Value = id, IsAnsi = true, Length = idLength },
            };

            fcbMemberModel = DbManager.Custom.Execute<FcbMemberModel>(strSql, para, commandType: System.Data.CommandType.Text);

            if (fcbMemberModel != null && string.IsNullOrEmpty(fcbMemberModel.CIF_ID) == false)
            {
                var employeeID = fcbMemberModel.CIF_ID.Substring(0, 10);
                var employee = GetEmployeeInfoByEmployeeID(employeeID);

                if (employee != null)
                {
                    fcbMemberModel.IsEmployee = employee.IsEmployee;
                    fcbMemberModel.IsManager = employee.IsManager;

                    if (employee.IsEmployee || employee.IsManager)
                    {
                        fcbMemberModel.AdvisrorID = employee.EmployeeCode;
                        fcbMemberModel.Advisror = employee.EmployeeName;
                    }
                }

                // FcbMemberModel 不要保留 CIF_ID
                fcbMemberModel.CIF_ID = string.Empty;
            }
            return CheckUserIdentity(fcbMemberModel);
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
            string CIF = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.CIF);
            string HRIS = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.HRIS);
            var strSql = @$"DECLARE @@platForm varchar(10) = @platForm, @@id varchar(100) = @id
                            SELECT
                            A.*,
                            SUBSTRING(B.CIF_EMP_RISK,1,1) AS Risk,
                            B.CIF_ESTABL_BIRTH_DATE AS Birthday,
                            B.CIF_CUST_ATTR AS Gender,
                            B.CIF_SAL_FLAG AS SalFlag,
                            C.EmployeeName AS Advisror,
                            C.EmployeeCode AS AdvisrorID,
                            B.CIF_ID,
                            B.CIF_MAIN_BRANCH AS MainBranchCode,
                            B.CIF_KYC_EXPIR_DATE,
                            B.CIF_EMP_PI_RISK_ATTR,
                            B.CIF_HIGH_ASSET_FLAG,
                            B.CIF_HIGH_ASSET_VAL_DATE
                            FROM [FCB_Member] AS A WITH (NOLOCK)
                            LEFT JOIN {CIF} AS B WITH (NOLOCK) ON B.CIF_ID = (SELECT TOP 1 CUST_ID FROM CFMBSEL WHERE PROMOTION_CODE COLLATE Latin1_General_CS_AS = A.WebBankId)
                            LEFT JOIN {HRIS} AS C WITH (NOLOCK) ON RIGHT(REPLICATE('0', 8) + CAST(B.[CIF_AO_EMPNO] AS VARCHAR(8)),8) = C.EmployeeCode
                            WHERE PlatForm = @@Platform AND PlatFormId COLLATE Latin1_General_CS_AS = @@id";

            var para = new
            {
                Platform = new DbString() { Value = platFormEunm.ToString(), Length = 10 },
                id = new DbString() { Value = id, IsAnsi = true, Length = 100 },
            };

            fcbMemberModel = DbManager.Custom.Execute<FcbMemberModel>(strSql, para, commandType: System.Data.CommandType.Text);

            if (fcbMemberModel != null && string.IsNullOrEmpty(fcbMemberModel.CIF_ID) == false)
            {
                var employeeID = fcbMemberModel.CIF_ID.Substring(0, 10);
                var employee = GetEmployeeInfoByEmployeeID(employeeID);

                if (employee != null)
                {
                    fcbMemberModel.IsEmployee = employee.IsEmployee;
                    fcbMemberModel.IsManager = employee.IsManager;

                    if (employee.IsEmployee || employee.IsManager)
                    {
                        fcbMemberModel.AdvisrorID = employee.EmployeeCode;
                        fcbMemberModel.Advisror = employee.EmployeeName;
                    }
                }

                // FcbMemberModel 不要保留 CIF_ID
                fcbMemberModel.CIF_ID = string.Empty;
            }

            return CheckUserIdentity(fcbMemberModel);
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
            string CIF = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.CIF);
            string HRIS = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.HRIS);
            var strSql = @$"DECLARE @@platForm varchar(10) = @platForm, @@promotionCode varchar(100) = @promotionCode
                            SELECT
                            A.*,
                            SUBSTRING(B.CIF_EMP_RISK,1,1) AS Risk,
                            B.CIF_ESTABL_BIRTH_DATE AS Birthday,
                            B.CIF_CUST_ATTR AS Gender,
                            B.CIF_SAL_FLAG AS SalFlag,
                            C.EmployeeName AS Advisror,
                            C.EmployeeCode AS AdvisrorID,
                            B.CIF_ID,
                            B.CIF_MAIN_BRANCH AS MainBranchCode,
                            B.CIF_KYC_EXPIR_DATE,
                            B.CIF_EMP_PI_RISK_ATTR,
                            B.CIF_HIGH_ASSET_FLAG,
                            B.CIF_HIGH_ASSET_VAL_DATE
                            FROM [FCB_Member] AS A WITH (NOLOCK)
                            LEFT JOIN {CIF} AS B WITH (NOLOCK) ON B.CIF_ID = (SELECT TOP 1 CUST_ID FROM CFMBSEL WHERE PROMOTION_CODE COLLATE Latin1_General_CS_AS = A.WebBankId)
                            LEFT JOIN {HRIS} AS C WITH (NOLOCK) ON RIGHT(REPLICATE('0', 8) + CAST(B.[CIF_AO_EMPNO] AS VARCHAR(8)),8) = C.EmployeeCode
                            WHERE PlatForm = @@Platform AND PlatFormId COLLATE Latin1_General_CS_AS = @@promotionCode";

            var para = new
            {
                Platform = new DbString() { Value = platFormEunm.ToString(), Length = 10 },
                promotionCode = new DbString() { Value = promotionCode, IsAnsi = true, Length = 100 },
            };

            fcbMemberModel = DbManager.Custom.Execute<FcbMemberModel>(strSql, para, commandType: System.Data.CommandType.Text);

            if (fcbMemberModel != null && string.IsNullOrEmpty(fcbMemberModel.CIF_ID) == false)
            {
                var employeeID = fcbMemberModel.CIF_ID.Substring(0, 10);
                var employee = GetEmployeeInfoByEmployeeID(employeeID);

                if (employee != null)
                {
                    fcbMemberModel.IsEmployee = employee.IsEmployee;
                    fcbMemberModel.IsManager = employee.IsManager;

                    if (employee.IsEmployee || employee.IsManager)
                    {
                        fcbMemberModel.AdvisrorID = employee.EmployeeCode;
                        fcbMemberModel.Advisror = employee.EmployeeName;
                    }
                }

                // FcbMemberModel 不要保留 CIF_ID
                fcbMemberModel.CIF_ID = string.Empty;
            }

            return CheckUserIdentity(fcbMemberModel);
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
            var strSql = $"SELECT TrackList FROM TrackList WHERE PlatFormId  COLLATE Latin1_General_CS_AS = @id";
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
                var strSql = @$"Merge TrackList as target  using (select @id COLLATE Latin1_General_CS_AS) as source (PlatFormId) on (target.PlatFormId = source.PlatFormId)
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
            string strSql = $"UPDATE [FCB_Member] Set MemberEmail=@email,UpdateTime =@time WHERE PlatFormId COLLATE Latin1_General_CS_AS = @id";
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
            string strSql = $"UPDATE [FCB_Member] Set VideoInfoOpen=@open,UpdateTime =@time WHERE PlatFormId COLLATE Latin1_General_CS_AS = @id";
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
            string strSql = $"UPDATE [FCB_Member] Set ArrivedInfoOpen=@open,UpdateTime =@time WHERE  PlatFormId  COLLATE Latin1_General_CS_AS = @id";
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
                string strSql = $"UPDATE [FCB_Member] Set StockShowColor=@colorType,UpdateTime =@time WHERE  PlatFormId COLLATE Latin1_General_CS_AS = @id";
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
                var strSql = @$"UPDATE [FCB_Member] Set CommonFunction=@jsonStr,UpdateTime=@Time WHERE  PlatFormId COLLATE Latin1_General_CS_AS = @PlatFormId ;";
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
                var strSql = @$"Select CommonFunction from FCB_Member WITH (NOLOCK) where PlatFormId COLLATE Latin1_General_CS_AS =@id ";
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
                if (CheckCommonTools(item))
                {
                    CommonFunctionsModel commonFunctionsModel = new();
                    var pageItem = ItemUtils.GetItem(item);
                    commonFunctionsModel.PageName = pageItem.Fields["Navigation Title"].Value;
                    commonFunctionsModel.PageUrl = pageItem.Url();
                    commonFunctionsModel.PageGuid = item;
                    commonFunctionsModels.Add(commonFunctionsModel);
                }
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
                var strSql = @$"Select CommonFunction from FCB_Member WITH (NOLOCK) where PlatFormId COLLATE Latin1_General_CS_AS =@id";
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
                    var strSql = @$"UPDATE [FCB_Member] Set CommonFunction=@jsonStr,UpdateTime=@Time WHERE  PlatFormId COLLATE Latin1_General_CS_AS = @PlatFormId;";
                    var para = new { jsonStr = JsonConvert.SerializeObject(tools), Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), PlatFormId = id };
                    var affectedRows = DbManager.Custom.ExecuteNonQuery(strSql, para, commandType: System.Data.CommandType.Text);
                    success = affectedRows != 0;
                    commons.Body = GetCommonToolsInfo(tools);
                    MemberLog memberLog = new MemberLog()
                    {
                        PlatForm = FcbMemberHelper.GetMemberPlatForm().ToString(),
                        PlatFormId = id,
                        Action = ActionEnum.Edit.ToString(),
                        Description = string.Format("修改常用功能: {0}", string.Join(",", tools.ToArray())),
                        ActionTime = Sitecore.DateUtil.ToServerTime(DateTime.UtcNow)
                    };
                    this.RecordMemberActionLog(memberLog);
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

        private string GetMemberId()
        {
            string id = string.Empty;
            string CFMBSEL = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.CFMBSEL);
            var strSql = @$" Declare @@promotionCode varchar(24) = @promotionCode
                             SELECT CUST_ID FROM {CFMBSEL} WITH (NOLOCK) WHERE PROMOTION_CODE　COLLATE Latin1_General_CS_AS = @@promotionCode";

            var para = new
            {
                promotionCode = new DbString() { Value = FcbMemberHelper.GetMemberWebBankId(), IsAnsi = true, Length = 24 },
            };
            id = DbManager.Custom.Execute<string>(strSql, para, commandType: System.Data.CommandType.Text);

            return id;
        }

        /// <summary>
        /// 檢查紅綠燈　綠燈:table可以讀取
        /// </summary>
        /// <returns></returns>
        public bool CheckEDHStatus()
        {
            string sql = "SELECT Status_code FROM EDHStatus where Status_item='CDC'";
            bool status = false;
            Log.Info("Oracle　檢查紅綠燈　table : EDHStatus,connection start");
            try
            {
                using (var connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["cif"].ConnectionString))
                {
                    connection.Open();
                    Log.Info("Oracle 檢查紅綠燈　table : EDHStatus,command start");
                    using (OdbcCommand command = new OdbcCommand(sql, connection))
                    {
                        Log.Info("Oracle 檢查紅綠燈　table : EDHStatus,command ExecuteScalar start");
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            string value = result.ToString();
                            status = value == "G";
                        }
                        Log.Info($"Oracle 檢查紅綠燈　table : EDHStatus,command ExecuteScalar end status={status}");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Info("Oracle開始 檢查紅綠燈 table : EDHStatus, exception message:" + ex.ToString());
            }
            return status;
        }
        /// <summary>
        /// 取得即時cif之風險屬性
        /// </summary>
        /// <returns></returns>
        public string GetCifRiskFormOracle()
        {
            string risk = string.Empty;
            Log.Info("取得CIF即時資料開始: WEA_ODS_CIF_VIEW,OdbcConnection start");
            try
            {
                using (var odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["cif"].ConnectionString))
                {
                    odbcConn.Open();
                    string query = "SELECT CIF_EMP_RISK FROM WEA_ODS_CIF_VIEW WHERE CIF_ID = ?";
                    Log.Info("取得CIF即時資料開始: WEA_ODS_CIF_VIEW,OdbcCommand start");
                    using (OdbcCommand command = new OdbcCommand(query, odbcConn))
                    {
                        command.Parameters.AddWithValue("param", GetMemberId());
                        using (OdbcDataReader reader = command.ExecuteReader())
                        {
                            Log.Info("取得CIF即時資料: WEA_ODS_CIF_VIEW,OdbcDataReader start");
                            while (reader.Read())
                            {
                                risk = reader["CIF_EMP_RISK"].ToString();
                            }
                            reader.Close();
                        }
                        Log.Info($"取得CIF即時資料結束: WEA_ODS_CIF_VIEW,OdbcDataReader end,取得CIF_EMP_RISK = {risk}");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Info("取得CIF即時資料 table : WEA_ODS_CIF_VIEW , exception message:" + ex.ToString());
            }
            return risk;
        }

        /// <summary>
        /// 更新SQL CIF風險屬性欄位
        /// </summary>
        /// <param name="getRisk"></param>
        /// <returns></returns>
        public (bool, string) UpdateCifRiskToSql(string getRisk)
        {
            var success = false;
            var id = GetMemberId();
            if (string.IsNullOrEmpty(getRisk) || string.IsNullOrEmpty(id))
            {
                return (success, string.Empty);
            }
            try
            {
                string sqlStr = @$"declare @@id VARCHAR(33) =@id,@@risk varchar(30) = @risk
                                   update [CIF] set CIF_EMP_RISK =@@risk where [CIF_ID] = @@id ";
                var para = new
                {
                    id = new DbString() { Value = id, IsAnsi = true, Length = 33 },
                    risk = new DbString() { Value = getRisk, Length = 20 }
                };
                var affectedRows = DbManager.Custom.ExecuteNonQuery(sqlStr, para, commandType: System.Data.CommandType.Text);
                success = affectedRows != 0;
            }
            catch (Exception ex)
            {
                Log.Info("更新CIF即時資料 TO SQL table : CIF , exception message:" + ex.ToString());
            }
            return (success, getRisk);
        }
        /// <summary>
        /// 取得會員預約諮詢日期
        /// </summary>
        /// <returns></returns>
        public DateTime? GetMemberScheduleDate()
        {
            DateTime? dt = null;
            var strSql = @$" Select top 1 ScheduleDate From ConsultSchedule where CustomerID  COLLATE Latin1_General_CS_AS =@id and StatusCode = '1' and  ScheduleDate>=CAST(GETDATE() AS DATE)  order by ScheduleDate ";

            var para = new
            {
                id = FcbMemberHelper.GetMemberWebBankId()
            };
            dt = DbManager.Custom.Execute<DateTime?>(strSql, para, commandType: System.Data.CommandType.Text);
            return dt;
        }
        /// <summary>
        /// 取得會員預約諮詢通知
        /// </summary>
        /// <returns></returns>
        public string GetMemberScheduleMessage()
        {
            string msg = string.Empty;
            var strSql = @$"  SELECT [StartTime] FROM [ConsultSchedule]
            where CustomerID  COLLATE Latin1_General_CS_AS =@id and StatusCode = '1'  and ScheduleDate =CAST(GETDATE() AS DATE) and
            CONVERT(TIME, StartTime) > CONVERT(TIME, CONVERT(VARCHAR(5), GETDATE(), 108))
            order by [StartTime] ";
            var para = new
            {
                id = FcbMemberHelper.GetMemberWebBankId()
            };
            var times = DbManager.Custom.ExecuteIList<string>(strSql, para, commandType: System.Data.CommandType.Text)?.ToList();
            if (times != null && times.Any())
            {
                string orderTimes = string.Join("、", times.ToArray());
                msg = string.Format("提醒您，您今日 {0} 有預約理專遠距諮詢，請準時參與！", orderTimes);
            }
            return msg;
        }
        /// <summary>
        /// 取得理顧預約諮詢通知
        /// </summary>
        /// <returns></returns>
        public string GetAdvisrorScheduleMessage()
        {
            string msg = string.Empty;
            var strSql = @$"  SELECT [StartTime] FROM [ConsultSchedule]
            where EmployeeID  COLLATE Latin1_General_CS_AS =@id and StatusCode = '1'  and ScheduleDate =CAST(GETDATE() AS DATE) and
            CONVERT(TIME, StartTime) > CONVERT(TIME, CONVERT(VARCHAR(5), GETDATE(), 108));";
            var para = new
            {
                id = FcbMemberHelper.GetMemberAllInfo().AdvisrorID
            };
            var times = DbManager.Custom.ExecuteIList<string>(strSql, para, commandType: System.Data.CommandType.Text)?.ToList();
            if (times != null && times.Any())
            {
                msg = "提醒您，您今日有客戶預約遠距諮詢，請準時參與！";
            }
            return msg;
        }
        /// <summary>
        /// 取得分行資訊
        /// </summary>
        /// <param name="branchCode">分行代碼</param>
        /// <returns></returns>
        public Branch GetMainBranchInfoByBranchCode(string branchCode)
        {
            var strSql = @$"
                            SELECT
                            BranchCode,
                            BranchName,
                            PhoneAreaCode,        
                            PhoneNumber
                            FROM [Branch_Data] 
                            WHERE BranchCode = @branchCode";
            var branch = DbManager.Custom.Execute<Branch>(strSql, new { branchCode }, commandType: System.Data.CommandType.Text);
            return branch;
        }
        /// <summary>
        /// 取得員工主要分行資訊
        /// </summary>
        /// <param name="employeeCode">員工代號</param>
        /// <returns></returns>
        public Branch GetMainBranchInfoByEmployee(string employeeCode)
        {
            string HRIS = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.HRIS);
            var strSql = @$"
                            SELECT
                            B.BranchCode,
                            B.BranchName,
                            B.PhoneAreaCode,        
                            B.PhoneNumber
                            FROM {HRIS} AS A  WITH (NOLOCK)
                            LEFT JOIN [Branch_Data] AS B  WITH (NOLOCK) ON SUBSTRING(A.OfficeOrBranchCode, 2, 3) = B.BranchCode
                            WHERE A.EmployeeCode = @employeeCode";

            var branch = DbManager.Custom.Execute<Branch>(strSql, new { employeeCode }, commandType: System.Data.CommandType.Text);
            return branch;
        }

        public void RecordMemberActionLog(MemberLog memberLog)
        {
            string strSql = $"INSERT INTO [MemberActionLog] (PlatForm,PlatFormId,Action,Description,ActionTime) values " +
               $"(@PlatForm,@PlatFormId,@Action,@Description,@ActionTime)";
            try
            {
                DbManager.Custom.ExecuteNonQuery(strSql, memberLog, commandType: System.Data.CommandType.Text);
            }
            catch (SqlException ex)
            {
                Log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        public Employee GetEmployeeInfoByEmployeeID(string id)
        {
            Employee member = null;
            string HRIS = TrafficLightHelper.GetTrafficLightTable(NameofTrafficLight.HRIS);
            var strSql = @$"DECLARE @@id varchar(10) = @id
                            SELECT TOP (1)
                            [EmployeeCode]
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
                            ,IIF(D.PersonalFinanceBusinessPersonnelCategory = '2', CONVERT(bit, 1), CONVERT(bit, 0)) AS IsEmployee
                            ,IIF(D.SupervisorCode <> '9' AND D.EmployeeCode IN (SELECT TOP (1) Supervisor FROM HRIS WITH (NOLOCK) WHERE Supervisor = D.EmployeeCode AND PersonalFinanceBusinessPersonnelCategory = '2'), CONVERT(bit, 1), CONVERT(bit, 0)) AS IsManager
                            FROM {HRIS} AS D WITH (NOLOCK)
                            WHERE [EmployeeID] = @@id ";
            try
            {
                var para = new
                {
                    id = new DbString() { Value = id, IsAnsi = true, Length = 10 },
                };

                member = DbManager.Custom.Execute<Employee>(strSql, para, commandType: System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return member;
        }

        /// <summary>
        /// 判斷高資產身分、專業投資人給風險屬性 For CIFMember
        /// </summary>
        /// <returns></returns>
        private CIFMember CheckUserIdentity(CIFMember member)
        {
            if (member == null)
            {
                return null;
            }
            DateTime today = DateTime.Today;
            //判斷是否為高資產客戶&&檢核高資產日期
            if (member.CIF_HIGH_ASSET_FLAG.ToBoolean() && !string.IsNullOrEmpty(member.CIF_HIGH_ASSET_VAL_DATE))
            {
                DateTime highAssetDate;
                bool isHighAssetDate = DateTime.TryParseExact(member.CIF_HIGH_ASSET_VAL_DATE, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out highAssetDate);
                if (isHighAssetDate && highAssetDate >= today)
                {
                    //高資產身分有效
                    member.CIF_EMP_RISK = "3";
                    return member;
                }
            }
            //專業機構投資人 3，不需檢核kyc日期
            if (!string.IsNullOrEmpty(member.CIF_EMP_PI_RISK_ATTR) && member.CIF_EMP_PI_RISK_ATTR != "0" && member.CIF_EMP_PI_RISK_ATTR == "3")
            {
                member.CIF_EMP_RISK = "3";
                return member;
            }
            //判斷風險屬性有效日期，9999/99/99表示未做過kyc
            if (!string.IsNullOrEmpty(member.CIF_EMP_RISK) && !string.IsNullOrEmpty(member.CIF_KYC_EXPIR_DATE) && member.CIF_KYC_EXPIR_DATE != "9999/99/99")
            {
                DateTime kycDate;
                bool isKycValidDate = DateTime.TryParseExact(member.CIF_KYC_EXPIR_DATE, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out kycDate);

                //判斷風險屬性有效
                if (!string.IsNullOrEmpty(member.CIF_EMP_RISK))
                {
                    if (isKycValidDate)
                    {
                        //風險屬性逾期
                        if (kycDate < today)
                        {
                            member.IsKycExpire = true;
                            return member;
                        }
                        //專業投資人1、2、4並且風險屬性有效 
                        if (!string.IsNullOrEmpty(member.CIF_EMP_PI_RISK_ATTR) && member.CIF_EMP_PI_RISK_ATTR != "0" && member.CIF_EMP_PI_RISK_ATTR != "3")
                        {
                            member.CIF_EMP_RISK = "3";
                            return member;
                        }
                    }
                    else
                    {
                        //風險屬性非正常日期格式
                        member.CIF_EMP_RISK = null;
                        return member;
                    }
                }
                else
                {
                    member.IsKycExpire = true;
                    return member;
                }
            }
            return member;
        }

        /// <summary>
        /// 判斷高資產身分、專業投資人給風險屬性 For FcbMemberModel
        /// </summary>
        /// <returns></returns>
        private FcbMemberModel CheckUserIdentity(FcbMemberModel member)
        {
            if (member == null)
            {
                return null;
            }
            DateTime today = DateTime.Today;
            //判斷是否為高資產客戶&&檢核高資產日期
            if (member.CIF_HIGH_ASSET_FLAG.ToBoolean() && !string.IsNullOrEmpty(member.CIF_HIGH_ASSET_VAL_DATE))
            {
                DateTime highAssetDate;
                bool isHighAssetDate = DateTime.TryParseExact(member.CIF_HIGH_ASSET_VAL_DATE, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out highAssetDate);
                if (isHighAssetDate && highAssetDate >= today)
                {
                    //高資產身分有效
                    member.Risk = "3";
                    return member;
                }
            }

            //專業機構投資人 3，不需檢核kyc日期
            if (!string.IsNullOrEmpty(member.CIF_EMP_PI_RISK_ATTR) && member.CIF_EMP_PI_RISK_ATTR != "0" && member.CIF_EMP_PI_RISK_ATTR == "3")
            {
                member.Risk = "3";
                return member;
            }

            //判斷風險屬性有效日期，9999/99/99表示未做過kyc
            if (!string.IsNullOrEmpty(member.Risk) && !string.IsNullOrEmpty(member.CIF_KYC_EXPIR_DATE) && member.CIF_KYC_EXPIR_DATE != "9999/99/99")
            {
                DateTime kycDate;
                bool isKycValidDate = DateTime.TryParseExact(member.CIF_KYC_EXPIR_DATE, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out kycDate);
                if (isKycValidDate)
                {
                    //風險屬性逾期
                    if (kycDate < today)
                    {
                        member.IsKycExpire = true;
                        return member;
                    }
                    //專業投資人1、2、4並且風險屬性有效 
                    if (!string.IsNullOrEmpty(member.CIF_EMP_PI_RISK_ATTR) && member.CIF_EMP_PI_RISK_ATTR != "0" && member.CIF_EMP_PI_RISK_ATTR != "3")
                    {
                        member.Risk = "3";
                        return member;
                    }
                }
                else
                {
                    //風險屬性非正常日期格式
                    member.Risk = null;
                    return member;
                }
            }
            return member;
        }
    }
}
