using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Jarad.Core.DTOS
{
    public record updatePassword
   ([EmailAddress] string Email,[Required] string password, [Required] string ConfirmPassword);
}
