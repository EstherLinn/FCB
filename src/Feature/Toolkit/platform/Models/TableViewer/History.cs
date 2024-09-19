using System;

namespace Feature.Wealth.Toolkit.Models.TableViewer
{
    internal class History
    {
        /// <summary>
        /// 唯一識別碼
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 類別
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 執行動作
        /// </summary>
        /// <remarks>執行的動作類型，如 Create、Save、Delete 等操作。</remarks>
        public string Action { get; set; }

        /// <summary>
        /// 項目Id
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// 項目語言
        /// </summary>
        public string ItemLanguage { get; set; }

        /// <summary>
        /// 項目版本號
        /// </summary>
        public int ItemVersion { get; set; }

        /// <summary>
        /// 項目路徑
        /// </summary>
        public string ItemPath { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        /// <remarks>執行此次操作的使用者名稱，通常是 Sitecore 使用者帳號。</remarks>
        public string UserName { get; set; }

        /// <summary>
        /// 任務堆疊資訊
        /// </summary>
        /// <remarks>記錄執行此變更的任務堆疊資訊，可能包括操作過程中的相關堆疊資訊。</remarks>
        public string TaskStack { get; set; }

        /// <summary>
        /// 附加資訊
        /// </summary>
        /// <remarks>用來記錄其他相關資訊，如操作的附註或詳細資料。</remarks>
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime Created { get; set; }
    }
}