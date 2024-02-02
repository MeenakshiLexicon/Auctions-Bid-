using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApplication_Inlamning_P_3.Models.Entities;
using WebApplication_Inlamning_P_3.Repository.Interfaces;

namespace WebApplication_Inlamning_P_3.Repository.Repo
{
    public class LoanRepo : ILoanRepo
    {
        private readonly IDBContext _context;

        public LoanRepo(IDBContext context)
        {
            _context = context;
        }

        public void PostLoanForCustomer(int customerId, decimal amount, int duration, decimal payments, int accountId)
        {
            try
            {
                using (IDbConnection db = _context.GetConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@CustomerId", customerId);
                    parameters.Add("@Amount", amount);
                    parameters.Add("@Duration", duration);
                    parameters.Add("@Payments", payments);
                    parameters.Add("@AccountId", accountId);

                    db.Execute("PostLoanForCustomer", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while posting a loan: {ex.Message}");
                throw; // Handle or log the exception as needed
            }

        }
        
    }
}
