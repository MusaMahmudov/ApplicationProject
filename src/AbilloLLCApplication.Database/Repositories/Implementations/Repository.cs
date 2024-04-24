using AbilloLLCApplication.Core.Entities.Common;
using AbilloLLCApplication.Database.Contexts;
using AbilloLLCApplication.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Database.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : BaseSectionEntity
    {
        private readonly AppDbContext _context;
        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll(params string[]? includes)
        {
           var queries = _context.Set<T>().AsNoTracking().OrderByDescending(q=>q.CreatedAt).AsQueryable();
            
            if(includes?.Length > 0 && includes is not null)
            {
                foreach(var include in includes)
                {
                   queries = queries.Include(include);
                }
                
            }
            return queries;
        }
        public IQueryable<T> GetFiltered(Expression<Func<T, bool>> expression, bool ignoreFilters ,params string[]? includes)
        {
            var queries = _context.Set<T>().Where(expression).AsNoTracking().OrderByDescending(q => q.CreatedAt).AsQueryable();
            if (ignoreFilters)
            {
                queries = queries.IgnoreQueryFilters();
            }
            if (includes?.Length > 0 && includes is not null)
            {
                foreach (var include in includes)
                {
                    queries = queries.Include(include);
                }

            }
            return queries;
        }
        public async Task<bool> IsExistsAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().AnyAsync(expression);
            
        }
        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> expression, params string[]? includes)
        {
            
            var query =  _context.Set<T>().AsQueryable();
            if(includes?.Length > 0 && includes is not null)
            {
                foreach(var include in includes)
                {
                    query = query.Include(include);
                }
                
            }
            return await query.FirstOrDefaultAsync(expression);
        }

        public async Task CreateAsync(T entity)
        {
           await _context.Set<T>().AddAsync(entity);  
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);

        }
        public void DeleteList(IQueryable<T> entities)
        {
            _context.RemoveRange(entities);
        }

        public async Task<int> SaveChangesAsync()
        {
         return  await _context.SaveChangesAsync();
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void UpdateList(List<T> entities)
        {
           _context.Set<T>().UpdateRange(entities);
        }

        
    }
}
