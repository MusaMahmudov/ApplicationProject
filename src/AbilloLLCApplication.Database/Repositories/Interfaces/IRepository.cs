using AbilloLLCApplication.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AbilloLLCApplication.Database.Repositories.Interfaces
{
    public interface IRepository<T> where T : BaseSectionEntity
    {
        IQueryable<T> GetAll(params string[]? includes);
        IQueryable<T> GetFiltered(Expression<Func<T,bool>> expression,bool ignoreFilters, params string[]? includes);
        Task<T> GetSingleAsync(Expression<Func<T,bool>> expression, params string[]? includes);
        Task CreateAsync(T entity);
        void Update(T entity);
        void UpdateList(List<T> entities);
        void Delete(T entity);
        void DeleteList(IQueryable<T> entities);
        Task<int> SaveChangesAsync();
        Task<bool> IsExistsAsync(Expression<Func<T,bool>> expression);


    }
}
