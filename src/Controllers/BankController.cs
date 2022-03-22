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
using Microsoft.AspNetCore.Authorization;

namespace ICICI.Controllers
{
    [Authorize]
    public class BankController : Controller
    {
        private IBankService _bankService;
        //private IUserService _users;
        //private IAPIServices _apiServices;
        //private readonly string Connectionstring;
        public BankController(IHttpContextAccessor httpContext, IUserService users, IAPIServices apiServices, IBankService bankService, IConnectionString connectionString)
        {
            _bankService = bankService;
            //_users = users;
            //_apiServices = apiServices;
            //Connectionstring = connectionString.connectionString;
        }
        public IActionResult Setting()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> _Setting()
        {
            var response = await _bankService.GetBankSetting(0, User.GetLoggedInUserId<int>());
            var entity = response.Result;
            return PartialView("partial/_Setting", entity ?? new List<BankSetting>());
        }

        [HttpPost]
        public async Task<IActionResult> EditSetting(int id = -1)
        {
            var response = await _bankService.GetBankSetting(id, User.GetLoggedInUserId<int>());
            var entity = response.Result?.FirstOrDefault();
            return PartialView(entity ?? new BankSetting());
        }

        [HttpPost]
        public async Task<IActionResult> SaveSetting(BankSetting bankSetting)
        {
            bankSetting.UserId = User.GetLoggedInUserId<int>();
            var response = await _bankService.AddBankSetting(bankSetting);
            return Json(response);
        }
    }
}
