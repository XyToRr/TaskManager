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
    public class TaskCommentRepository : IRepository<TaskComment>
    {
        private readonly TaskManagerContext _context;

        public TaskCommentRepository(TaskManagerContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TaskComment entity)
        {
            await Task.Run(() => _context.TaskComments.Add(entity));
        }

        public async Task DeleteAsync(int id)
        {
            var comment = await GetByCondition(u => u.Id == id).FirstAsync();
            if (comment != null)
                _context.TaskComments.Remove(comment);
        }

        public IQueryable<TaskComment> GetAll() => _context.TaskComments;
        public IQueryable<TaskComment> GetByCondition(Expression<Func<TaskComment, bool>> condition) => _context.TaskComments.Where(condition);

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(TaskComment newEntity, int id)
        {
            throw new NotImplementedException();
        }
    }
}
