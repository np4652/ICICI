using ICICI.AppCode.Reops.Entities;
using System;
using System.Threading.Tasks;

namespace ICICI.AppCode.Interfaces
{
    public interface IAPIServices:IDisposable
    {
        Task<FetchStatement> FetchStatementAsync(string url);
        Task PostStatementAsync(string url, PostStatetmentRequest postStatementRequest);
    }
}
