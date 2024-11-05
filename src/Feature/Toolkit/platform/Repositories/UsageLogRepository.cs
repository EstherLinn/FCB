using Feature.Wealth.Toolkit.Models.TableViewer;
using Feature.Wealth.Toolkit.Models.TableViewer.SitecoreMemberRecord;
using log4net;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.Toolkit.Repositories
{
    public class UsageLogRepository
    {
        private readonly ILog _log = Logger.General;
        private const string DATETIMEFORMAT = "yyyy-MM-dd HH:mm:ss:fff";

        /// <summary>
        /// User Record 檔案匯出
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public MemoryStream BuildUserRecordReportExcel(IEnumerable<object> objects)
        {
            MemoryStream excelDatas = new();

            // Create Excel
            XSSFWorkbook xssfworkbook = new();
            ISheet sheet = xssfworkbook.CreateSheet("Usage Log");

            try
            {
                if (objects == null)
                {
                    return excelDatas;
                }

                // 標題
                int rowCounter = 0;
                sheet.CreateRow(rowCounter);
                var headerRow = sheet.GetRow(rowCounter);
                headerRow.CreateCell(0).SetCellValue("Id");
                headerRow.CreateCell(1).SetCellValue("類別");
                headerRow.CreateCell(2).SetCellValue("執行動作");
                headerRow.CreateCell(3).SetCellValue("項目Id");
                headerRow.CreateCell(4).SetCellValue("項目語言");
                headerRow.CreateCell(5).SetCellValue("項目版本號");
                headerRow.CreateCell(6).SetCellValue("項目路徑");
                headerRow.CreateCell(7).SetCellValue("使用者名稱");
                headerRow.CreateCell(8).SetCellValue("任務堆疊資訊");
                headerRow.CreateCell(9).SetCellValue("附加資訊");
                headerRow.CreateCell(10).SetCellValue("建立時間");

                // Excel 資料
                List<History> historyData = objects.OfType<History>().ToList();

                foreach (History historyRowData in historyData)
                {
                    rowCounter++;
                    sheet.CreateRow(rowCounter);
                    var currentRow = sheet.GetRow(rowCounter);
                    currentRow.CreateCell(0).SetCellValue(historyRowData.Id.ToString());
                    currentRow.CreateCell(1).SetCellValue(historyRowData.Category);
                    currentRow.CreateCell(2).SetCellValue(historyRowData.Action);
                    currentRow.CreateCell(3).SetCellValue(historyRowData.ItemId.ToString());
                    currentRow.CreateCell(4).SetCellValue(historyRowData.ItemLanguage);
                    currentRow.CreateCell(5).SetCellValue(historyRowData.ItemVersion);
                    currentRow.CreateCell(6).SetCellValue(historyRowData.ItemPath);
                    currentRow.CreateCell(7).SetCellValue(historyRowData.UserName);
                    currentRow.CreateCell(8).SetCellValue(historyRowData.TaskStack);
                    currentRow.CreateCell(9).SetCellValue(historyRowData.AdditionalInfo);
                    currentRow.CreateCell(10).SetCellValue(historyRowData.Created.ToString(DATETIMEFORMAT));
                }

                xssfworkbook.Write(excelDatas);
            }
            catch (Exception ex)
            {
                this._log.Error("UsageLogRepository 檔案匯出發生錯誤", ex);
            }

            return excelDatas;
        }

        /// <summary>
        /// Sitecore Member Record 檔案匯出
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public MemoryStream BuildSitecoreMemberRecordReportExcel(IEnumerable<object> objects)
        {
            MemoryStream excelDatas = new();

            // Create Excel
            XSSFWorkbook xssfworkbook = new();
            ISheet sheet = xssfworkbook.CreateSheet("Member Usage Log");

            try
            {
                if (objects == null)
                {
                    return excelDatas;
                }

                // 標題
                int rowCounter = 0;
                sheet.CreateRow(rowCounter);
                var headerRow = sheet.GetRow(rowCounter);
                headerRow.CreateCell(0).SetCellValue("Id");
                headerRow.CreateCell(1).SetCellValue("執行動作");
                headerRow.CreateCell(2).SetCellValue("使用者Id");
                headerRow.CreateCell(3).SetCellValue("建立時間");
                headerRow.CreateCell(4).SetCellValue("使用者姓名");
                headerRow.CreateCell(5).SetCellValue("單位名稱");
                headerRow.CreateCell(6).SetCellValue("角色");
                headerRow.CreateCell(7).SetCellValue("IP");

                // Excel 資料
                List<AuthenticationHistory> historyData = objects.OfType<AuthenticationHistory>().ToList();

                foreach (AuthenticationHistory historyRowData in historyData)
                {
                    rowCounter++;
                    sheet.CreateRow(rowCounter);
                    var currentRow = sheet.GetRow(rowCounter);
                    currentRow.CreateCell(0).SetCellValue(historyRowData.Id.ToString());
                    currentRow.CreateCell(1).SetCellValue(historyRowData.Action);
                    currentRow.CreateCell(2).SetCellValue(historyRowData.UserName);
                    currentRow.CreateCell(3).SetCellValue(historyRowData.Created.ToString(DATETIMEFORMAT));
                    currentRow.CreateCell(4).SetCellValue(historyRowData.FullName);
                    currentRow.CreateCell(5).SetCellValue(historyRowData.DepartmentName);
                    currentRow.CreateCell(6).SetCellValue(historyRowData.Roles);
                    currentRow.CreateCell(7).SetCellValue(historyRowData.IP);
                }

                xssfworkbook.Write(excelDatas);
            }
            catch (Exception ex)
            {
                this._log.Error("UsageLogRepository 檔案匯出發生錯誤", ex);
            }

            return excelDatas;
        }
    }
}
