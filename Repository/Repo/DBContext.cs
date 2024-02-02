using Microsoft.Data.SqlClient;
using WebApplication_Inlamning_P_3.Repository.Interfaces;

namespace WebApplication_Inlamning_P_3.Repository.Repo
{
    public class DBContext :IDBContext
    {
        private readonly string? _connString;
        public DBContext(IConfiguration config)
        {
            _connString = config.GetConnectionString("DBConnection");

        }
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connString);

        }
    }
}
