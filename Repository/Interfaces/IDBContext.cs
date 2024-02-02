using Microsoft.Data.SqlClient;

namespace WebApplication_Inlamning_P_3.Repository.Interfaces
{
    public interface IDBContext
    {
      SqlConnection GetConnection();
    }
}
