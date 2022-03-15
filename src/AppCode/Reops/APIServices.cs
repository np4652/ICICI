using ApiRequestUtility;
using ICICI.AppCode.Interfaces;
using ICICI.AppCode.Reops.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ICICI.AppCode.Reops
{
    public class APIServices : IAPIServices
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<FetchStatement> FetchStatementAsync(string url)
        {
            var response = await AppWebRequest.O.CallUsingHttpWebRequest_GETAsync(url);
            var result = JsonConvert.DeserializeObject<FetchStatement>(response);
            return result ?? new FetchStatement();
        }

        public async Task PostStatementAsync(string url, PostStatetmentRequest postStatementRequest)
        {
            var headers = new Dictionary<string, string>
            {

            };
            string reponse = await AppWebRequest.O.PostJsonDataUsingHWRTLS(url, postStatementRequest, headers).ConfigureAwait(false);
        }
    }
}
