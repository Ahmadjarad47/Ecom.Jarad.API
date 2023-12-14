using Ecom.Jarad.Core.DTOS;
using Ecom.Jarad.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Jarad.Core.Interfaces
{
    public interface ISubCategory : IGenericRepositry<SubCategory>
    {
        Task<IReadOnlyList<SubCategory>> GetAllAsync();
        Task<string> AddAsync(SubCategoryDTO subCategoryDTO);
        Task<bool> Remove(int? id);
    }
}
