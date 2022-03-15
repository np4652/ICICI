using Dapper;
using ICICI.AppCode.DAL;
using System.Linq;

namespace ICICI.AppCode.Migrations
{
    public class Database
    {
        private readonly IDapperRepository _context;
        public Database(IDapperRepository context) => _context = context;
        public void CreateDatabase(string dbName)
        {
            var query = "SELECT * FROM sys.databases WHERE name = @name";
            var parameters = new DynamicParameters();
            parameters.Add("name", dbName);
            using (var connection = _context.GetMasterConnection())
            {
                var records = connection.Query(query, parameters);
                if (!records.Any())
                    connection.Execute($"CREATE DATABASE {dbName}");
            }
        }
    }
}