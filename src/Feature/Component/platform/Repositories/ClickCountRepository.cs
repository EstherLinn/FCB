using Foundation.Wealth.Manager;
using log4net;
using System;
using System.Data;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.Component.Repositories
{
    public class ClickCountRepository
    {
        private ILog Log { get; } = Logger.General;

        /// <summary>
        /// 取得點擊次數
        /// </summary>
        /// <param name="pageId">頁面節點ID</param>
        /// <param name="renderingId">元件ID</param>
        /// <param name="datasourceId">資料源ID</param>
        /// <param name="linkFieldId">連結欄位ID</param>
        /// <returns>點擊次數</returns>
        public int? GetClickCount(Guid? pageId, Guid? renderingId, Guid? datasourceId, Guid? linkFieldId)
        {
            int? result = null;

            if (pageId == null || renderingId == null || datasourceId == null || linkFieldId == null)
            {
                return result;
            }

            try
            {
                string sql = @"
                SELECT [ClickCount]
                FROM [dbo].[ClickCount]
                WHERE [PageId] = @PageId
                AND [RenderingId] = @RenderingId
                AND [DatasourceId] = @DatasourceId
                AND [LinkFieldId] = @LinkFieldId";

                var param = new
                {
                    PageId = pageId.Value,
                    RenderingId = renderingId.Value,
                    DatasourceId = datasourceId.Value,
                    LinkFieldId = linkFieldId.Value
                };

                result = DbManager.Custom.Execute<int?>(sql, param, CommandType.Text);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return result;
        }

        /// <summary>
        /// 更新得點擊次數
        /// </summary>
        /// <param name="pageId">頁面節點ID</param>
        /// <param name="renderingId">元件ID</param>
        /// <param name="datasourceId">資料源ID</param>
        /// <param name="linkFieldId">連結欄位ID</param>
        /// <returns>更新是否成功，成功回傳true，失敗回傳 false</returns>
        public async Task<bool> UpdateClickCount(Guid? pageId, Guid? renderingId, Guid? datasourceId, Guid? linkFieldId)
        {
            bool result = false;

            if (pageId == null || renderingId == null || datasourceId == null || linkFieldId == null)
            {
                return result;
            }

            try
            {
                var strSql = @"
            MERGE [dbo].[ClickCount] AS target
            USING (SELECT @PageId AS PageId, @RenderingId AS RenderingId, @DatasourceId AS DatasourceId, @LinkFieldId AS LinkFieldId) AS source
            ON (target.PageId = source.PageId AND target.RenderingId = source.RenderingId AND target.DatasourceId = source.DatasourceId AND target.LinkFieldId = source.LinkFieldId)
            WHEN MATCHED THEN 
                UPDATE SET ClickCount = ClickCount + 1
            WHEN NOT MATCHED BY TARGET THEN 
                INSERT (PageId, RenderingId, DatasourceId, LinkFieldId, ClickCount) VALUES (@PageId, @RenderingId, @DatasourceId, @LinkFieldId, 1);";

                var param = new
                {
                    PageId = pageId.Value,
                    RenderingId = renderingId.Value,
                    DatasourceId = datasourceId.Value,
                    LinkFieldId = linkFieldId.Value
                };

                var affectedRows = await DbManager.Custom.ExecuteNonQueryAsync(strSql, param, CommandType.Text);
                result = affectedRows != 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return result;
        }
    }
}
