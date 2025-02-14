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
    public class ProjectService : IService<Project>
    {
        public IRepository<Project> projectRepository { get; set; }
        public IRepository<UserToProject> linkRepository { get; set; }  

        public ProjectService(IRepository<Project> projectRepository, IRepository<UserToProject> userRepository)
        {
            this.projectRepository = projectRepository;
            this.linkRepository = userRepository;
        }

        public ProjectService(IRepository<Project> projectRepository) { this.projectRepository = projectRepository; }

        public async Task AddAsync(Project newEntity) => await projectRepository.AddAsync(newEntity);

        public async Task DeleteAsync(Project Entity) => await projectRepository.DeleteAsync(Entity.Id);

        public IQueryable<Project> GetAll() => projectRepository.GetAll();

        public IQueryable<Project> GetByCondition(Expression<Func<Project, bool>> expression) => projectRepository.GetByCondition(expression);

        public Task UpdateAsync(Project newEntity, int oldId) => projectRepository.UpdateAsync(newEntity, oldId);

        public async Task AddUser(int userId, Role role, int projectId)
        {
            var project = GetByCondition(p => p.Id == projectId);
            if(project != null)
            {
                await linkRepository.AddAsync(new UserToProject()
                {
                    RepositoryId = projectId,
                    UserId = userId,
                    Role = role,
                });
            }
           
        }

        public async Task SaveAsync() => await projectRepository.SaveAsync();
    }
}
