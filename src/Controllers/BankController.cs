using ICICI.AppCode.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ICICI.AppCode.Extensions;
using System.Linq;
using ICICI.AppCode.Reops.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace ICICI.Controllers
{
    [Authorize]
    public class BankController : Controller
    {
        private IBankService _bankService;
        public BankController(IBankService bankService)
        {
            _bankService = bankService;
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
