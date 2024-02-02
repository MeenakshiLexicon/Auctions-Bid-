using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication_Inlamning_P_3.Repository.Interfaces;

namespace WebApplication_Inlamning_P_3.Repository.Repo
{
    public class JWTServiceRepo : IJWTService
    {
        private readonly string _secretKey;

        public JWTServiceRepo(string secretKey)
        {
            _secretKey = secretKey;
        }

        
        public bool ValidateToken(string token)
        {
           var tokenHandler = new JwtSecurityTokenHandler();
             try
             {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,   // Validera i en request att vår token är uppsatt av denna app
                    ValidateAudience = true, // Validera att det är en verifierad användare
                    ValidateLifetime = true, // Validera att aktuell token fortfarande gäller
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "http://localhost:5006/", // Validera URL där token har satts upp
                    ValidAudience = "http://localhost:5006/", // Validera URL som api ligger på
                    IssuerSigningKey =
    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysecretkey1234678NewSecretKey!#")),
                    ClockSkew = TimeSpan.Zero
                }, out _);
             }
             catch
             {
                return false;
             }

                return true;
        }
    
}

}
