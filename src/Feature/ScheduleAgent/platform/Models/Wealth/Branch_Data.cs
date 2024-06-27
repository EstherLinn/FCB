using FixedWidthParserWriter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    public class BranchData : IFixedWidth
    {
        [FixedWidthLineField(Start = 1, Length = 3)]
        public string BranchCode { get; set; }

        [FixedWidthLineField(Start = 4, Length = 16)]
        public string BranchName { get; set; }

        [FixedWidthLineField(Start = 20, Length = 50)]
        public string BranchAddress { get; set; }

        [FixedWidthLineField(Start = 70, Length = 3)]
        public string PhoneAreaCode { get; set; }

        [FixedWidthLineField(Start = 73, Length = 8)]
        public string PhoneNumber { get; set; }

        [FixedWidthLineField(Start = 81, Length = 1)]
        public string OperationalStatus { get; set; }

        [FixedWidthLineField(Start = 82, Length = 3)]
        public string ConvertedBranchCode { get; set; }

        [FixedWidthLineField(Start = 85, Length = 10)]
        public string Latitude { get; set; }

        [FixedWidthLineField(Start = 95, Length = 11)]
        public string Longitude { get; set; }

        [FixedWidthLineField(Start = 106, Length = 6)]
        public string City { get; set; }

        [FixedWidthLineField(Start = 112, Length = 6)]
        public string District { get; set; }

        public DefaultConfig GetDefaultConfig(int structureTypeId)
        {
            var defaultConfig = new DefaultConfig();

            return defaultConfig;

        }
    }
}
