namespace WebApplication_Inlamning_P_3.Models.Entities
{
    public class JwtTokenEntity
    {
        public string Token { get; set; }


        public JwtTokenEntity(string token)
        {
            Token = token;
        }
    }
}
