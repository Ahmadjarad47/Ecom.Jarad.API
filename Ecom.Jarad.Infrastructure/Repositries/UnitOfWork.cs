using AutoMapper;
using Ecom.Jarad.Core.Entities;
using Ecom.Jarad.Core.Interfaces;
using Ecom.Jarad.Core.Services;
using Ecom.Jarad.Infrastructure.Data;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;

namespace Ecom.Jarad.Infrastructure.Repositries
{
    internal class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext context;

        private readonly IFileProvider fileProvider;

        private readonly IMapper mapper;
        private readonly SaveImage saveImage;

        public ICarousel Carousel { get; }

        public ICategory Category { get; }

        public ISubCategory SubCategory { get; }

        public IProducts Products { get; }

        public UnitOfWork(ApplicationDbContext context, IFileProvider fileProvider, IMapper mapper, SaveImage saveImage)
        {
            this.saveImage = saveImage;
            Carousel = new CarouselRepositry(context, fileProvider, mapper, saveImage);
            this.context = context;
            this.fileProvider = fileProvider;
            this.mapper = mapper;
            SubCategory = new SubCategoryRepositry(context);
            Category = new CategoryRepositry(context, fileProvider, saveImage);
            Products = new ProductsRepositry(context, fileProvider, saveImage);

        }
    }
}
