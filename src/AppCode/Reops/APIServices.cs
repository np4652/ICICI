using ApiRequestUtility;
using ICICI.AppCode.Interfaces;
using ICICI.AppCode.Reops.Entities;
using ICICI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICICI.AppCode.Reops
{
    public class APIServices : IAPIServices
    {
        private readonly List<APIConfig> _apiConfig;
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public APIServices(List<APIConfig> apiConfig)
        {
            _apiConfig = apiConfig;
        }
        public async Task<FetchStatement> FetchStatementAsync(string url)
        {
            var response = await AppWebRequest.O.CallUsingHttpWebRequest_GETAsync(url);
            var result = JsonConvert.DeserializeObject<FetchStatement>(response);
            return result ?? new FetchStatement();
        }

        public async Task PostStatementAsync(string baseUrl, string apiKey, PostStatetmentRequest postStatementRequest)
        {
            //var apiConfig = _apiConfig.Where(x => x.Name.Equals("postStatement", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            var headers = new Dictionary<string, string>
            {
                //{ "API_KEY",apiConfig.APIKey}
                { "API_KEY",apiKey}
            };
            var newList = new List<ICICTransactionDetail>();
            postStatementRequest.data.ForEach(obj =>
            {
                decimal withdrawl = 0;
                decimal deposite = 0;
                decimal.TryParse(obj.Deposit_Amt_INR_ ?? string.Empty, out deposite);
                decimal.TryParse(obj.Withdrawal_Amt_INR_ ?? string.Empty, out withdrawl);
                newList.Add(new ICICTransactionDetail
                {
                    TransactionId = obj.Transaction_ID ?? obj.Tran_ID,
                    TransactionAmount = obj.Transaction_Amount_INR_ ?? Convert.ToString(withdrawl + deposite),
                    TransactionType = obj.Cr_Dr ?? (withdrawl > 0 ? "dr" : "cr"),
                    TransactionDate = obj.Txn_Posted_Date ?? obj.Transaction_Date,
                    TransactionPostedDate = obj.Txn_Posted_Date ?? obj.Transaction_Posted_Date,
                    AvailableBalance = obj.Available_Balance_INR_ ?? obj.Balance_INR_,
                    ChequeRefNo = obj.ChequeNo_ ?? obj.Cheque_No_Ref_No_,
                    SlNo = obj.No_ ?? string.Empty,
                    TransactionRemarks = obj.Description ?? obj.Transaction_Remarks,
                    ValueDate = obj.Value_Date
                });
            });
            if (newList != null && newList.Count > 0)
            {
                var PostICICIStatementRequest = new PostICICIStatementRequest
                {
                    AccountNo = postStatementRequest.AccountNo,
                    data = newList
                };
                //apiConfig.Url
                if(!baseUrl.Contains("/ProcessICICIStatement"))
                    baseUrl = string.Concat(baseUrl, "/ProcessICICIStatement");
                string reponse = await AppWebRequest.O.PostJsonDataUsingHWRTLS(baseUrl, PostICICIStatementRequest, headers).ConfigureAwait(false);
            }
        }
    }
}
