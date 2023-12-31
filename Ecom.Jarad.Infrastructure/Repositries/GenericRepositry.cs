using Ecom.Jarad.Core.Entities;
using Ecom.Jarad.Core.Interfaces;
using Ecom.Jarad.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Jarad.Infrastructure.Repositries
{
    public class GenericRepositry<T> : IGenericRepositry<T> where T : BaseEntity<int>
    {
        private readonly ApplicationDbContext context;
        private readonly IFileProvider fileProvider;

        public GenericRepositry(ApplicationDbContext context)
        {
            this.context = context;
        }

        public GenericRepositry(ApplicationDbContext context, IFileProvider fileProvider)
        {
            this.context = context;
            this.fileProvider = fileProvider;
        }

        public async Task AddAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        }




        public async Task<int> CountAsync()
        => await context.Set<T>().CountAsync();

        public async Task DeleteAsync(int id)
        {
            var entity = await context.Set<T>().FindAsync(id);
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll()
        => context.Set<T>().AsNoTracking().ToList();



        public async Task<IReadOnlyList<T>> GetAllAsync()
        => await context.Set<T>().AsNoTracking().ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            var query = context.Set<T>().AsQueryable();
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(int id)
       => await context.Set<T>().FindAsync(id);

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> values = context.Set<T>().Where(x => x.Id == id);
            foreach (var item in includes)
            {
                values = values.Include(item);
            }
            return await values.FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(int id, T entity)
        {
            var entities = await context.Set<T>().FindAsync(id);
            context.Update(entities);
            await context.SaveChangesAsync();
        }
    }
}
