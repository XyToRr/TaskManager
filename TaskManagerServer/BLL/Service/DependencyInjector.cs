using DLL.Context;
using DLL.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class DependencyInjector
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<TaskManagerContext>(options =>
            {
                options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TaskManagerDB;Integrated Security=True;");
            });

            services.AddTransient<IRepository<Project>, ProjectRepository>();
            services.AddTransient<ProjectService>();
            services.AddTransient<IRepository<TaskComment>, TaskCommentRepository>();
            services.AddTransient<TaskCommentService>();
            services.AddTransient<IRepository<TaskModel>, TaskRepository>();
            services.AddTransient<TaskService>();
            services.AddTransient<IRepository<User>,UserRepository>();
            services.AddTransient<UserService>();

            services.AddTransient<IRepository<UserToProject>, UserToProjectRepository>();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
