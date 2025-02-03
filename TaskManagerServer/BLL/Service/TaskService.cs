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
    public class TaskService : IService<TaskModel>
    {
        public IRepository<TaskModel> taskRepository;

        public TaskService(IRepository<TaskModel> taskRepository) { this.taskRepository = taskRepository; }
        public IQueryable<TaskModel> GetAll() => taskRepository.GetAll();
        public IQueryable<TaskModel> GetByCondition(Expression<Func<TaskModel, bool>> predicate) => taskRepository.GetByCondition(predicate);
        public async Task AddAsync(TaskModel newEntity) => await taskRepository.AddAsync(newEntity);
        public async Task DeleteAsync(TaskModel taskModel) => await taskRepository.DeleteAsync(taskModel.Id);
        public async Task UpdateAsync(TaskModel newEntity, int oldId) => await taskRepository.UpdateAsync(newEntity, oldId);
    }
}
