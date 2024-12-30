using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.ScheduleAgent.Models.DataCountSettings
{
    public class DataCountSettings
    {
        //後臺設定:數量值
        public int? Number { get; set; }

        //後臺設定:是否勾選
        public bool IsChecked { get; set; }

        //後臺設定:是否大於XXX值
        public bool IsUpto { get; set; }
    }
}
