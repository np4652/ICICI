namespace ICICI.AppCode.Reops.Entities
{
    public class EmailConfig
    {
        public int Id { get; set; }
        public string EmailFrom { get; set; }
        public string Password { get; set; }
        public bool EnableSSL { get; set; }
        public int Port { get; set; } = 587;
        public string HostName { get; set; }
    }
}
