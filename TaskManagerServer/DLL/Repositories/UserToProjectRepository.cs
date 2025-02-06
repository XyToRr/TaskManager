using DLL.Context;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Repositories
{
    public class UserToProjectRepository : IRepository<UserToProject>
    {
        private readonly TaskManagerContext _context;

        public UserToProjectRepository(TaskManagerContext context)
        {
            _context = context;
        }
        public async Task AddAsync(UserToProject entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserToProject> GetAll() => _context.UsersToRepositories;

        public IQueryable<UserToProject> GetByCondition(Expression<Func<UserToProject, bool>> condition)
        {
             return _context.UsersToRepositories.Where(condition);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(UserToProject newEntity, int id)
        {
            throw new NotImplementedException();
        }
    }
}
