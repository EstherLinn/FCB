using Foundation.Wealth.Manager;
using Foundation.Wealth.Models;
using Sitecore.Configuration;
using System;
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
                string signal = results?.Name.ToString();
                return signal;
            }
            else
            {
                return EnumUtil.GetEnumDescription(name);
            }
        }
    }
}
