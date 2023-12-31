﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Jarad.Core.Entities
{
    public class Products : BaseEntity<int>
    {
        public string name { get; set; }

        public string description { get; set; }

        public string TypeView { get; set; }

        public string Image { get; set; }

        public string oldPrice { get; set; }

        public string Newprice { get; set; }


        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        public int SubCategoryId { get; set; }


        [ForeignKey(nameof(SubCategoryId))]
        public virtual SubCategory SubCategory { get; set; }


    }
}
