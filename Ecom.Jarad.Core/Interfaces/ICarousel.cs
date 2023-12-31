using Ecom.Jarad.Core.DTOS;
using Ecom.Jarad.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Jarad.Core.Interfaces
{
    public interface ICarousel : IGenericRepositry<Carousel>
    {
        Task<bool> AddAsync(CarouselDTO item);
        Task<bool> UpdateAsync(int id, CarouselDTO item);
        Task<bool> DeleteAsync(int id);
    }
}
