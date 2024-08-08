using System;

namespace Feature.Wealth.Toolkit.Models.TableViewer
{
    internal class History
    {
        public Guid Id { get; set; }
        public string Category { get; set; }
        public string Action { get; set; }
        public Guid ItemId { get; set; }
        public string ItemLanguage { get; set; }
        public int ItemVersion { get; set; }
        public string ItemPath { get; set; }
        public string UserName { get; set; }
        public string TaskStack { get; set; }
        public string AdditionalInfo { get; set; }
        public DateTime Created { get; set; }
    }
}