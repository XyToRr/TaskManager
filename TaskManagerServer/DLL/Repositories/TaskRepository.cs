using DLL.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Repositories
{
    public class TaskRepository : IRepository<TaskModel>
    {
        private readonly TaskManagerContext _context;
        public TaskRepository(TaskManagerContext context) 
        {
            _context = context;
        }

        public async Task AddAsync(TaskModel entity)
        {
            await Task.Run(() => _context.TaskModels.Add(entity));
        }

        public async Task DeleteAsync(int id)
        {
            var task = await GetByCondition(u => u.Id == id).FirstAsync();
            if (task != null)
                _context.TaskModels.Remove(task);
        }

        public IQueryable<TaskModel> GetAll() => _context.TaskModels;

        public IQueryable<TaskModel> GetByCondition(Expression<Func<TaskModel, bool>> condition) => _context.TaskModels.Where(condition);

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(TaskModel newEntity, int id)
        {
            throw new NotImplementedException();
        }
    }
}
