using Authentication.Models;
using Microsoft.AspNetCore.Authentication;
//using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.Services
{
    public class JWTTokenService
    {
        private readonly UserService _userService;
        public JWTTokenService(UserService userService)
        {
            _userService = userService;
        }

        public async Task<string?> GenerateAuthToken(LoginModel loginModel)
        {
            Users? user = await _userService.GetAsync(loginModel.UserName, loginModel.Password);

            if (user is null)
            {
                return null;
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("topsecretsecretJWTsigningKey@12356789"));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var expirationTimeStamp = DateTime.Now.AddMinutes(50);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Name, user.UserID),
            new Claim("Role", user.Role)
            //new Claim("scope", string.Join(" ", user.Scopes))
        };

            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:5002",
                claims: claims,
                expires: expirationTimeStamp,
                signingCredentials: signingCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return tokenString;
        }
    }
}
