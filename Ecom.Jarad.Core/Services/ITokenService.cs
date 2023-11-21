using Ecom.Jarad.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Jarad.Core.Services
{
    public interface ITokenService
    {
        string GetAndCreateToken(AppUsers token);
        ClaimsPrincipal GetPrincipalFromRefreshToken(string accessToken);
        string CreateRefreshToken();

    }
}
