using ICICI.AppCode.Interfaces;
using ICICI.AppCode.Reops.Entities;
using ICICI.AppCode.DAL;
using ICICI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ICICI.AppCode.Reops
{
    public class BankService : IBankService
    {
        private IDapperRepository _dapper;
        public BankService(IDapperRepository dapper)
        {
            _dapper = dapper;
        }
        public async Task<IResponse> AddBankSetting(BankSetting bankSetting)
        {
            var res = await _dapper.ExecuteAsync("aproc_Add_Bank_Setting", bankSetting, commandType: CommandType.StoredProcedure);
            return new Response
            {
                StatusCode = res != -1 ? ResponseStatus.Success : ResponseStatus.Failed,
                ResponseText = res != -1 ? ResponseStatus.Success.ToString() : ResponseStatus.Failed.ToString(),
            };
        }

        public async Task<IResponse> SaveFetchResponse(TransactionDetail data, string AccountNo)
        {
            string sqlQuery = @"insert into FetchStatementLog(AccountNo,tranID,EntryOn) Values (@AccountNo,@tranID,GetDate())";
            var res = await _dapper.ExecuteAsync(sqlQuery,
                new
                {
                    AccountNo,
                    tranID = string.Concat((data.Transaction_ID ?? string.Empty)," " ,(data.Tran_ID ?? string.Empty))
                }, commandType: CommandType.Text);
            return new Response
            {
                StatusCode = res != -1 ? ResponseStatus.Success : ResponseStatus.Failed,
                ResponseText = res != -1 ? ResponseStatus.Success.ToString() : ResponseStatus.Failed.ToString(),
            };
        }

        public async Task<IEnumerable<FetchStatementLog>> GetFetchRecordFromDB(string AccountNo, string date)
        {
            string sqlQuery = @"select * from FetchStatementLog(nolock) where AccountNo=@AccountNo and Cast(EntryOn as Date) = Cast(@date as Date)";
            var res = await _dapper.GetAllAsync<FetchStatementLog>(sqlQuery, new { AccountNo, date = date ?? DateTime.Now.ToString("dd MMM yyyy") }, commandType: CommandType.Text);
            return res ?? new List<FetchStatementLog>();
        }
        public async Task<IResponse<List<BankSetting>>> GetBankSetting(int id, int userId, string Role = "", bool forJobOnly = false)
        {
            var response = new Response<List<BankSetting>>();
            string sqlQuery = @"Declare @Role varchar(30);
                                select @Role = r.Name from UserRoles u(nolock) inner join ApplicationRole r(nolock) on r.Id=u.RoleId where u.UserId=@UserId
                                select * from BankSetting(nolock) where (Id = @id or @id=0) and (userId=@userId or @Role='Admin')";
            if (forJobOnly)
                sqlQuery = @"select * from BankSetting(nolock) where IsActive=1 and  DateDiff(MINUTE,ISNULL(LastUpdatedOn,GETDATE()-1),GetDate()) > = ISNULL(duration,0)
                             Update BankSetting Set LastUpdatedOn = GETDATE() where IsActive = 1 and DateDiff(MINUTE, ISNULL(LastUpdatedOn, GETDATE()-1),GetDate()) > = ISNULL(duration, 0)";
            var res = await _dapper.GetAllAsync<BankSetting>(sqlQuery, new { id, userId }, commandType: CommandType.Text);
            if (res != null)
                response = new Response<List<BankSetting>>
                {
                    StatusCode = ResponseStatus.Success,
                    ResponseText = ResponseStatus.Success.ToString(),
                    Result = res.ToList()
                };
            return response;
        }
    }
}
