using System;

namespace Feature.Wealth.Component.Models.VisitCount
{
    public class VisitCountModel
    {
        public Guid PageId { get; set; }
        public int VisitCount { get; set; }
        public string QueryStrings { get; set; }
    }
}