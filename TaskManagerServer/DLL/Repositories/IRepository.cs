using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> condition);
        public Task AddAsync(T entity);
        public Task UpdateAsync(T newEntity, int id);
        public Task DeleteAsync(int id);
        public Task SaveAsync();
    }
}
