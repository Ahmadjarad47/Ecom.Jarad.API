using Ecom.Jarad.Core.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Jarad.Core.Interfaces
{
    public interface IEmailSender
    {
        void sendEmail(EmailModelDTO email);
    }
}
