using Ecom.Jarad.Core.DTOS;
using Ecom.Jarad.Core.Entities;
using Ecom.Jarad.Core.Interfaces;
using Ecom.Jarad.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Jarad.Infrastructure.Repositries
{
    public class CategoryRepositry : GenericRepositry<Category>, ICategory
    {
        private readonly ApplicationDbContext context;
        private readonly IFileProvider provider;

        public CategoryRepositry(ApplicationDbContext context, IFileProvider provider) : base(context)
        {
            this.context = context;
            this.provider = provider;
        }

        public async Task<bool> AddAsync(CategoryDTO categoryDTO)
        {
            string defultName = DateTime.Now.ToFileTime().ToString() + categoryDTO.Image.FileName;
            string ImageName = defultName.Replace(' ', '_');
            if (!Directory.Exists("wwwroot" + "/images/category"))
            {
                Directory.CreateDirectory("wwwroot" + "/images/category");
            }
            string src = $"/images/category/{ImageName}";

            IFileInfo info = provider.GetFileInfo(src);

            string root = info.PhysicalPath;

            using (FileStream stream = new(root, FileMode.Create))
            {
                await categoryDTO.Image.CopyToAsync(stream);
            }
            Category category = new Category()
            {
                Name = categoryDTO.Name,
                Image = src,
                type = categoryDTO.type
            };
            await context.category.AddAsync(category);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IReadOnlyList<Category>> GetAllAsync(string type)
        {
            List<Category> categoryList = await context.category.ToListAsync();
            if (string.IsNullOrEmpty(type))
            {
                return categoryList;
            }
            else
            {
                return categoryList.Where(m => m.type == type).ToList();
            }
        }

        public async Task<bool> UpdateAsync(int id, CategoryUpdateDTO categoryDTO)
        {
            Category category = await context.category.FindAsync(id);

            string src = category.Image;

            if (categoryDTO.Image is not null)
            {
                string defultName = DateTime.Now.ToFileTime().ToString() + categoryDTO.Image.FileName;
                string ImageName = defultName.Replace(' ', '_');
                if (!Directory.Exists("wwwroot" + "/images/category"))
                {
                    Directory.CreateDirectory("wwwroot" + "/images/category");
                }
                src = $"/images/category/{ImageName}";

                IFileInfo info = provider.GetFileInfo(src);

                string root = info.PhysicalPath;

                using (FileStream stream = new(root, FileMode.Create))
                {
                    await categoryDTO.Image.CopyToAsync(stream);
                }
                IFileInfo pichinfo = provider.GetFileInfo(category.Image);

                string rootpath = pichinfo.PhysicalPath;

                System.IO.File.Delete(rootpath);

                category = new Category()
                {
                    Image = src,
                    Name = categoryDTO.Name,
                    type = categoryDTO.type,
                    Id = id
                };
                context.category.Update(category);
                await context.SaveChangesAsync();
                return true;
            }

            else
            {
                category = new Category()
                {
                    Image = src,
                    Name = categoryDTO.Name,
                    type = categoryDTO.type,
                    Id = id
                };
                context.category.Update(category);
                await context.SaveChangesAsync();
                return true;
            }

        }
        public async Task<bool> Delete(int id)
        {
            Category category = await context.category.FindAsync(id);
            IFileInfo pichinfo = provider.GetFileInfo(category.Image);

            string rootpath = pichinfo.PhysicalPath;

            System.IO.File.Delete(rootpath);
            context.category.Remove(category);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
