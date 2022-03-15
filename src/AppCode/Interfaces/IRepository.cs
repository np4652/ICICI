using ICICI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICICI.AppCode.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<Response<T>> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(T entity = null);
        Task<Response> AddAsync(T entity);
        Task<Response> DeleteAsync(int id);
        Task<IReadOnlyList<T>> GetDropdownAsync(T entity);

        Task<T> GetDetails(object id);

        //Task GetByIdAsync(object id);
    }
}
