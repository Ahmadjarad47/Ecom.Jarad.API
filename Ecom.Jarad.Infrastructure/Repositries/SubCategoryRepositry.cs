using Ecom.Jarad.Core.DTOS;
using Ecom.Jarad.Core.Entities;
using Ecom.Jarad.Core.Interfaces;
using Ecom.Jarad.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Jarad.Infrastructure.Repositries
{
    public class SubCategoryRepositry : GenericRepositry<SubCategory>, ISubCategory
    {
        private readonly ApplicationDbContext context;

        public SubCategoryRepositry(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<string> AddAsync(SubCategoryDTO subCategoryDTO)
        {
            Category getCategoryById = await context.category
                .FindAsync(subCategoryDTO.CategoryId);

            if (getCategoryById is null)
            {
                return "this categoryId is wrong";
            }

            SubCategory subcategory = await context.SubCategory.Where(m => m.Name == subCategoryDTO.Name).FirstOrDefaultAsync();

            if (subcategory is not null)
            { return "this subcategory is already Exist the item is not added"; }

            subcategory = new()
            {
                CategoryId = subCategoryDTO.CategoryId,
                Name = subCategoryDTO.Name,
                CountItems = subCategoryDTO.countItem
            };
            await context.SubCategory.AddAsync(subcategory);

            await context.SaveChangesAsync();

            return "subCategory Added - Done!";

        }




        public async Task<bool> Remove(int? id)
        {
            SubCategory sub = await context.SubCategory.FindAsync(id);

            if (sub is null)
            {
                return false;
            }
            context.SubCategory.Remove(sub);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IReadOnlyList<SubCategory>> GetAllAsync()
            => await context.SubCategory.Include(x => x.Categories).ToListAsync();

    }
}
