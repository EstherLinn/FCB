using System;
using Dapper;
using System.IO;
using System.Linq;
using System.Data;
using System.Data.Odbc;
using System.Reflection;
using System.Data.Common;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Foundation.Wealth.Manager;
using System.Collections.Generic;
using Xcms.Sitecore.Foundation.Basic.Logging;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using Foundation.Wealth.Models;
using Sitecore.Configuration;
using Sitecore.Data.Items;


namespace Feature.Wealth.ScheduleAgent.Repositories
{
    public class ProcessRepository
    {

        private readonly ILoggerService _logger;
        private readonly Item _Supplementsettings;

        public ProcessRepository(ILoggerService logger, IEnumerable<Item> jobItems)
        {
            this._logger = logger;
            this._Supplementsettings = jobItems.FirstOrDefault(j => j.TemplateID == Templates.SupplementSetting.Id);
        }
        public ProcessRepository(ILoggerService logger)
        {
            this._logger = logger;
        }

        /// <summary>
        /// 將資料插入資料庫(如果有一樣的就更新，有不同資料則新增)
        /// </summary>
        public void BulkInsertToDatabase<T>(IEnumerable<T> data, string tableName, string uniqueColumn, string key, string filePath, DateTime now)
        {
            string mergeQuery = GenerateMergeQuery<T>(tableName, uniqueColumn, key);
            int line = DbManager.Custom.ExecuteNonQuery(mergeQuery, data, CommandType.Text);
            var endTime = DateTime.UtcNow;
            var duration = endTime - now;
            LogChangeHistory(DateTime.UtcNow, filePath, "資料差異更新", tableName, line, duration.TotalSeconds, "Y");
            _logger.Info($"{filePath} 資料差異更新 {tableName} {line}");
        }

        /// <summary>
        /// 將資料插入資料庫(如果有一樣的就更新，有不同資料則新增)三個參數比對
        /// </summary>
        public void BulkInsertToDatabase<T>(IEnumerable<T> data, string tableName, string uniqueColumn, string key, string key2, string filePath, DateTime now)
        {
            var properties = typeof(T).GetProperties();
            string columns = string.Join(",", properties.Select(p => p.Name));
            string parameters = string.Join(",", properties.Select(p => "@" + p.Name));

            string mergeQuery = $@"
            MERGE INTO {tableName} AS target
            USING (VALUES ({parameters})) AS source ({columns})
            ON
            (source.{uniqueColumn} IS NULL
            AND target.{uniqueColumn} IS NULL
            OR target.{uniqueColumn} = source.{uniqueColumn})
            AND (target.{key} = source.{key})
            AND (target.{key2} = source.{key2})
            WHEN MATCHED THEN
              UPDATE SET {GenerateUpdateSet(properties)}
            WHEN NOT MATCHED THEN
              INSERT ({columns}) VALUES ({parameters});
             ";

            int line = DbManager.Custom.ExecuteNonQuery(mergeQuery, data, CommandType.Text);
            var endTime = DateTime.UtcNow;
            var duration = endTime - now;
            LogChangeHistory(DateTime.UtcNow, filePath, "資料差異更新", tableName, line, duration.TotalSeconds, "Y");
            _logger.Info($"{filePath} 資料差異更新 {tableName} {line}");
        }

        public void BulkInsertToDatabaseForHIS<T>(IEnumerable<T> data, string tableName, string uniqueColumn, string key, string filePath, DateTime now)
        {
            var properties = typeof(T).GetProperties();
            string columns = string.Join(",", properties.Select(p => p.Name));
            string parameters = string.Join(",", properties.Select(p => "@" + p.Name));

            string mergeQuery = $@"
            MERGE INTO {tableName} AS target
            USING (VALUES ({parameters})) AS source ({columns})
            ON
            (target.{key} > DATEADD(DAY, -30, GETDATE()))
            AND (source.{uniqueColumn} IS NULL
            AND target.{uniqueColumn} IS NULL
            OR target.{uniqueColumn} = source.{uniqueColumn})
            AND (target.{key} = source.{key})
            WHEN MATCHED THEN
              UPDATE SET {GenerateUpdateSet(properties)}
            WHEN NOT MATCHED THEN
              INSERT ({columns}) VALUES ({parameters});
             ";

            int line = DbManager.Custom.ExecuteNonQuery(mergeQuery, data, CommandType.Text);
            var endTime = DateTime.UtcNow;
            var duration = endTime - now;
            LogChangeHistory(DateTime.UtcNow, filePath, "資料差異更新", tableName, line, duration.TotalSeconds, "Y");
            _logger.Info($"{filePath} 資料差異更新 {tableName} {line}");
        }

        public void BulkInsertToDatabaseFor30Days<T>(IEnumerable<T> data, string tableName, string uniqueColumn, string key, string key2, string filePath, string date, DateTime now)
        {
            var properties = typeof(T).GetProperties();
            string columns = string.Join(",", properties.Select(p => p.Name));
            string parameters = string.Join(",", properties.Select(p => "@" + p.Name));

            string mergeQuery = $@"
            MERGE INTO {tableName} AS target
            USING (VALUES ({parameters})) AS source ({columns})
            ON
            (target.{date} > DATEADD(DAY, -30, GETDATE()))
            AND (source.{uniqueColumn} IS NULL
            AND target.{uniqueColumn} IS NULL
            OR target.{uniqueColumn} = source.{uniqueColumn})
            AND (target.{key} = source.{key})
            AND (target.{key2} = source.{key2})
            WHEN MATCHED THEN
              UPDATE SET {GenerateUpdateSet(properties)}
            WHEN NOT MATCHED THEN
              INSERT ({columns}) VALUES ({parameters});
             ";

            int line = DbManager.Custom.ExecuteNonQuery(mergeQuery, data, CommandType.Text);
            var endTime = DateTime.UtcNow;
            var duration = endTime - now;
            LogChangeHistory(DateTime.UtcNow, filePath, "資料差異更新", tableName, line, duration.TotalSeconds, "Y");
            _logger.Info($"{filePath} 資料差異更新 {tableName} {line}");
        }


        /// <summary>
        /// 將資料插入最新的資料表中(會刪除舊資料，插入最新的資料=資料表裡僅保留最新的資料)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas">集合</param>
        /// <param name="tableName">資料表名稱</param>
        /// <param name="fileName">檔案名稱</param>
        public void BulkInsertToNewDatabase<T>(IEnumerable<T> datas, string tableName, string fileName, DateTime now)
        {
            string selectQuery = $@"SELECT * FROM {tableName}";
            var results = DbManager.Custom.ExecuteIList<T>(selectQuery, null, CommandType.Text);

            var properties = typeof(T).GetProperties();
            string columns = string.Join(",", properties.Select(p => p.Name));
            string parameters = string.Join(",", properties.Select(p => "@" + p.Name));

            var spparameters = new DynamicParameters();
            spparameters.Add("@schemaname", "dbo", DbType.String, ParameterDirection.Input);
            spparameters.Add("@tablename", tableName, DbType.String, ParameterDirection.Input);

            string storedProcedureName = "P_TruncateTable";
            string truncateQuery = storedProcedureName;
            ExecuteNonQuery(truncateQuery, spparameters, CommandType.StoredProcedure, true);

            string insertQuery = $@"
            INSERT INTO {tableName} ({columns})
            VALUES ({parameters});
            ";

            int line = ExecuteNonQuery(insertQuery, datas, CommandType.Text, true);

            if (line == 0)
            {
                line = ExecuteNonQuery(insertQuery, results, CommandType.Text, true);
            }

            var endTime = DateTime.UtcNow;
            var duration = endTime - now;
            LogChangeHistory(DateTime.UtcNow, fileName, "最新資料", tableName, line, duration.TotalSeconds, "Y");
            _logger.Info($"{fileName} 最新資料 {tableName} {line}");
        }

        ///加密的資料
        public void BulkInsertToEncryptedDatabase<T>(IList<T> data, string tableName, string filePath, DateTime now)
        {
            var properties = typeof(T).GetProperties();

            using (SqlConnection connection = (SqlConnection)DbManager.Custom.DbConnection())
            {
                connection.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = tableName;

                    for (int i = 0; i < properties.Length; i++)
                    {
                        var property = properties[i];
                        bulkCopy.ColumnMappings.Add(property.Name, property.Name);
                    }

                    try
                    {
                        DataTable dataTable = ConvertToDataTable<T>(data, properties);

                        if (dataTable.Rows.Count > 0)
                        {
                            TrancateTable(tableName);
                        }

                        bulkCopy.WriteToServer(dataTable);

                        int rowsAffected = dataTable.Rows.Count;

                        var endTime = DateTime.UtcNow;
                        var duration = endTime - now;
                        LogChangeHistory(DateTime.UtcNow, filePath, "最新資料", tableName, rowsAffected, duration.TotalSeconds, "Y");
                        _logger.Info($"{filePath} 最新資料 {tableName} {rowsAffected} 行");
                    }
                    catch (Exception ex)
                    {
                        this._logger.Error(ex.Message, ex);
                    }
                }
            }

        }

        /// <summary>
        /// 將資料直接插入最新的資料表中
        /// </summary>

        public void BulkInsertDirectToDatabase<T>(IEnumerable<T> data, string tableName, string filePath, DateTime startTime)
        {
            var properties = typeof(T).GetProperties();
            string columns = string.Join(",", properties.Select(p => p.Name));
            string parameters = string.Join(",", properties.Select(p => "@" + p.Name));

            string insertQuery = $@"
            INSERT INTO {tableName} ({columns})
            VALUES ({parameters});
            ";

            int line = ExecuteNonQuery(insertQuery, data, CommandType.Text, true);
            LogChangeHistory(DateTime.UtcNow, filePath, "最新資料", tableName, line, (DateTime.UtcNow - startTime).TotalSeconds, "Y");
            _logger.Info($"{filePath} 最新資料 {tableName} {line}，花費 {(DateTime.UtcNow - startTime).TotalSeconds} 秒匯入資料庫");
        }


        private string GenerateMergeQuery<T>(string tableName, string uniqueColumn, string key)
        {
            var properties = typeof(T).GetProperties();
            string columns = string.Join(",", properties.Select(p => p.Name));
            string parameters = string.Join(",", properties.Select(p => "@" + p.Name));

            string mergeQuery = $@"
          MERGE INTO {tableName} AS target
          USING (VALUES ({parameters})) AS source ({columns})
          ON
        (source.{uniqueColumn} IS NULL
        AND target.{uniqueColumn} IS NULL
        OR target.{uniqueColumn} = source.{uniqueColumn})
        AND (target.{key} = source.{key})
          WHEN MATCHED THEN
              UPDATE SET {GenerateUpdateSet(properties)}
          WHEN NOT MATCHED THEN
              INSERT ({columns}) VALUES ({parameters});
        ";

            return mergeQuery;
        }

        private string GenerateUpdateSet(PropertyInfo[] properties)
        {
            string updateColumns = string.Join(",", properties.Select(p => $"target.{p.Name} = source.{p.Name}"));
            return updateColumns;
        }

        public async Task BulkInsertFromOracle<T>(IList<T> data, string tableName)
        {
            var properties = typeof(T).GetProperties();

            using (SqlConnection connection = (SqlConnection)DbManager.Custom.DbConnection())
            {
                await connection.OpenAsync();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = tableName;

                    for (int i = 0; i < properties.Length; i++)
                    {
                        var property = properties[i];
                        bulkCopy.ColumnMappings.Add(property.Name, property.Name);
                    }

                    try
                    {
                        DataTable dataTable = ConvertToDataTable(data, properties);
                        await bulkCopy.WriteToServerAsync(dataTable);
                    }
                    catch (Exception ex)
                    {
                        this._logger.Error(ex.Message, ex);
                    }
                }
            }
        }

        private DataTable ConvertToDataTable<T>(IList<T> data, PropertyInfo[] properties)
        {
            DataTable dataTable = new DataTable();

            foreach (var property in properties)
            {
                DataColumn column = new DataColumn(property.Name);

                if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                {
                    column.DataType = typeof(DateTime);
                }
                else
                {
                    column.DataType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                }

                dataTable.Columns.Add(column);
            }

            foreach (T item in data)
            {
                DataRow row = dataTable.NewRow();
                foreach (var property in properties)
                {
                    row[property.Name] = property.GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }


        public void TrancateTable(string tableName)
        {
            var spparameters = new DynamicParameters();
            spparameters.Add("@schemaname", "dbo", DbType.String, ParameterDirection.Input);
            spparameters.Add("@tablename", tableName, DbType.String, ParameterDirection.Input);

            string storedProcedureName = "P_TruncateTable";
            ExecuteNonQuery(storedProcedureName, spparameters, CommandType.StoredProcedure, true);
        }

        public void LogChangeHistory(DateTime timestamp, string filePath, string operationType, string tableName, int line, double time, string YorN)
        {
            var changeHistory = new ChangeHistory
            {
                FileName = Path.GetFileName(filePath),
                ModificationDate = timestamp,
                ModificationType = operationType,
                DataTable = tableName,
                ModificationLine = line,
                TotalSeconds = time,
                Success = YorN
            };
            InsertChangeHistory(changeHistory);
        }

        public void InsertChangeHistory(ChangeHistory changeHistory)
        {
            string insertHistoryQuery = """
                                        INSERT INTO ChangeHistory (FileName, ModificationDate, ModificationType, DataTable,ModificationLine,TotalSeconds,Success)
                                        VALUES (@FileName, @ModificationDate, @ModificationType, @DataTable, @ModificationLine,@TotalSeconds,@Success);
                                        """;

            using (var connection = DbManager.Custom.DbConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Execute(insertHistoryQuery, changeHistory, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        this._logger.Error(ex.Message, ex);
                    }
                }

            }
        }

        private int ExecuteNonQuery(string sql, object param, CommandType commandType, bool hasTransaction = false)
        {
            int result = 0;
            using (var connection = DbManager.Custom.DbConnection())
            {
                connection.Open();
                if (hasTransaction)
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            result = connection.Execute(sql, param, transaction, null, commandType);
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            this._logger.Error(ex.Message, ex);
                        }
                    }
                }
                else
                {
                    try
                    {
                        result = connection.Execute(sql, param, null, null, commandType);
                    }
                    catch (Exception ex)
                    {
                        this._logger.Error(ex.Message, ex);
                    }
                }
            }

            return result;
        }


        public IEnumerable<T> Enumerate<T>(string sql) where T : new()
        {
            string connString = ConfigurationManager.ConnectionStrings["cif"].ConnectionString;
            OdbcConnection connection = null;
            OdbcCommand command = null;
            DbDataReader reader = null;

            try
            {
                this._logger.Info("OdbcConnection start");
                connection = new OdbcConnection(connString);
                connection.Open();
                this._logger.Info("Opened Successfully");

                command = new OdbcCommand(sql, connection);
                this._logger.Info("Command created successfully");

                reader = command.ExecuteReader();
                this._logger.Info("Reader created successfully");
            }
            catch (Exception ex)
            {
                this._logger.Error(ex.ToString());
                yield break;
            }

            var properties = typeof(T).GetProperties();
            this._logger.Info("Properties retrieved successfully");

            try
            {
                while (reader.Read())
                {
                    var item = new T();
                    foreach (var property in properties)
                    {
                        var ordinal = reader.GetOrdinal(property.Name);
                        if (!reader.IsDBNull(ordinal))
                        {
                            property.SetValue(item, reader.GetValue(ordinal), null);
                        }
                    }
                    yield return item;
                }
            }
            finally
            {
                reader?.Close();
                connection.Close();
                this._logger.Info("Reader and Connection closed.");
            }
        }




        ///<summary>
        ///改變紅綠燈狀態
        ///</summary>
        
        public void TurnTrafficLight(NameofTrafficLight name, TrafficLightStatus status)
        {
            // 先偵測總開關有沒有開
            bool masterSwitch = Settings.GetBoolSetting("MasterLightSwitch", false);

            // 如果總開關沒有開，記錄警告並返回
            if (!masterSwitch)
            {
                this._logger.Warn("紅綠燈總開關沒開");
                return;
            }
            // 獲取 Supplement Setting 中的 Turn All Lights 值
            string turnAllLightsSetting = this._Supplementsettings?["Turn All Lights"]?.ToString();

            TrafficLightStatus effectiveStatus;

            // 根據 Turn All Lights 的設定決定燈號的顏色
            if (turnAllLightsSetting != null)
            {
                // 根據設定的值來決定燈號顏色
                if (turnAllLightsSetting.Equals("Red", StringComparison.OrdinalIgnoreCase))
                {
                    effectiveStatus = TrafficLightStatus.Red;
                    this._logger.Info("開啟永遠紅燈");
                }
                else if (turnAllLightsSetting.Equals("Green", StringComparison.OrdinalIgnoreCase))
                {
                    effectiveStatus = TrafficLightStatus.Green;
                    this._logger.Info("開啟永遠綠燈");
                }
                else
                {
                    effectiveStatus = status;
                    this._logger.Info($"Turn On {status}");
                }
            }
            else
            {
                effectiveStatus = status;
                this._logger.Info($"Turn On {status}");
            }

            //改變資料庫燈號狀態
            var parameters = new DynamicParameters();
            parameters.Add("@number", (int)name, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@status", (int)effectiveStatus, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@time", DateTime.Now, DbType.DateTime, ParameterDirection.Input);

            string sql = """
                    UPDATE [SignalStatus]
                    SET Status = @status,UpdateTime = @time
                    WHERE Number = @number
                    """;

            try
            {
                DbManager.Custom.ExecuteNonQuery(sql, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                this._logger.Error(ex.Message, ex);
            }
        }

    }
}