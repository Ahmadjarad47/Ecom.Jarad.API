using Ecom.Jarad.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Jarad.Core.DTOS
{
    public class ProductsDTO
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public string TypeView { get; set; }
        [Required]
        public IFormFile Image { get; set; }
        [Required]
        public string oldPrice { get; set; }
        [Required]
        public string Newprice { get; set; }
        [Required]

        public int CategoryId { get; set; }
        [Required]

        public int SubCategoryId { get; set; }



    }
}
