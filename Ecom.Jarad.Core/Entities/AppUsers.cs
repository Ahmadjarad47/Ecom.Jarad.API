using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Jarad.Core.Entities
{
    public class AppUsers:IdentityUser
    {
        public DateTime TimeRegister { get; set; }=DateTime.Now;
        public DateTime TimeRefreshToken { get; set; }
        public string refreshToken { get; set; }

        public string ResetPasswordToken { get; set; }
        public DateTime ResetPasswordTokenExpier { get; set; }

        public virtual Address Address { get; set; }

    }
}
