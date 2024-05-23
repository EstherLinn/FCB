using CsvHelper.Configuration.Attributes;
using System;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    public class SysjustCompanyFund1
    {
        public string FundCompanyName { get; set; }
        public string FundCompanyCode { get; set; }
        public DateTime EstablishmentDate { get; set; }
        public string Chairman { get; set; }
        public string GeneralManager { get; set; }
        public string Spokesperson { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string CompanyProfile { get; set; }
        public string CustomerServiceContact { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string FundCount { get; set; }
        public string FundSize { get; set; }
    }
}