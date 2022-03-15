using ICICI.AppCode.Reops.Entities;

namespace ICICI.Models
{
    public class EmailSettings: EmailConfig
    {
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
