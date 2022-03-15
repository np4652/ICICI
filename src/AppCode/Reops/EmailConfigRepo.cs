using Dapper;
using ICICI.AppCode.DAL;
using ICICI.AppCode.Extensions;
using ICICI.AppCode.Interfaces;
using ICICI.AppCode.Reops.Entities;
using ICICI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ICICI.AppCode.Reops
{
    public class EmailConfigRepo : IRepository<EmailConfig>
    {
        private IDapperRepository _dapper;
        public EmailConfigRepo(IDapperRepository dapper)
        {
            _dapper = dapper;
        }
        public async Task<Response> AddAsync(EmailConfig entity)
        {
            throw new System.NotImplementedException();
        }




        public async Task<Response> DeleteAsync(int id)
        {
            //Response res = new Response();
            //try
            //{
            //    var dbparams = new DynamicParameters();
            //    dbparams.Add("CategoryID", id);
            //    dbparams.Add("CategoryName", "");
            //    dbparams.Add("ParentID", 0);
            //    dbparams.Add("Icon", "");
            //    dbparams.Add("QueryType", "D");
            //    res = await _dapper.GetAsync<Response>("proc_Category", dbparams, commandType: CommandType.StoredProcedure);
            //}
            //catch (Exception ex)
            //{
            //    res.Exception = ex;
            //}
            //return res;
            throw new System.NotImplementedException();
        }

        //public async Task<IEnumerable<EmailConfig>> GetAllAsync(EmailConfig entity = null)
        //{
        //    string sqlQuery = @"Select * from EmailConfig(nolock)";
        //    entity = entity == null ? new EmailConfig() : entity;
        //    var prepared = _dapper.PrepareParameters(sqlQuery, entity.ToDictionary());
        //    var res = await _dapper.GetAllAsync<EmailConfig>(prepared.preparedQuery, prepared.dynamicParameters, commandType: CommandType.Text);
        //    return res ?? new List<EmailConfig>();
        //}

        public async Task<IEnumerable<EmailConfig>> GetAllAsync(EmailConfig entity = null)
        {
            string sqlQuery = @"Select * from EmailConfig(nolock)";
            entity = entity == null ? new EmailConfig() : entity;
            var res = await _dapper.GetAllAsync<EmailConfig>(entity, sqlQuery);
            return res ?? new List<EmailConfig>();
        }

        public async Task<Response<EmailConfig>> GetByIdAsync(int id)
        {
            var response = new Response<EmailConfig>
            {
                StatusCode = ResponseStatus.Failed
            };
            var dbparams = new DynamicParameters();
            dbparams.Add("ID", id);
            string sqlQuery = @"Select * from EmailConfig(nolock) where ID=@Id";
            var result = await _dapper.GetAsync<EmailConfig>(sqlQuery, dbparams, commandType: CommandType.Text);
            if (result != null)
            {
                response = new Response<EmailConfig>
                {
                    StatusCode = ResponseStatus.Success,
                    Result = result
                };
            }
            return response;
        }

        public Task<EmailConfig> GetDetails(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<EmailConfig>> GetDropdownAsync(EmailConfig entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
