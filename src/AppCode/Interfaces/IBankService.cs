using ICICI.AppCode.Reops.Entities;
using ICICI.AppCode.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ICICI.AppCode.Interfaces
{
    public interface IBankService
    {
        Task<IResponse> AddBankSetting(BankSetting bankSetting);
        Task<IResponse<List<BankSetting>>> GetBankSetting(int id, int userId, string Role = "",bool forJobOnly = false);
        Task<IResponse> SaveFetchResponse(Datum data, string AccountNo);
        Task<IEnumerable<FetchStatementLog>> GetFetchRecordFromDB(string AccountNo, string date);
    }
}
