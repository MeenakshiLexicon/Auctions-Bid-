using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using WebApplication_Inlamning_P_3.Repository.Interfaces;
using WebApplication_Inlamning_P_3.Repository.Repo;

namespace WebApplication_Inlamning_P_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
       private readonly IUserRepo _userRepo;
            private readonly IJWTService _jwtService;
            private readonly ILoanRepo _loanRepo;
            //private readonly ICustomerRepo _customerRepo;

    public LoginController(IUserRepo repo, IJWTService jwtService, ILoanRepo loanRepo)
    {
                _userRepo = repo;
                _jwtService = jwtService;
                _loanRepo = loanRepo;
    }

       [HttpPost("token")]
       public IActionResult Login([FromHeader(Name = "Authorization")] string authorizationHeader)
       {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Basic "))
                    return BadRequest("Missing or invalid Authorization header");

                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialsBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialsBytes).Split(':', 2);
                var username = credentials[0];
                var password = credentials[1];

                // Call this method to check if username and password exist
                var returnedUser = _userRepo.GetUserByUsernameAndPassword(username, password);

                // Check if the user exists in the database
                if (returnedUser == null)
                    return Unauthorized("Invalid Credentials");

                // If the user is valid, create claims for the authenticated user
                var claims = new[]
                {
                new Claim(ClaimTypes.Role, returnedUser.Result.UserType),
                new Claim(ClaimTypes.Name, returnedUser.Result.UserName)
                };

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysecretkey1234678NewSecretKey!#"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                // Create options to set up a token
                var tokenOptions = new JwtSecurityToken(
                    issuer: "http://localhost:5006/",
                    audience: "http://localhost:5006/",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: signinCredentials);

                // Generate a new token to be sent back
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                return Ok(new { Token = tokenString });
            }
            catch (Exception ex)
            {
                // Handle the exception as per your application's requirements
                Console.WriteLine($"An error occurred in Login: {ex.Message}");
                return StatusCode(500, "An error occurred during login");
            }
       }

    }

}
