using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Jarad.Core.Interfaces
{
    public interface IUnitOfWork
    {
        public ICarousel Carousel { get; }
        public ICategory Category { get; }
        public ISubCategory SubCategory { get; }
        public IProducts Products { get; }

    }
}
