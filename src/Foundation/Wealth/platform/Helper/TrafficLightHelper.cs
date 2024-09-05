using Foundation.Wealth.Manager;
using Foundation.Wealth.Models;
using System;
using System.Data;

namespace Foundation.Wealth.Helper
{
    internal class TrafficLightHelper
    {
        ///<summary>
        ///判別紅綠燈
        ///</summary>
        public string CheckTrafficLight(NameofTrafficLight name)
        {
            var param = new { number = name };
            //查sp或function在sql裡面return table名稱
            string sql = """
                SELECT Status
                FROM [SignalStatus]
                WHERE Number = @number
                """;

            var results = DbManager.Custom.Execute<SignalStatus>(sql, param, CommandType.Text);
            string signal = results?.SignalName.ToString();
            return signal;
        }
    }
}
