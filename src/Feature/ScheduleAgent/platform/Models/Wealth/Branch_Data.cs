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

        [FixedWidthLineField(Start = 4, Length = 8)]
        public string BranchName { get; set; }

        [FixedWidthLineField(Start = 12, Length = 25)]
        public string BranchAddress { get; set; }

        [FixedWidthLineField(Start = 37, Length = 3)]
        public string PhoneAreaCode { get; set; }

        [FixedWidthLineField(Start = 40, Length = 8)]
        public string PhoneNumber { get; set; }

        [FixedWidthLineField(Start = 48, Length = 1)]
        public string OperationalStatus { get; set; }

        [FixedWidthLineField(Start = 49, Length = 3)]
        public string ConvertedBranchCode { get; set; }

        [FixedWidthLineField(Start = 52, Length = 10)]
        public string Latitude { get; set; }

        [FixedWidthLineField(Start = 62, Length = 11)]
        public string Longitude { get; set; }

        [FixedWidthLineField(Start = 73, Length = 3)]
        public string City { get; set; }

        [FixedWidthLineField(Start = 76, Length = 3)]
        public string District { get; set; }

        public DefaultConfig GetDefaultConfig(int structureTypeId)
        {
            var defaultConfig = new DefaultConfig();

            return defaultConfig;

        }
    }
}
