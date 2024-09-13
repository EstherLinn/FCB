using Foundation.Wealth.Manager;
using Foundation.Wealth.Models;
using Sitecore.Configuration;
using System.Data;
using Xcms.Sitecore.Foundation.Basic.Extensions;

namespace Foundation.Wealth.Helper
{
    public static class TrafficLightHelper
    {
        ///<summary>
        ///判別紅綠燈，取得對應資料表名稱
        ///</summary>
        public static string GetTrafficLightTable(NameofTrafficLight name)
        {
            //先偵測總開關有沒有開
            bool masterSwitch = Settings.GetBoolSetting("MasterLightSwitch", false);
            var tablename = EnumUtil.GetEnumDescription(name);

            if (masterSwitch)
            {
                var param = new { number = name };

                //查sp或function在sql裡面return table名稱
                string sql = """
                            SELECT Name
                            FROM [SignalStatus] WITH (NOLOCK)
                            WHERE Number = @number
                            """;

                var results = DbManager.Custom.Execute<SignalStatus>(sql, param, CommandType.Text);

                if (results == null)
                {
                    return tablename;
                }
                if (results.Status == 0)
                {
                    tablename = results.Name + "_Process";
                }
                else if (results.Status == 1)
                {
                    tablename = results.Name;
                }
            }
            return tablename;
        }
    }
}
