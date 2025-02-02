using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DLL.Repositories;
using Domain.Models;

namespace BLL.Service
{
    public class TaskCommentService : IService<TaskComment>
    {
        public IRepository<TaskComment> taskCommentRepository;

        public TaskCommentService(IRepository<TaskComment> taskCommentRepository) { this.taskCommentRepository = taskCommentRepository; }
        public IQueryable<TaskComment> GetAll() => taskCommentRepository.GetAll();
        public IQueryable<TaskComment> GetByCondition(Expression<Func<TaskComment, bool>> predicate) => taskCommentRepository.GetByCondition(predicate);
        public async Task AddAsync(TaskComment newEntity) => await taskCommentRepository.AddAsync(newEntity);
        public async Task DeleteAsync(TaskComment comment) => await taskCommentRepository.DeleteAsync(comment.Id);
        public async Task UpdateAsync(TaskComment newEntity, int oldId) => await taskCommentRepository.UpdateAsync(newEntity, oldId);
    }
}
