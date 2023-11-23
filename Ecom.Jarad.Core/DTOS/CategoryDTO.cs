using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Jarad.Core.DTOS
{
    public class CategoryDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string type { get; set; }

        public IFormFile Image { get; set; }
    }
    public class CategoryUpdateDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string type { get; set; }

        public IFormFile Image { get; set; }
    }
}
