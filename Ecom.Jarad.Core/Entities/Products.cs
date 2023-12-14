using System;
using System.Collections.Generic;
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

    }
}
