using Foundation.Wealth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.ScheduleAgent.Models.SignalStatus
{
    public class SignalStatus
    {
        public int Number { get; set; }
        public string SignalName { get; set; }
        public TrafficLightStatus Status { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
