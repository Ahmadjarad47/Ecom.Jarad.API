using Ecom.Jarad.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Jarad.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUsers>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public virtual DbSet<Address> Addresses { get; set; }

        public virtual DbSet<AppUsers> AppUsers { get; set; }


        public virtual DbSet<Carousel> Carousels { get; set; }


        public virtual DbSet<Category> category { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
