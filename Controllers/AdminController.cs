using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using WebApplication_Inlamning_P_3.Models.Entities;
using WebApplication_Inlamning_P_3.Repository.Interfaces;
using WebApplication_Inlamning_P_3.Repository.Repo;
using static WebApplication_Inlamning_P_3.Models.Entities.User;

namespace WebApplication_Inlamning_P_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AdminController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly IJWTService _jwtService;
        private readonly ILoanRepo _loanRepo;
        //private readonly ICustomerRepo _customerRepo;

        public AdminController(IUserRepo repo, IJWTService jwtService, ILoanRepo loanRepo)
        {
            _userRepo = repo;
            _jwtService = jwtService;
            _loanRepo = loanRepo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userRepo.GetAllUser();
            // Return HTTP 200 with users
            return Ok(users);
        }


        [HttpPost("adduser")]
        public IActionResult AddUser([FromBody] UserRegistration userRegistration, [FromServices] IUserRepo repo)
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
                    userRegistration.User.ExecuterRole = ExtractClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
                    userRegistration.User.ExecuterName = ExtractClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");

                    if (userRegistration.User.UserType == "User")
                    {
                        // Call Add user method with claims
                        var resultAddUser = _userRepo.AddUser(userRegistration.User, userRegistration.Customer, userRegistration.Account);
                        return Ok(resultAddUser);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in AddUser: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }



        [HttpPost("postloan")]
        public IActionResult PostLoan(Loan loanRequest)
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

                try
                {
                    if (ExecuterRole == "Admin")
                    {
                        _loanRepo.PostLoanForCustomer(loanRequest.CustomerId, loanRequest.Amount, loanRequest.Duration, loanRequest.Payments, loanRequest.AccountId);
                        return Ok("Loan posted successfully");
                    }
                    else
                    {
                        return Unauthorized();
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest($"Failed to post loan: {ex.Message}");
                }
            }

        }

    }

}
