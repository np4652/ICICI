using System.ComponentModel.DataAnnotations;

namespace ICICI.AppCode.Reops.Entities
{
    public class BankSetting
    {
        
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required]
        public string AccountNo { get; set; }
        [Required]
        public string Password { get; set; }
        public string CustomerId { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
