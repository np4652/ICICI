using Dapper;
using ICICI.AppCode.Reops.Entities;
using ICICI.AppCode.DAL;
using ICICI.AppCode.Interfaces;
using ICICI.AppCode.Reops.Entities;
using ICICI.Models;
using ICICI.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ICICI.AppCode.Reops
{
    public class UsersRepo : IUserService
    {
        private IDapperRepository _dapper;
        public UsersRepo(IDapperRepository dapper)
        {
            _dapper = dapper;
        }



        public Task<Response> AddAsync(ApplicationUser entity)
        {
            throw new System.NotImplementedException();
        }

       
        public Task<Response> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<ApplicationUser>> GetAllAsync(ApplicationUser entity = null)
        {
            List<ApplicationUser> res = new List<ApplicationUser>();

            try
            {
                var dbparams = new DynamicParameters();

                var ires = await _dapper.GetAllAsync<ApplicationUser>("proc_users", dbparams, commandType: CommandType.StoredProcedure);
                res = ires.ToList();
            }
            catch (Exception ex)
            { }
            return res;
        }

        public Task<Response<ApplicationUser>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> GetDetails(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ApplicationUser>> GetDropdownAsync(ApplicationUser entity = null)
        {
            throw new NotImplementedException();
        }
    }

}
