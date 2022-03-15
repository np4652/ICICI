using ICICI.AppCode.Interfaces;
using ICICI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ICICI.AppCode.Extensions;
using System.Linq;
using ICICI.AppCode.Reops.Entities;
using System.Collections.Generic;
using System.Text;
using System;
using Hangfire;
using ICICI.AppCode.CustomAttributes;
using ICICI.AppCode.Helper;
using Hangfire.SqlServer;

namespace ICICI.Controllers
{
    [CanBePaused]
    public class UserController : Controller
    {
        private IUserService _users;
        private IAPIServices _apiServices;
        private IBankService _bankService;
        private readonly string Connectionstring;
        public UserController(IHttpContextAccessor httpContext, IUserService users, IAPIServices apiServices, IBankService bankService, IConnectionString connectionString)
        {
            _users = users;
            _apiServices = apiServices;
            _bankService = bankService;
            Connectionstring = connectionString.connectionString;
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