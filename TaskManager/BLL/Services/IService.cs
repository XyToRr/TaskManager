using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IService<T> where T : class
    {
        public IQueryable<T> GetAll();
        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
        public Task AddAsync(T newEntity);
        public Task DeleteAsync(T newEntity);
        public Task UpdateAsync(T newEntity, int oldId);
    }
}
