using DLL.Context;
using DLL.Repositories;
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

            services.AddTransient<ProjectRepository>();
            services.AddTransient<ProjectService>();
            services.AddTransient<TaskCommentRepository>();
            services.AddTransient<TaskCommentService>();
            services.AddTransient<TaskRepository>();
            services.AddTransient<TaskService>();
            services.AddTransient<UserRepository>();
            services.AddTransient<UserService>();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
