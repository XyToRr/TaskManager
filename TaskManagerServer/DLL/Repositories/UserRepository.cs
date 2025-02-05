using DLL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly TaskManagerContext _context;

        public UserRepository(TaskManagerContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User entity)
        {
           await Task.Run(() => _context.Users.Add(entity));
            await SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
           var user = await GetByCondition(u => u.Id == id).FirstAsync();
            if (user != null)
            {
                _context.Users.Remove(user);
                await SaveAsync();
            }
               
        }

        public IQueryable<User> GetAll() => _context.Users;

        public IQueryable<User> GetByCondition(Expression<Func<User, bool>> condition) => _context.Users.Where(condition);

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(User newEntity, int id)
        {
           throw new NotImplementedException();
        }
    }
}
