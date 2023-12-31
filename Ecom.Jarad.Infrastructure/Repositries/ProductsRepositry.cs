using Ecom.Jarad.Core.DTOS;
using Ecom.Jarad.Core.Entities;
using Ecom.Jarad.Core.Interfaces;
using Ecom.Jarad.Core.Services;
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
    public class ProductsRepositry : GenericRepositry<Products>, IProducts
    {
        private readonly ApplicationDbContext context;
        private readonly IFileProvider fileProvider;
        private readonly SaveImage saveImage;
        public ProductsRepositry(ApplicationDbContext context, IFileProvider fileProvider, SaveImage saveImage) : base(context)
        {
            this.context = context;
            this.fileProvider = fileProvider;
            this.saveImage = saveImage;
        }

        public async Task<bool> AddAsync(ProductsDTO productsDTO)
        {
            if (productsDTO.Image is not null)
            {
                string src = await saveImage.AddImage(productsDTO.Image, "product") ?? "return-Null";
                if (src == "return-Null") { return false; }

                Category category = await context.category.FindAsync(productsDTO.CategoryId);
                if (category is null) { return false; }

                SubCategory subCategory = await context.SubCategory.FindAsync(productsDTO.SubCategoryId);
                if (subCategory is null) { return false; }


                Products products = new Products()
                {
                    Image = src,
                    name = productsDTO.name,
                    CategoryId = productsDTO.CategoryId,
                    description = productsDTO.description
                ,
                    Newprice = productsDTO.Newprice,
                    oldPrice = productsDTO.oldPrice
                ,
                    SubCategoryId = productsDTO.SubCategoryId,
                    TypeView = productsDTO.TypeView
                ,
                };

                await context.Products.AddAsync(products);
                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }




        public async Task<IReadOnlyList<Products>> GetarrivalsAsync()
        => await context.Products.Include(m => m.Category)
            .Include(m => m.SubCategory)
             .Where(m => m.TypeView.Equals("arrivals")).ToListAsync();



        public async Task<IReadOnlyList<Products>> GettrendingAsync()
      => await context.Products.Include(m => m.Category)
            .Include(m => m.SubCategory)
             .Where(m => m.TypeView.Equals("trending")).ToListAsync();



        public async Task<IReadOnlyList<Products>> GettTopRatedAsync()
       => await context.Products.Include(m => m.Category)
          .Include(m => m.SubCategory)
           .Where(m => m.TypeView.Equals("TopRated")).ToListAsync();

        public async Task<bool> UpdateAsync(int? id, ProductsDTO productsDTO)
        {
            Products products = await context.Products.FindAsync(id);
            string src = products.Image;
            if (products != null)
            {
                if (productsDTO.Image != null)
                {
                    src = await saveImage.AddImage(productsDTO.Image, "product") ?? "return-Null";
                    if (src == "return-Null") { return false; }

                    saveImage.DeleteImage(products.Image);
                    products = new Products()
                    {
                        Image = src,
                        CategoryId = productsDTO.CategoryId,
                        description = productsDTO.description,
                        name = productsDTO.name
                         ,
                        Newprice = productsDTO.Newprice
                         ,
                        oldPrice = productsDTO.oldPrice
                         ,
                        SubCategoryId = productsDTO.SubCategoryId
                         ,
                        TypeView = productsDTO.TypeView
                         ,
                        Id = products.Id
                    };


                    context.Products.Update(products);
                    await context.SaveChangesAsync();
                    return true;
                }
                products = new Products()
                {
                    Image = src,
                    CategoryId = productsDTO.CategoryId,
                    description = productsDTO.description,
                    name = productsDTO.name
                         ,
                    Newprice = productsDTO.Newprice
                         ,
                    oldPrice = productsDTO.oldPrice
                         ,
                    SubCategoryId = productsDTO.SubCategoryId
                         ,
                    TypeView = productsDTO.TypeView
                         ,
                    Id = products.Id
                };
                context.Products.Update(products);
                await context.SaveChangesAsync();
                return true;


            }
            return false;
        }

        public async Task DeleteAsync(int id)
        {
            Products products = await context.Products.FindAsync(id);
            saveImage.DeleteImage(products.Image);
            context.Products.Remove(products);
            await context.SaveChangesAsync();

        }
    }
}
