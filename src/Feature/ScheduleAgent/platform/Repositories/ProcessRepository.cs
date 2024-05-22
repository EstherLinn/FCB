using Feature.Wealth.ScheduleAgent.Models.Sysjust;
using Foundation.Wealth.Manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;

namespace Feature.Wealth.ScheduleAgent.Repositories
{
    public  class ProcessRepository
    {
        /// <summary>
        /// 將資料插入資料庫(如果有一樣的就更新，有不同資料則新增)
        /// </summary>
        public  void BulkInsertToDatabase<T>(List<T> data, string tableName, string uniqueColumn, string key, string filePath)
        {
            string mergeQuery = GenerateMergeQuery<T>(tableName, uniqueColumn, key);
            int line = DbManager.Custom.ExecuteNonQuery(mergeQuery, data, CommandType.Text);
            LogChangeHistory(DateTime.Now, filePath, "資料差異更新", tableName, line);
        }

        /// <summary>
        /// 將資料插入資料庫(如果有一樣的就更新，有不同資料則新增)三個參數比對
        /// </summary>
        public  void BulkInsertToDatabase<T>(List<T> data, string tableName, string uniqueColumn, string key, string key2, string filePath)
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
            LogChangeHistory(DateTime.Now, filePath, "資料差異更新", tableName, line);
        }

        /// <summary>
        /// 將資料插入最新的資料表中(會刪除舊資料，插入最新的資料=資料表裡僅保留最新的資料)
        /// </summary>
        public  void BulkInsertToNewDatabase<T>(List<T> data, string tableName, string filePath)
        {
            var properties = typeof(T).GetProperties();
            string columns = string.Join(",", properties.Select(p => p.Name));
            string parameters = string.Join(",", properties.Select(p => "@" + p.Name));

            string insertQuery = $@"
                TRUNCATE TABLE {tableName};
                INSERT INTO {tableName} ({columns})
                VALUES ({parameters});
                ";

            int line = DbManager.Custom.ExecuteNonQuery(insertQuery, data, CommandType.Text, true);
            LogChangeHistory(DateTime.Now, filePath, "最新資料", tableName, line);
        }

        /// <summary>
        /// 直接將資料插入資料庫(增量)
        /// </summary>
        public  void BulkInsertDirectToDatabase<T>(List<T> data, string tableName, string filePath)
        {
            var properties = typeof(T).GetProperties();
            string columns = string.Join(",", properties.Select(p => p.Name));
            string parameters = string.Join(",", properties.Select(p => "@" + p.Name));

            string insertQuery = $@"
            INSERT INTO {tableName} ({columns})
            VALUES ({parameters});
            ";

            int line = DbManager.Custom.ExecuteNonQuery(insertQuery, data, CommandType.Text, true);
            LogChangeHistory(DateTime.Now, filePath, "資料增量", tableName, line);
        }

        private  string GenerateMergeQuery<T>(string tableName, string uniqueColumn, string key)
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

        private  string GenerateUpdateSet(PropertyInfo[] properties)
        {
            string updateColumns = string.Join(",", properties.Select(p => $"target.{p.Name} = source.{p.Name}"));
            return updateColumns;
        }

        public  string CalculateHash(string archiveFilePath)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(File.ReadAllBytes(archiveFilePath));
                return BitConverter.ToString(hashBytes);
            }
        }

        public  void LogChangeHistory(DateTime timestamp, string filePath, string operationType, string tableName, int line)
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

        public  void InsertChangeHistory(ChangeHistory changeHistory)
        {
            string insertHistoryQuery = """
                                        INSERT INTO ChangeHistory (FileName, ModificationDate, ModificationType, DataTable,ModificationLine)
                                        VALUES (@FileName, @ModificationDate, @ModificationType, @DataTable, @ModificationLine);
                                        """;

            DbManager.Custom.ExecuteNonQuery(insertHistoryQuery, changeHistory, CommandType.Text, true);
        }
    }
}