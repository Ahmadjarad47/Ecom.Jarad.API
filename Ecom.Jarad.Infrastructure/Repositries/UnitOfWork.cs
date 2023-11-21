using AutoMapper;
using Ecom.Jarad.Core.Entities;
using Ecom.Jarad.Core.Interfaces;
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


        public ICarousel Carousel { get; }


        public UnitOfWork(ApplicationDbContext context, IFileProvider fileProvider, IMapper mapper)
        {
            Carousel = new CarouselRepositry(context, fileProvider, mapper);
            this.context = context;
            this.fileProvider = fileProvider;
            this.mapper = mapper;
        }
    }
}
