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

        public async Task PostStatementAsync(PostStatetmentRequest postStatementRequest)
        {
            var apiConfig = _apiConfig.Where(x => x.Name.Equals("postStatement", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            var headers = new Dictionary<string, string>
            {
                { "API_KEY",apiConfig.APIKey}
            };
            var newList = new List<ICICTransactionDetail>();
            postStatementRequest.data.ForEach(obj => {
                newList.Add(new ICICTransactionDetail
                {
                    TransactionId = obj.Transaction_ID,
                    TransactionAmount = obj.Transaction_Amount_INR_,
                    TransactionType = obj.Cr_Dr,
                    TransactionDate = obj.Txn_Posted_Date,
                    TransactionPostedDate = obj.Txn_Posted_Date,
                    AvailableBalance = obj.Available_Balance_INR_,
                    ChequeRefNo = obj.ChequeNo_,
                    SlNo = obj.No_,
                    TransactionRemarks = obj.Description,
                    ValueDate = obj.Value_Date
                });
            });
            var PostICICIStatementRequest = new PostICICIStatementRequest
            {
                AccountNo = postStatementRequest.AccountNo,
                data = newList
            };
            string reponse = await AppWebRequest.O.PostJsonDataUsingHWRTLS(apiConfig.Url, PostICICIStatementRequest, headers).ConfigureAwait(false);
        }
    }
}
