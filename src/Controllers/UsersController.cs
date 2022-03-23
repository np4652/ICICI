using ICICI.AppCode.Interfaces;
using ICICI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ICICI.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private IUserService _users;
        public UserController(IUserService users)
        {
            _users = users;
        }

        [Route("User/Index/{role}")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UsersList()
        {
            var users = await _users.GetAllAsync();
            return PartialView("~/Views/Account/PartialView/_UsersList.cshtml", users);
        }

        [HttpPost]
        public IActionResult Edit(string role)
        {
            return PartialView("~/Views/Account/PartialView/_Register.cshtml", new RegisterViewModel { IsAdmin = true, RoleType = role });
        }
    }
}