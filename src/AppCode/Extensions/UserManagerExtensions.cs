using ICICI.Models;
using LinqToDB;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ICICI.AppCode.Extensions
{
    public static class UserManagerExtensions
    {
        public static Task<ApplicationUser> FindByNameAsync(this UserManager<ApplicationUser> userManager, string name)
        {
            return userManager?.Users?.FirstOrDefaultAsync(um => um.UserName == name);
        }
        public static ApplicationUser FindByMobileNo(this UserManager<ApplicationUser> userManager, string mobileNo)
        {
            return new ApplicationUser();
        }
    }
}
