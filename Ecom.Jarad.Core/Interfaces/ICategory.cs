using Ecom.Jarad.Core.DTOS;
using Ecom.Jarad.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Jarad.Core.Interfaces
{
    public interface ICategory : IGenericRepositry<Category>
    {
        Task<IReadOnlyList<Category>> GetAllAsync(string type);
        Task<bool> AddAsync(CategoryDTO categoryDTO);
        Task<bool> UpdateAsync(int id, CategoryUpdateDTO categoryDTO);


        Task<bool> Delete(int id);
    }
}
