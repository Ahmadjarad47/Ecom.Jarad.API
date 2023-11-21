using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Jarad.Core.DTOS
{
    public record registerDto
   ([Required]string UserName,[EmailAddress]string Email ,[Required]string Password);
}
