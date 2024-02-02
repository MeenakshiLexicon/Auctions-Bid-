using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Transactions;
using WebApplication_Inlamning_P_3.Models.Entities;
using WebApplication_Inlamning_P_3.Repository.Interfaces;
using static WebApplication_Inlamning_P_3.Models.Entities.User;

namespace WebApplication_Inlamning_P_3.Repository.Repo
{
    public class UserRepo : IUserRepo
    {
      private readonly IDBContext _context;

    public UserRepo(IDBContext context)
    {
            _context = context;
    }

        
       public async Task<User> GetUserByUsernameAndPassword(string userName, string password)
       {
                // Implement the logic to fetch the user from the database based on the username and password

                using (var db = _context.GetConnection())
                {
                    var users = await db.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE UserName = @UserName AND Password = @Password",
                       new { UserName = userName, Password = password });
                    DynamicParameters parameters = new DynamicParameters();
                    //parameters.Add("@UserId", user.UserId);
                    parameters.Add("@UserType", users.UserType);

               return users;

            }
       }

       public List<User> GetAllUser()
       {
                try
                {
                    using (IDbConnection db = _context.GetConnection())
                    {
                        return db.Query<User>("GetAllUsers", commandType: CommandType.StoredProcedure).ToList();
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception details (replace Console.WriteLine with your logging mechanism)
                    Console.WriteLine($"An error occurred in GetAllUsers: {ex.Message}");
                    throw;
                }
       }
        
       public Account AddUser(User user, Customer customer, Account account)
       {
         // Check if the current user has a specific claim, for example, "AdminClaim"
         if (user.ExecuterRole != "Admin")
         {
                    // If not an admin, throw an exception, log, or handle as needed
         throw new UnauthorizedAccessException("Only administrators can add new users.");
         }
           else
           {
                try
                {
                     using (IDbConnection db = _context.GetConnection())
                     {
                          DynamicParameters parameters = new DynamicParameters();
                           
                          parameters.Add("@UserName", user.UserName);
                          parameters.Add("@Password", user.Password);
                          parameters.Add("@UserType", user.UserType);
                          parameters.Add("@GivenName", customer.Givenname);
                          parameters.Add("@Surname", customer.Surname);
                          parameters.Add("@Gender", customer.Gender);
                          parameters.Add("@City", customer.City);
                          parameters.Add("@Streetaddress", customer.Streetaddress);
                          parameters.Add("@Zipcode", customer.Zipcode);
                          parameters.Add("@Country", customer.Country);
                          parameters.Add("@CountryCode", customer.CountryCode);
                          parameters.Add("@Birthday", customer.Birthday);
                          parameters.Add("@Telephonecountrycode", customer.Telephonecountrycode);
                          parameters.Add("@Telephonenumber", customer.Telephonenumber);
                          parameters.Add("@Emailaddress", customer.Emailaddress);
                          parameters.Add("@AccountType", account.AccountType);
                          parameters.Add("@Frequency", account.Frequency);
                          parameters.Add("@Balance", account.Balance);


                        return db.Query<Account>("AddUserWithBankAccountAndCustomer", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                            
                    }
                     }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred in Create New User Accounts: {ex.Message}");
                    throw;
                }
            }
       }

       public List<UserAccount> GetUserDetails(string userName)
       {
            try
            {
                using (IDbConnection db = _context.GetConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserName", userName);

                    return db.Query<UserAccount>("GetUserAccountDetails", parameters, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception as per your application's requirements
                Console.WriteLine($"An error occurred in GetUserDetails: {ex.Message}");
                throw; // You may choose to throw the exception or return an empty list, depending on your use case
            }
       }

       public List<AccountTransactions> GetTransactionsOfUser(string username, int accountId)
       {
            try
            {
                using (IDbConnection db = _context.GetConnection())
                {
                    // Assuming Transaction is a model that represents the result of your stored procedure
                    return db.Query<AccountTransactions>("GetTransactionsOfUser",
                        new { Username = username, AccountId = accountId },
                        commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception as per your application's requirements
                Console.WriteLine($"An error occurred in GetTransactionsOfUser: {ex.Message}");
                throw; // You may choose to throw the exception or return an empty list, depending on your use case
            }
       }

       public Account CreateUserAccounts(string username,string accountType, string frequency, decimal balance)
       {
            try
            {
                using (IDbConnection db = _context.GetConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@UserName", username);
                    parameters.Add("@AccountType",accountType);
                    parameters.Add("@Frequency", frequency);
                    parameters.Add("@Balance", balance);


                    return db.Query<Account>("CreateNewUserAccounts", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in Create New User Accounts: {ex.Message}");
                throw;
            }
       }

       public string TransferMoney(string username, int sourceAccountId, int destinationAccountId, decimal amount, string symbol, string bank)
       {
            try
            {
                using (IDbConnection db = _context.GetConnection())
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@UserName", username);
                    parameters.Add("@SourceAccountId", sourceAccountId);
                    parameters.Add("@DestinationAccountId", destinationAccountId);
                    parameters.Add("@Amount", amount);
                    parameters.Add("@Symbol",symbol);
                    parameters.Add("@Bank", bank);
                   


                    return db.QueryFirstOrDefault<string>("TransferMoneyWithinBank", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in Create New User Accounts: {ex.Message}");
                throw;
            }
       }


       private List<T> Query<T>(IDbConnection db, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return db.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType).ToList();
        }
    }

}



