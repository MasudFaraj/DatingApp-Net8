using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.EntitiesModels;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        /*private readonly SymmetricSecurityKey key;
        public TokenService(IConfiguration config){
            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }*/
        public string CreateToken(AppUser user)
        {
            var tokenKey = config["TokenKey"] ?? throw new Exception("cann't access tokenKey from appsettings.Development.json");
            //if (tokenKey.Length < 64) throw new Exception("your tokenkey should be longer");
            var key =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var claims = new List<Claim>{
                new(ClaimTypes.NameIdentifier, user.Name),
                //new Claim(JwtRegisteredClaimNames.NameId, user.Name)
            };
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
