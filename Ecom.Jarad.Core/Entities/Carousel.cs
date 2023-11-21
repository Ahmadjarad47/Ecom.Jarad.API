using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Jarad.Core.Entities
{
    public class Carousel : BaseEntity<int>
    {
        public string Title { get; set; }


        public string Description { get; set; }


        public string LinkProduct { get; set; }

        public string Image { get; set; }

    }
}
