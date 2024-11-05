using Dapper;
using Foundation.Wealth.Manager;
using log4net;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Xcms.Sitecore.Foundation.Basic.DbContext;
using Xcms.Sitecore.Foundation.Basic.Extensions;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Xcms.Sitecore.Foundation.Basic.SitecoreExtensions;

namespace Feature.Wealth.Toolkit.Models.TableViewer.SitecoreMemberRecord
{
    public class SitecoreMemberRecordModel
    {
        private readonly ILog _log = Logger.General;

        private void InitSetting()
        {
            this.Path = HttpContext.Current.Request?.Path;
            if (RenderingContext.CurrentOrNull != null)
            {
                var param = WebUtil.ParseUrlParameters(RenderingContext.CurrentOrNull.Rendering["Parameters"]);
                _params[this.Path] = param.ToDictionary();
            }
        }

        public void Init()
        {
            InitSetting();
            this.IsManager = CheckIsManager();
        }

        public void Init(string[][] rules, int iCount, int iOrder, int iOrderColumeName)
        {
            string str_TopCount = string.Empty, ddlOrder = string.Empty, result = string.Empty;

            if (iCount > 0 && iCount < 3001)
            {
                str_TopCount = string.Format("Top {0}", iCount);
            }

            switch (iOrder)
            {
                case 0:
                    ddlOrder = "Desc";
                    break;

                case 1:
                    ddlOrder = "Asc";
                    break;
            }

            string columeName = GetColumeName(iOrderColumeName);

            var sqlParams = new List<SqlParameter>();

            if (rules?.Length > 0)
            {
                string where = UpdateRule(rules, sqlParams);

                if (string.IsNullOrEmpty(where))
                {
                    this.Message = MESSAGE;
                }
                else
                {
                    result = "WHERE " + where;
                }
            }

            if (!string.IsNullOrEmpty(this.Domain))
            {
                result = $"{result}{(string.IsNullOrEmpty(result) ? "WHERE" : " AND")} UPPER([UserName]) LIKE UPPER(@Domain)";
                sqlParams.Add(new SqlParameter("Domain", this.Domain + "\\%"));
            }

            string sqlOrder = !string.IsNullOrEmpty(columeName) ? $" ORDER BY {columeName} {ddlOrder}" : string.Empty;

            this.Count = iCount;
            this.Order = iOrder;
            this.OrderColumeName = columeName;
            this.SqlAuthComm = $@"SELECT {str_TopCount}  [Id], [Action], [UserName], [Created], [FullName], [DepartmentName], [Roles], [IP]
                                  FROM [dbo].[AuthenticationHistory] WITH(NOLOCK) {result} {sqlOrder}";

            this.Dictionary = sqlParams.ToDictionary(x => x.ParameterName, y => y.Value);
            this.IsManager = CheckIsManager();
        }

        /// <summary>
        /// 檢驗是否為管理者
        /// </summary>
        /// <returns></returns>
        private bool CheckIsManager()
        {
            if (!Sitecore.Context.IsLoggedIn)
            {
                return false;
            }

            if (Sitecore.Context.IsAdministrator)
            {
                return true;
            }

            try
            {
                var configItem = ItemUtils.GetItem(ConfigItemID.UserRecord);

                if (configItem == null)
                {
                    return false;
                }

                string[] roles = configItem.GetMultiLineText(Templates.UserRecordConfiguration.Fields.Managers);

                foreach (string roleName in roles)
                {
                    if (Sitecore.Context.User.IsInRole(roleName))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                this._log.Error("檢驗是否為管理者發生錯誤", ex);
            }

            return false;
        }

        private string GetColumeName(int input)
        {
            string columeName = string.Empty;
            switch (input)
            {
                case 1:
                    columeName = "Id";
                    break;

                case 2:
                    columeName = "Action";
                    break;

                case 3:
                    columeName = "UserName";
                    break;

                case 4:
                    columeName = "Created";
                    break;

                case 5:
                    columeName = "FullName";
                    break;

                case 6:
                    columeName = "DepartmentName";
                    break;

                case 7:
                    columeName = "Roles";
                    break;

                case 8:
                    columeName = "IP";
                    break;
            }

            return columeName;
        }

        private string ComparisonOperator(int input)
        {
            string @operator = string.Empty;
            switch (input)
            {
                case 0:
                    @operator = ">=";
                    break;

                case 1:
                    @operator = ">";
                    break;

                case 2:
                    @operator = "=";
                    break;

                case 3:
                    @operator = "<=";
                    break;

                case 4:
                    @operator = "<";
                    break;

                case 5:
                    @operator = "!=";
                    break;

                case 6:
                    @operator = "LIKE";
                    break;

                case 7:
                    @operator = "NOT LIKE";
                    break;

                case 8:
                    @operator = "START WITH";
                    break;

                case 9:
                    @operator = "NOT START WITH";
                    break;

                case 10:
                    @operator = "END WITH";
                    break;

                case 11:
                    @operator = "NOT END WITH";
                    break;

                case 12:
                    @operator = "DISTINCT";
                    break;

                case 13:
                    @operator = "REDUNDENT";
                    break;

                case 14:
                    @operator = "EMPTY";
                    break;

                case 15:
                    @operator = "NOT EMPTY";
                    break;

                case 16:
                    @operator = "END WITH";
                    break;

                case 17:
                    @operator = "VALID ASCII";
                    break;

                case 18:
                    @operator = "INVALID ASCII";
                    break;
            }

            return @operator;
        }

        public IEnumerable<object> GetData(CommandType commandType = CommandType.Text) => GetData(this.SqlAuthComm, this.Dictionary, commandType);

        private IEnumerable<object> GetData(string sqlComm, Dictionary<string, object> dictionary, CommandType commandType)
        {
            var history = new List<AuthenticationHistory>();
            try
            {
                var parameters = new DynamicParameters(dictionary);
                history = this.CustomSQL.ExecuteIList<AuthenticationHistory>(sqlComm, parameters, commandType).ToList();
            }
            catch (Exception ex)
            {
                this._log.Error("TableViewer Get History Data Error", ex);
            }

            string param = this.OrderColumeName;
            var propertyInfo = typeof(AuthenticationHistory).GetProperty(param);

            if (this.Order == 0)
            {
                history = history.OrderByDescending(p => p.GetType()
                    .GetProperty(this.OrderColumeName)
                    .GetValue(p, null)).Take(this.Count).ToList();
            }
            else
            {
                history = history.OrderBy(p => p.GetType()
                    .GetProperty(this.OrderColumeName)
                    .GetValue(p, null)).Take(this.Count).ToList();
            }

            return history;
        }

        /// <summary>
        /// 更新規則
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="sqlParams"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        private string UpdateRule(string[][] rules, List<SqlParameter> sqlParams, string TableName = "History")
        {
            string ruleQuery = string.Empty;
            bool groupCheck = false;

            for (int i = 0 ; i < rules.Length ; i++)
            {
                string ddlFieldName;
                if (int.TryParse(rules[i][1], out int j))
                {
                    ddlFieldName = GetColumeName(j);
                }
                else
                {
                    continue;
                }

                string fieldTxetValue = string.Empty;
                if (!string.IsNullOrEmpty(rules[i][3]))
                {
                    fieldTxetValue = rules[i][3];
                }
                //else
                //{
                //    continue;
                //}

                string ddlComparisonOperator = string.Empty;
                if (int.TryParse(rules[i][2], out int k))
                {
                    ddlComparisonOperator = ComparisonOperator(k);
                }

                string ddlLogicOperator = string.Empty;
                switch (int.TryParse(rules[i][5], out int l) ? l : -1)
                {
                    case 0:
                        ddlLogicOperator = "AND";
                        break;

                    case 1:
                        ddlLogicOperator = "OR";
                        break;
                }

                string ddlGroupOpening = string.Empty;
                if (rules[i][0] == "1")
                {
                    ddlGroupOpening = "(";
                }

                string ddlGroupClosing = string.Empty;
                if (rules[i][4] == "1")
                {
                    ddlGroupClosing = ")";
                }

                string sqlParam = "@sqlParam" + i;
                string fieldName = "[" + ddlFieldName + "]";
                string fieldValue = fieldTxetValue;
                string comparisonOperator = ddlComparisonOperator;
                string logicOperator = ddlLogicOperator;
                string groupOpening = ddlGroupOpening;
                string groupClosing = ddlGroupClosing;

                var columnDataType = CheckColumeDataType(this.CustomSQL, ddlFieldName, TableName);
                // Field value cannot be empty for these operations
                if (comparisonOperator != "DISTINCT" && comparisonOperator != "REDUNDENT" && comparisonOperator != "EMPTY" && comparisonOperator != "NOT EMPTY" &&
                    comparisonOperator != "VALID ASCII" && comparisonOperator != "INVALID ASCII" && comparisonOperator != "VALID CPIS" && comparisonOperator != "INVALID CPIS")
                {
                    if (string.IsNullOrEmpty(fieldValue))
                    {
                        return string.Empty;
                    }

                    if (columnDataType != ColumnDataType.NotDateOrTime)
                    {
                        if (comparisonOperator != "IN")
                        {
                            fieldValue = CheckDateFormat(fieldValue, columnDataType);
                            if (fieldValue == string.Empty)
                            {
                                return string.Empty;
                            }
                        }

                        if (comparisonOperator == "IN")
                        {
                            bool isValueCorrect = true;
                            string newfieldValue = string.Empty;
                            foreach (string arg in fieldValue.Split(','))
                            {
                                string value = CheckDateFormat(arg.Trim(), columnDataType);
                                if (value == string.Empty)
                                {
                                    isValueCorrect = false;
                                    break;
                                }

                                newfieldValue = value + ",";
                            }

                            if (!isValueCorrect)
                            {
                                return string.Empty;
                            }

                            fieldValue = newfieldValue.TrimEnd(',');
                        }
                    }
                }

                // Logic operator is required between items
                if (i < rules.Length - 1 && string.IsNullOrEmpty(logicOperator))
                {
                    return string.Empty;
                }

                // Logic operator cannot be assigned for the last item
                if (i == rules.Length - 1 && !string.IsNullOrEmpty(logicOperator))
                {
                    return string.Empty;
                }

                // Add up number of (
                if (!string.IsNullOrEmpty(groupOpening))
                {
                    ruleQuery += "(";

                    if (groupCheck)
                    {
                        return string.Empty;
                    }

                    groupCheck = true;
                }

                switch (comparisonOperator)
                {
                    //ruleQuery += "CONTAINS(" + fieldName + ", '" + fieldValue + "')";
                    case "LIKE":
                        ruleQuery += fieldName + " " + comparisonOperator + " '%'+ " + sqlParam + " +'%' ";
                        sqlParams.Add(new SqlParameter(sqlParam, fieldValue));
                        break;

                    case "NOT LIKE":
                        ruleQuery += fieldName + " " + comparisonOperator + " '%'+ " + sqlParam + " +'%' ";
                        sqlParams.Add(new SqlParameter(sqlParam, fieldValue));
                        break;

                    case "START WITH":
                        ruleQuery += fieldName + " LIKE " + sqlParam + " +'%' ";
                        sqlParams.Add(new SqlParameter(sqlParam, fieldValue));
                        break;

                    case "NOT START WITH":
                        ruleQuery += fieldName + " NOT LIKE " + sqlParam + " +'%' ";
                        sqlParams.Add(new SqlParameter(sqlParam, fieldValue));
                        break;

                    case "END WITH":
                        ruleQuery += fieldName + " LIKE '%'+ " + sqlParam;
                        sqlParams.Add(new SqlParameter(sqlParam, fieldValue));
                        break;

                    case "NOT END WITH":
                        ruleQuery += fieldName + " NOT LIKE '%' + " + sqlParam;
                        sqlParams.Add(new SqlParameter(sqlParam, fieldValue));
                        break;

                    case "EMPTY":
                        ruleQuery += "(" + fieldName + " IS NULL OR " + fieldName + " = '')";
                        break;

                    case "NOT EMPTY":
                        ruleQuery += "(" + fieldName + " IS NOT NULL AND " + fieldName + " <> '')";
                        break;

                    case "VALID ASCII":
                        ruleQuery += "(Sweepstakes.ufn_IsValidAscii(" + fieldName + ") = 1)";
                        break;

                    case "INVALID ASCII":
                        ruleQuery += "(Sweepstakes.ufn_IsValidAscii(" + fieldName + ") = 0)";
                        break;

                    case "VALID CPIS":
                        ruleQuery += "(dbo.IsValidCPISAscii(" + fieldName + ") = 1)";
                        break;

                    case "INVALID CPIS":
                        ruleQuery += "(dbo.IsValidCPISAscii(" + fieldName + ") = 0)";
                        break;

                    case "DISTINCT":
                        ruleQuery += fieldName + " IN (SELECT " + fieldName +
                                     " FROM @TableName " +
                                     " GROUP BY " + fieldName + " HAVING " +
                                     " COUNT(" + fieldName + ") = 1)";
                        break;

                    case "REDUNDENT":
                        ruleQuery += fieldName + " IN (SELECT " + fieldName +
                                     " FROM @TableName " +
                                     " GROUP BY " + fieldName + " HAVING " +
                                     " COUNT(" + fieldName + ") > 1)";
                        break;

                    case "IN":
                        string[] args = fieldValue.Split(',');
                        ruleQuery += fieldName + " IN (";
                        int index = 0;
                        foreach (string arg in args)
                        {
                            string sqlInParam = sqlParam + "In" + index;
                            ruleQuery += sqlInParam + " ,";
                            sqlParams.Add(new SqlParameter(sqlInParam, arg));
                            index++;
                        }

                        ruleQuery = ruleQuery.Trim(',');
                        ruleQuery += ")";
                        break;

                    default:
                        ruleQuery += fieldName + " " + comparisonOperator + " " + sqlParam;
                        sqlParams.Add(new SqlParameter(sqlParam, fieldValue));
                        break;
                }

                // Add up number of )
                if (!string.IsNullOrEmpty(groupClosing))
                {
                    ruleQuery += ")";

                    if (groupCheck)
                    {
                        groupCheck = false;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }

                // If logic operator is selected
                if (!string.IsNullOrEmpty(logicOperator))
                {
                    ruleQuery += " " + logicOperator + " ";
                }
            }

            return ruleQuery;
        }

        private ColumnDataType CheckColumeDataType(IDataAccess sqlHelper, string columeName, string tableName)
        {
            string sqlComm = @" SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WITH(NOLOCK)
                                WHERE TABLE_NAME = @TABLE_NAME AND COLUMN_NAME = @COLUMN_NAME";
            try
            {
                object DataType = sqlHelper.ExecuteScalar<object>(sqlComm, new
                {
                    TABLE_NAME = tableName,
                    COLUMN_NAME = columeName
                }, CommandType.Text);
                if ("datetime".Equals(DataType?.ToString()))
                {
                    return ColumnDataType.DateTime;
                }
            }
            catch (SqlException ex)
            {
                Log.Error($"TableViewer Error : {ex.ToString()}", this);
            }
            catch (Exception ex)
            {
                Log.Error($"TableViewer Error : {ex.ToString()}", this);
            }

            return ColumnDataType.NotDateOrTime;
        }

        /// <summary>
        /// 轉換日期形式
        /// </summary>
        /// <param name="date">日期字串</param>
        /// <param name="columnDataType"></param>
        /// <returns>日期格式</returns>
        private static string CheckDateFormat(string date, ColumnDataType columnDataType)
        {
            if (columnDataType == ColumnDataType.DateTime)
            {
                DateTime dt;
                if (DateTime.TryParse(date, out dt))
                {
                    return dt.ToString(DISPLAYTIMEFORMAT);
                }
            }

            return string.Empty;
        }

        #region Property

        private Dictionary<string, object> Dictionary { get; set; }

        private string SqlAuthComm { get; set; }

        private int Count { get; set; }

        private int Order { get; set; }

        private string OrderColumeName { get; set; }

        public string Message { get; set; }

        public string Path { get; set; }

        public bool IsManager { get; set; }

        private IDataAccess CustomSQL => DbManager.Custom;

        private const string DISPLAYTIMEFORMAT = "yyyy/MM/dd HH:mm:ss.fff";

        private const string MESSAGE = "<br/>篩選條件設定不正確:" +
                                       "<br/>1. 除了 DISTINCT、REDUNDENT、EMPTY、NOT EMPTY、VALID ASCII、INVALID ASCII 外，請確認每個項目都有值。" +
                                       "<br/>2. 項目之間需要選擇關聯 (AND/OR)。" +
                                       "<br/>3. 設定條件群組時，請確認括號是否對稱。" +
                                       "<br/>4. Datatime 欄位的值請輸入 '" + DISPLAYTIMEFORMAT + "'(中間為一個空格) 。" +
                                       "<br/>5. GUID 欄位的值請輸入 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'。";

        private static readonly Dictionary<string, IDictionary<string, string>> _params = new();

        private IDictionary<string, string> Setting
        {
            get
            {
                if (!_params.TryGetValue(this.Path, out var dic))
                {
                    if (RenderingContext.CurrentOrNull != null)
                    {
                        var param = WebUtil.ParseUrlParameters(RenderingContext.CurrentOrNull.Rendering["Parameters"]);
                        dic = param.ToDictionary();
                        _params[this.Path] = dic;
                    }
                }

                return dic;
            }
        }

        public string Domain => this.Setting?["Filter Options Domain"];

        #endregion Property

        public class ConfigItemID
        {
            public const string UserRecord = "{24191774-4AA0-443E-A180-CCEB339B8C60}";
        }
    }
}