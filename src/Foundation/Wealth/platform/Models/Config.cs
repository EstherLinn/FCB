using Foundation.Wealth.Helper;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Wealth.Models
{
    /// <summary>
    /// 設定檔
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 環境定義 取得當前環境
        /// </summary>
        public static string CuntextEnvironment = new RoleConfigurationHelper().DefinedEnvironment.FirstOrDefault();

        /// <summary>
        ///  檢查開發環境 是true/否false
        /// </summary>
        private static bool IsCheckDevelop
        {
            get
            {
                var isDevelop = false;
                if (string.IsNullOrEmpty(CuntextEnvironment) || CuntextEnvironment == "LOCAL" || CuntextEnvironment == "DEVELOPMENT")
                {
                    isDevelop = true;
                }
                return isDevelop;
            }
        }

        /// <summary>
        /// 環境檢查
        /// </summary>
        public static bool IsEnableCheck = !IsCheckDevelop;

        /// <summary>
        /// Api HttpVerb GET
        /// </summary>
        public const string Get = "GET";

        /// <summary>
        /// Api HttpVerb POST
        /// </summary>
        public const string Post = "POST";

        /// <summary>
        /// 環境排程
        /// </summary>
        public static string CuntextScheduleAgent = new RoleConfigurationHelper().DefinedScheduleAgent.FirstOrDefault();

        /// <summary>
        /// 檢查排程環境 是true/否false
        /// </summary>
        private static bool IsCheckSiteCronDevelop
        {
            get
            {
                var isAgent = false;
                if (CuntextScheduleAgent == "SCHEDULEAGENT")
                {
                    isAgent = true;
                }
                return isAgent;
            }
        }

        /// <summary>
        /// 環境排程
        /// </summary>
        public static bool IsAgentEnableCheck = IsCheckSiteCronDevelop;



    }
}