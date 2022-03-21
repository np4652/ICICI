using System.ComponentModel.DataAnnotations;

namespace ICICI.AppCode.Reops.Entities
{
    public class BankSetting
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage ="Account Number is mendetory to fill")]
        public string AccountNo { get; set; }
        [Required]
        public string Password { get; set; }
        [Required(ErrorMessage = "CustomerId(Bank Login Id) is mendetory to fill")]
        public string CustomerId { get; set; }
        [Required]
        public int Duration { get; set; }        
        public bool IsActive { get; set; }
        
        [Required(ErrorMessage ="Project name is required")]
        public string ProjectName { get; set; }
        [Required(ErrorMessage = "Base url is required")]
        public string BaseUrl { get; set; }
        [Required(ErrorMessage = "API Key is required")]
        public string APIKey { get; set; }
    }
}
