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
    }
}
