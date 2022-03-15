using System.ComponentModel.DataAnnotations;

namespace ICICI.Models
{
    public class RegisterViewModel
    {
        public string RoleName { get; set; }
        [Required(ErrorMessage = "Please enter name")]
        [StringLength(200)]
        public string Name { get; set; }
     
        [Required(ErrorMessage = "Please enter Mobile ")]
        [StringLength(10)]
        
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string Mobile { get; set; }
        [Required]

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string PinCode { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and compare password do not match..")]
        public string ConfirmPassword { get; set; }
        public bool IsAdmin { get; set; }
        public string RoleType { get; set; }
    }
}
