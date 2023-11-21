using Ecom.Jarad.Core.Entities;
using Ecom.Jarad.Core.Services;
using Ecom.Jarad.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Jarad.Infrastructure.Repositries
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;
        public TokenService(IConfiguration configuration, ApplicationDbContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }
        public string GetAndCreateToken(AppUsers user)
        {
            ClaimsIdentity identity = new ClaimsIdentity(new Claim[]
        {
             new Claim(ClaimTypes.Email,user.Email),
             new Claim(ClaimTypes.Name,user.UserName),
             
         });
            string? secret = "ve...@!.#ryv.][erysecret...@!.#2.][pws@]";
            byte[]? key = Encoding.ASCII.GetBytes(secret);
            var date = DateTime.Now;
            SigningCredentials credentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = identity,
                Expires = DateTime.Now.AddSeconds(10),
                Issuer = configuration["Token:Issuer"],
                SigningCredentials = credentials,
                NotBefore = date,
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string CreateRefreshToken()
        {
            byte[] tokenByte = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenByte);
            var tokenInUser = context.AppUsers
                .Any(a => a.refreshToken == refreshToken);
            if (tokenInUser)
            {
                return CreateRefreshToken();
            }
            return refreshToken;
        }
        public ClaimsPrincipal GetPrincipalFromRefreshToken(string Token)
        {
            TokenValidationParameters tokenValidtionParameter = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ve...@!.#ryv.][erysecret...@!.#2.][pws@]")),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
            };
            var TokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = TokenHandler.ValidateToken(Token, tokenValidtionParameter, out securityToken);
            var JwtsecurityToken = securityToken as JwtSecurityToken;
            if (JwtsecurityToken is null || !JwtsecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("this is In Vaild");
            }
            return principal;
        }

    }
}
