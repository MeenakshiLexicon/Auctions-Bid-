using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using WebApplication_Inlamning_P_3.Models.Entities;
using WebApplication_Inlamning_P_3.Repository.Interfaces;
using WebApplication_Inlamning_P_3.Repository.Repo;

namespace WebApplication_Inlamning_P_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly IJWTService _jwtService;
        private IJWTService object1;
        private IUserRepo object2;

        public UserController(IUserRepo repo,IJWTService jWTService)
        {
            _userRepo = repo;
            _jwtService = jWTService;
        }

       

        [HttpGet("accounts")]
        public IActionResult GetUserDetails()
        {
            try
            {
                // Get and inspect Bearer Token
                var TokenHeader = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                // Call a function to validate token
                var isValidToken = _jwtService.ValidateToken(TokenHeader);

                if (!isValidToken)
                {
                    return Unauthorized();
                }
                else
                {
                    // Decode and store token to string
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jsonToken = tokenHandler.ReadToken(TokenHeader) as JwtSecurityToken;
                    string tokenString = jsonToken?.ToString();

                    string ExtractClaim(string claimType)
                    {
                        string claimPrefix = $"\"{claimType}\":\"";
                        int startIndex = tokenString.IndexOf(claimPrefix) + claimPrefix.Length;
                        int endIndex = tokenString.IndexOf("\",\"", startIndex);

                        return tokenString.Substring(startIndex, endIndex - startIndex);
                    }

                    // Extract the role and name claims from decoded token using string function
                    string ExecuterRole = ExtractClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
                    string ExecuterName = ExtractClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");

                    if (ExecuterRole == "User")
                    {
                        var userDetails = _userRepo.GetUserDetails(ExecuterName);
                        if (userDetails == null)
                        {
                            return NotFound("User not found");
                        }

                        return Ok(userDetails);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in GetUserDetails: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }


        [HttpGet("transactions/{accountId}")]
        public IActionResult GetTransactionDetails(int accountId)
        {
            //Get and inspect Bearer Token
            var TokenHeader = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            //Call a function to validate token
            var isValidToken = _jwtService.ValidateToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));

            if (!isValidToken)
            {
                return Unauthorized();
            }
            else
            {
                // Decode and store token to string
                var tokenHandler = new JwtSecurityTokenHandler();
                var jsonToken = tokenHandler.ReadToken(TokenHeader) as JwtSecurityToken;
                string tokenString = jsonToken?.ToString();


                string ExtractClaim(string claimType)
                {
                    string claimPrefix = $"\"{claimType}\":\"";
                    int startIndex = tokenString.IndexOf(claimPrefix) + claimPrefix.Length;
                    int endIndex = tokenString.IndexOf("\",\"", startIndex);

                    return tokenString.Substring(startIndex, endIndex - startIndex);
                }

                // Extract the role and name claims from decoded token using string function
                string ExecuterRole = ExtractClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
                string ExecuterName = ExtractClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
                if (ExecuterRole == "User")
                {
                    try
                    {
                        var transactionDetails = _userRepo.GetTransactionsOfUser(ExecuterName, accountId);

                        if (transactionDetails != null)
                        {
                            return Ok(transactionDetails);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log or handle exceptions
                        return StatusCode(500, $"Internal Server Error: {ex.Message}");
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }

        }

        [HttpPost("create-accounts")]
        public IActionResult CreateAccount(Account account)
        {
            //Get and inspect Bearer Token
            var TokenHeader = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            //Call a function to validate token
            var isValidToken = _jwtService.ValidateToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));

            if (!isValidToken)
            {
                return Unauthorized();
            }
            else
            {
                // Decode and store token to string
                var tokenHandler = new JwtSecurityTokenHandler();
                var jsonToken = tokenHandler.ReadToken(TokenHeader) as JwtSecurityToken;
                if (jsonToken == null)
                {
                    Console.WriteLine("Token could not be properly read.");
                    return BadRequest("Invalid token format");
                }

                string tokenString = jsonToken?.ToString();


                string ExtractClaim(string claimType)
                {
                    string claimPrefix = $"\"{claimType}\":\"";
                    int startIndex = tokenString.IndexOf(claimPrefix) + claimPrefix.Length;
                    int endIndex = tokenString.IndexOf("\",\"", startIndex);

                    return tokenString.Substring(startIndex, endIndex - startIndex);
                }

                // Extract the role and name claims from decoded token using string function
                string ExecuterRole = ExtractClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
                string ExecuterName = ExtractClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
                if (ExecuterRole == "User")
                {
                    if (ExecuterName != null)
                    {
                        var NewAccountDetails = _userRepo.CreateUserAccounts(ExecuterName, account.AccountType, account.Frequency, account.Balance);
                        return Ok(NewAccountDetails);
                    }

                    // account.UserName = ExecuterName;
                    else
                    {
                        Console.WriteLine("ExecuterName is null");
                        // Handle the case when ExecuterName is null
                        return BadRequest("ExecuterName is null");
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
        }

        [HttpPost("transfer")]
        public IActionResult TransferMoney(AccountTransactions transactions)
        {
            // Get and inspect Bearer Token
            var TokenHeader = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            //Call a function to validate token
            var isValidToken = _jwtService.ValidateToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));

            if (!isValidToken)
            {
                return Unauthorized();
            }
            else
            {
                // Decode and store token to string
                var tokenHandler = new JwtSecurityTokenHandler();
                var jsonToken = tokenHandler.ReadToken(TokenHeader) as JwtSecurityToken;
                string tokenString = jsonToken?.ToString();


                string ExtractClaim(string claimType)
                {
                    string claimPrefix = $"\"{claimType}\":\"";
                    int startIndex = tokenString.IndexOf(claimPrefix) + claimPrefix.Length;
                    int endIndex = tokenString.IndexOf("\",\"", startIndex);

                    return tokenString.Substring(startIndex, endIndex - startIndex);
                }

                // Extract the role and name claims from decoded token using string function
                string ExecuterRole = ExtractClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
                string ExecuterName = ExtractClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
                if (ExecuterRole == "User")
                {

                    // account.UserName = ExecuterName;
                    var NewTransaction = _userRepo.TransferMoney(ExecuterName,transactions.SourceAccountId, transactions.DestinationAccountId, transactions.Amount, transactions.Symbol, transactions.Bank);
                    return Ok(NewTransaction);
                }
                else
                {
                    return Unauthorized();
                }
            }
        }
    }

}
