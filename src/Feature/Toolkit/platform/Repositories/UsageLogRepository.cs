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
                headerRow.CreateCell(1).SetCellValue("Category");
                headerRow.CreateCell(2).SetCellValue("Action");
                headerRow.CreateCell(3).SetCellValue("ItemId");
                headerRow.CreateCell(4).SetCellValue("ItemLanguage");
                headerRow.CreateCell(5).SetCellValue("ItemVersion");
                headerRow.CreateCell(6).SetCellValue("ItemPath");
                headerRow.CreateCell(7).SetCellValue("UserName");
                headerRow.CreateCell(8).SetCellValue("TaskStack");
                headerRow.CreateCell(9).SetCellValue("AdditionalInfo");
                headerRow.CreateCell(10).SetCellValue("Created");

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
                headerRow.CreateCell(1).SetCellValue("Action");
                headerRow.CreateCell(2).SetCellValue("UserName");
                headerRow.CreateCell(3).SetCellValue("Created");

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
