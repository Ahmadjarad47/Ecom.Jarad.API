using AutoMapper;
using Ecom.Jarad.Core.DTOS;
using Ecom.Jarad.Core.Entities;
using Ecom.Jarad.Core.Interfaces;
using Ecom.Jarad.Core.Services;
using Ecom.Jarad.Infrastructure.Data;
using Microsoft.Extensions.FileProviders;
using static System.Net.Mime.MediaTypeNames;

namespace Ecom.Jarad.Infrastructure.Repositries
{
    public class CarouselRepositry : GenericRepositry<Carousel>, ICarousel
    {
        private readonly ApplicationDbContext context;
        private readonly IFileProvider fileProvider;
        private readonly IMapper mapper;
        private readonly SaveImage saveImage;

        public CarouselRepositry(ApplicationDbContext context, IFileProvider fileProvider, IMapper mapper, SaveImage saveImage) : base(context)
        {
            this.context = context;
            this.fileProvider = fileProvider;
            this.mapper = mapper;
            this.saveImage = saveImage;
        }

        public async Task<bool> AddAsync(CarouselDTO item)
        {
            if (item.Image is not null)
            {
                string src = await saveImage.AddImage(item.Image, "carousel") ?? "return-Null";
                if (src == "return-Null") { return false; }
                Carousel result = new()
                {
                    Title = item.Title,
                    Description = item.Description,
                    LinkProduct = item.LinkProduct,
                    Image = src
                };

                await this.context.AddAsync(result);
                await context.SaveChangesAsync();
                return true;

            }
            return false;
        }

        public async Task<bool> DeleteImage(int id)
        {
            Carousel getCurrentCarousel = await context.Carousels.FindAsync(id);

            if (!string.IsNullOrEmpty(getCurrentCarousel.Image))
            {
                IFileInfo pichinfo = fileProvider.GetFileInfo(getCurrentCarousel.Image);

                string rootpath = pichinfo.PhysicalPath;

                System.IO.File.Delete(rootpath);
            }
            context.Carousels.Remove(getCurrentCarousel);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(int id, CarouselDTO item)
        {
            Carousel getCurrentCarousel = await context.Carousels.FindAsync(id);
            if (item is not null)
            {
                string src = "";
                if (item.Image is not null)
                {

                    src = await saveImage.AddImage(item.Image, "carousel") ?? "return-Null";
                    if (src == "return-Null") { return false; }
                }
                if (!string.IsNullOrEmpty(getCurrentCarousel.Image))
                {
                    saveImage.DeleteImage(getCurrentCarousel.Image);
                }
                getCurrentCarousel = new()
                {
                    Id = id,
                    Title = item.Title,
                    Description = item.Description,
                    Image = src,
                    LinkProduct = item.LinkProduct
                };
                context.Carousels.Update(getCurrentCarousel);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Carousel getCurrentCarousel = await context.Carousels.FindAsync(id);
            saveImage.DeleteImage(getCurrentCarousel.Image);
            this.context.Carousels.Remove(getCurrentCarousel);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
