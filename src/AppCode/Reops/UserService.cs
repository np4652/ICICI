using ICICI.AppCode.Interfaces;
using ICICI.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ICICI.AppCode.Reops.Entities;
using ICICI.AppCode.DAL;
using Dapper;
using System.Data;

namespace ICICI.AppCode.Reops
{
    public class UserService : IUserService
    {
        private IDapperRepository _dapper;
        public UserService(IDapperRepository dapper)
        {
            _dapper = dapper;
        }

        public Task<Response> AddAsync(ApplicationUser entity)
        {
            throw new NotImplementedException();
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

        public Task<Response> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationUser>> GetAllAsync(ApplicationUser entity = null)
        {
            throw new NotImplementedException();
        }

        public Task<IResponse<List<BankSetting>>> GetBankSetting(int id, int userId, string Role = "")
        {
            throw new NotImplementedException();
        }

        public Task<Response<ApplicationUser>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> GetDetails(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ApplicationUser>> GetDropdownAsync(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }
    }
}