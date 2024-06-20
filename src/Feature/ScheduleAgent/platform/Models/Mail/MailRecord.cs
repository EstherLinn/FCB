using Newtonsoft.Json;
using System;
namespace Feature.Wealth.ScheduleAgent.Models.Mail
{
     public  class MailRecord
    {
        public int RecordNumber { get; set; }

        [JsonIgnore]
        public string PlatFormId { get; set; }

        public DateTime InfoDateTime { get; set; }

        public string MailInfoType { get; set; }
        public string InfoContent { get; set; }
        public string InfoLink { get; set; }

        public bool HaveRead { get; set; }
    }
}
