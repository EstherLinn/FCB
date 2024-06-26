using Feature.Wealth.Component.Models;
using Foundation.Wealth.Manager;
using log4net;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Xcms.Sitecore.Foundation.Basic.Logging;

namespace Feature.Wealth.Component.Repositories
{
    public class SearchKeywordsRepository
    {
        private ILog Log { get; } = Logger.General;

        /// <summary>
        /// 儲存搜尋關鍵字資訊
        /// </summary>
        /// <param name="pageId">頁面 Id</param>
        /// <param name="keyword">搜尋的關鍵字</param>
        /// <param name="productType">產品類別</param>
        /// <returns>儲存成功回傳true，失敗則回傳false</returns>
        public async Task<bool> InsertSearchKeywords(Guid? pageId, string keyword, string productType)
        {
            try
            {
                if (Enum.GetNames(typeof(ProductTypeEnum)).Contains(productType))
                {
                    await DbManager.Custom.ExecuteNonQueryAsync(@"
                INSERT INTO [dbo].[SearchKeywords] ([PageId], [ProductType], [SearchKeywords],[UpdateTime])
                VALUES (@PageId, @ProductType, @SearchKeywords, @UpdateTime)", new
                    {
                        PageId = pageId.Value,
                        ProductType = productType,
                        SearchKeywords = keyword,
                        UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    }, CommandType.Text) ;

                    return true;
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

            return false;
        }
    }
}