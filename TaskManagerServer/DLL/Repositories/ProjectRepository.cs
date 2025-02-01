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
    public class ProjectRepository : IRepository<Project>
    {
        private readonly TaskManagerContext _context;

        public ProjectRepository(TaskManagerContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Project entity)
        {
            await Task.Run(() => _context.Repositories.Add(entity));
        }

        public async Task DeleteAsync(int id)
        {
            var project = await GetByCondition(u => u.Id == id).FirstAsync();
            if (project != null)
                _context.Repositories.Remove(project);
        }

        public IQueryable<Project> GetAll() => _context.Repositories;

        public IQueryable<Project> GetByCondition(Expression<Func<Project, bool>> condition) => _context.Repositories.Where(condition);

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public Task UpdateAsync(Project newEntity, int id)
        {
            throw new NotImplementedException();
        }

    }
}
