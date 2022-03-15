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
            string reponse = await AppWebRequest.O.PostJsonDataUsingHWRTLS(apiConfig.Url, postStatementRequest, headers).ConfigureAwait(false);
        }
    }
}
