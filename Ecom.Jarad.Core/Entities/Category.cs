using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Jarad.Core.Entities
{
    public class Category : BaseEntity<int>
    {
        public string Name { get; set; }
        public string type { get; set; }

        public string Image { get; set; }
    }
}
