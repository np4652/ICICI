using ICICI.AppCode.Interfaces;
using ICICI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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
    public class TaskController : Controller
    {
        private IAPIServices _apiServices;
        private IBankService _bankService;
        private readonly string Connectionstring;
        private readonly List<APIConfig> _apiConfig;

        public TaskController(IAPIServices apiServices, IBankService bankService, IConnectionString connectionString, List<APIConfig> apiConfig)
        {
            _apiServices = apiServices;
            _bankService = bankService;
            Connectionstring = connectionString.connectionString;
            _apiConfig = apiConfig;
        }

        public ActionResult ScheduleFetchStatement(string startDate, string endDate)
        {
            startDate = string.IsNullOrEmpty(startDate) || string.IsNullOrWhiteSpace(startDate) ? null : startDate;
            endDate = string.IsNullOrEmpty(endDate) || string.IsNullOrWhiteSpace(endDate) ? null : endDate;
            RecurringJob.AddOrUpdate(() => FetchStatement(startDate, endDate), "*/1 * * * *");
            
            return Json("Scheduled");
        }

        public async Task FetchStatement(string startDate, string endDate)
        {
            var res = new FetchStatement();
            var response = await _bankService.GetBankSetting(0, 0, "", true);
            var settings = response.Result;
            if (settings != null)
            {
                foreach (var item in settings)
                {
                    //StringBuilder sb = new StringBuilder(_apiConfig.Where(x => x.Name.Equals("fetchStatement", StringComparison.OrdinalIgnoreCase)).Select(x => x.Url).FirstOrDefault());
                    //sb.Replace("{loginId}", item.CustomerId);
                    //sb.Replace("{pass}", item.Password);
                    //sb.Replace("{accountNo}", item.AccountNo);
                    //sb.Replace("{startDate}", startDate ?? DateTime.Now.ToString("dd-MM-yyyy"));
                    //sb.Replace("{endDate}", endDate ?? DateTime.Now.ToString("dd-MM-yyyy"));
                    //res = await _apiServices.FetchStatementAsync(sb.ToString());
                    string _url = GenrateURL(item, startDate, endDate);
                    res = await _apiServices.FetchStatementAsync(_url);
                    var preFetch = await GetFetchRecordFromDb(item.AccountNo, DateTime.Now.ToString("dd MMM yyyy"));
                    //var newData = res.data.Where(x => !preFetch.Any(y => y.tranID == x.Transaction_ID || y.tranID == x.Tran_ID));
                    var newData = UniqueTransactions(res.data, preFetch.ToList());
                    if (newData.Count() > 0)
                    {
                        PostStatement(item.BaseUrl, item.APIKey, new PostStatetmentRequest { AccountNo = item.AccountNo, data = newData.ToList() });
                        SaveFetchResponse(newData.ToList(), item.AccountNo);
                    }
                }
            }
        }

        private string GenrateURL(BankSetting bankSetting,string startDate,string endDate)
        {
            StringBuilder sb = new StringBuilder(_apiConfig.Where(x => x.Name.Equals("fetchStatement", StringComparison.OrdinalIgnoreCase)).Select(x => x.Url).FirstOrDefault());
            sb.Replace("{loginId}", bankSetting.CustomerId);
            sb.Replace("{pass}", bankSetting.Password);
            sb.Replace("{accountNo}", bankSetting.AccountNo);
            sb.Replace("{startDate}", startDate ?? DateTime.Now.ToString("dd-MM-yyyy"));
            sb.Replace("{endDate}", endDate ?? DateTime.Now.ToString("dd-MM-yyyy"));
            return sb.ToString() ?? string.Empty;
        }
        private IEnumerable<TransactionDetail> UniqueTransactions(List<TransactionDetail> currentFetch, List<FetchStatementLog> preFetch)
        {
            var unique = currentFetch.Where(c => preFetch.All(pre => pre.tranID != c.Transaction_ID && pre.tranID != c.Tran_ID));
            return unique ?? new List<TransactionDetail>();
        }
        public async Task SaveFetchResponse(List<TransactionDetail> data, string AccountNo)
        {
            await Task.Delay(0);
            if (data != null)
            {
                foreach (var item in data)
                {
                    _bankService.SaveFetchResponse(item, AccountNo);
                }
            }
        }

        public async Task PostStatement(string baseUrl, string apiKey, PostStatetmentRequest postStatementRequest)
        {
            await Task.Delay(0);
            _apiServices.PostStatementAsync(baseUrl, apiKey, postStatementRequest);
        }

        public async Task<IEnumerable<FetchStatementLog>> GetFetchRecordFromDb(string AccountNo, string date)
        {
            var res = await _bankService.GetFetchRecordFromDB(AccountNo, date);
            return res;
        }

        public IActionResult PauseTask()
        {
            var storage = new SqlServerStorage(Connectionstring);
            using (var connection = storage.GetConnection())
            {
                connection.Pause(typeof(TaskController));
            }
            return Json("Task paused");
        }

        public IActionResult ResumeTask()
        {
            var storage = new SqlServerStorage(Connectionstring);
            using (var connection = storage.GetConnection())
            {
                connection.Resume(typeof(TaskController));
            }
            return Json("Task Resumed");
        }

        #region Testing

        public ActionResult ScheduleTestTask()
        {
            RecurringJob.AddOrUpdate(() => TestTask(), "*/5 * * * *");
            return Json("Scheduled");
        }

        public async Task TestTask()
        {
            var data = new List<TransactionDetail>();
            for (int i = 1; i < 100; i++)
            {
                data.Add(new TransactionDetail
                {
                    Transaction_ID = string.Concat("tansaction_", i.ToString())
                });
            }
            SaveFetchResponse(data, "123456789");
        }
        #endregion
        public IActionResult ScheduleTestTask1(string baseUrl, string apiKey, PostStatetmentRequest postStatementRequest)
        {
            _apiServices.PostStatementAsync(baseUrl, apiKey, postStatementRequest);
            return Ok("ok");
        }
    }
}

