using Dapper;
using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using Feature.Wealth.ScheduleAgent.Models.Wealth;
using Foundation.Wealth.Manager;
using Sitecore.Data.Items;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Xdb.Reporting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.Logging;
using static Sitecore.ContentSearch.Linq.Extensions.ReflectionExtensions;

namespace Feature.Wealth.ScheduleAgent.Repositories
{
    public class ProcessRepository
    {

        private readonly ILoggerService _logger;

        public ProcessRepository(ILoggerService logger)
        {
            this._logger = logger;
        }

            /// <summary>
            /// 將資料插入資料庫(如果有一樣的就更新，有不同資料則新增)
            /// </summary>
            public void BulkInsertToDatabase<T>(IEnumerable<T> data, string tableName, string uniqueColumn, string key, string filePath)
        {
            string mergeQuery = GenerateMergeQuery<T>(tableName, uniqueColumn, key);
            int line = DbManager.Custom.ExecuteNonQuery(mergeQuery, data, CommandType.Text);
            LogChangeHistory(DateTime.UtcNow, filePath, "資料差異更新", tableName, line);
        }

        /// <summary>
        /// 將資料插入資料庫(如果有一樣的就更新，有不同資料則新增)三個參數比對
        /// </summary>
        public void BulkInsertToDatabase<T>(IEnumerable<T> data, string tableName, string uniqueColumn, string key, string key2, string filePath)
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
            LogChangeHistory(DateTime.UtcNow, filePath, "資料差異更新", tableName, line);
        }

        /// <summary>
        /// 將資料插入最新的資料表中(會刪除舊資料，插入最新的資料=資料表裡僅保留最新的資料)
        /// </summary>

        public void BulkInsertToNewDatabase<T>(IEnumerable<T> data, string tableName, string filePath)
        {
            var properties = typeof(T).GetProperties();
            string columns = string.Join(",", properties.Select(p => p.Name));
            string parameters = string.Join(",", properties.Select(p => "@" + p.Name));

            string truncateQuery = $"TRUNCATE TABLE {tableName};";
            ExecuteNonQuery(truncateQuery, null, CommandType.Text, true);

            string insertQuery = $@"
            INSERT INTO {tableName} ({columns})
            VALUES ({parameters});
            ";

            int line = ExecuteNonQuery(insertQuery, data, CommandType.Text, true);
            LogChangeHistory(DateTime.UtcNow, filePath, "最新資料", tableName, line);
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

            string connString = ConfigurationManager.ConnectionStrings["custom"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connString))
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


        public void LogChangeHistory(DateTime timestamp, string filePath, string operationType, string tableName, int line)
        {
            var changeHistory = new ChangeHistory
            {
                FileName = Path.GetFileName(filePath),
                ModificationDate = timestamp,
                ModificationType = operationType,
                DataTable = tableName,
                ModificationLine = line
            };
            InsertChangeHistory(changeHistory);
        }


        public void InsertChangeHistory(ChangeHistory changeHistory)
        {
            string insertHistoryQuery = """
                                        INSERT INTO ChangeHistory (FileName, ModificationDate, ModificationType, DataTable,ModificationLine)
                                        VALUES (@FileName, @ModificationDate, @ModificationType, @DataTable, @ModificationLine);
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



        public List<T> ConnectOdbc<T>(string sql) where T : new()
        {
            string connString = ConfigurationManager.ConnectionStrings["cif"].ConnectionString;
            var resultList = new List<T>();

            using (OdbcConnection connection = new OdbcConnection(connString))
            {
                try
                {
                    connection.Open();
                    this._logger.Info("Opened Successfully");

                    using (OdbcCommand command = new OdbcCommand(sql, connection))
                    {
                        this._logger.Info("Command created successfully");

                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            this._logger.Info("Reader created successfully");

                            var properties = typeof(T).GetProperties();
                            this._logger.Info("Properties retrieved successfully");

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
                                resultList.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this._logger.Error(ex.ToString());
                    return null;
                }
                finally
                {
                    connection.Close();
                    this._logger.Info("Connection closed.");
                }
            }

            return resultList;
        }

        public IEnumerable<T> Enumerate<T>(string sql) where T : new()
        {
            string connString = ConfigurationManager.ConnectionStrings["cif"].ConnectionString;
            OdbcConnection connection = null;
            OdbcCommand command = null;
            DbDataReader reader = null;

            try
            {
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

        public IEnumerable<T> EnumerateSQL<T>(string sql) where T : new()
        {
            string connString = ConfigurationManager.ConnectionStrings["custom"].ConnectionString;
            SqlConnection connection = null;
            SqlCommand command = null;
            DbDataReader reader = null;

            try
            {
                connection = new SqlConnection(connString);
                connection.Open();
                this._logger.Info("Opened Successfully");

                command = new SqlCommand(sql, connection);
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



    }
}