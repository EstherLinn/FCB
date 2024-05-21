using System;
using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.VisitCount
{
    public class VisitCountModel
    {
        public Guid PageId { get; set; }
        public int VisitCount { get; set; }
        public string QueryStrings { get; set; }
    }

    public class VisitInfo
    {
        public int VisitCount { get; set; }
        public Dictionary<string, string> QueryStrings { get; set; }
    }
}