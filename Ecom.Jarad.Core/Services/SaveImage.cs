using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Jarad.Core.Services
{
    public interface SaveImage
    {
        Task<string> AddImage(IFormFile formFile, string src);
        void DeleteImage(string formFile);
    }
}
