using Ecom.Jarad.Core.DTOS;
using Ecom.Jarad.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Jarad.Core.Interfaces
{
    public interface IProducts : IGenericRepositry<Products>
    {
        Task<IReadOnlyList<Products>> GetarrivalsAsync();
        Task<IReadOnlyList<Products>> GettrendingAsync();

        Task<IReadOnlyList<Products>> GettTopRatedAsync();
        Task<bool> AddAsync(ProductsDTO productsDTO);
        Task<bool> UpdateAsync(int? id, ProductsDTO productsDTO);
        Task DeleteAsync(int id);
    }
}
