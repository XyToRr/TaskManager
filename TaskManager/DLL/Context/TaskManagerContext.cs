using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    class TaskManagerContext : DbContext
    {
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<TaskModel> TaskModels { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserToRepository> UsersToRepositories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetUpTaskModelLinks(modelBuilder);
            SetUpUserRepositoryManyToMany(modelBuilder);
        }

        private static void SetUpTaskModelLinks(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskModel>()
                .HasOne(t => t.Repository)
                .WithMany(r => r.Tasks)
                .HasForeignKey(t => t.RepositoryId);

            modelBuilder.Entity<TaskModel>()
                .HasOne(t => t.AssignedUser)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssignedUserId);

            modelBuilder.Entity<TaskModel>()
                .HasOne(t => t.CreatedUser)
                .WithMany(u => u.CreatedTasks)
                .HasForeignKey(t => t.CreatedUserId);


            modelBuilder.Entity<TaskComment>()
                .HasOne(c => c.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskId);
        }

        private static void SetUpUserRepositoryManyToMany(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserToRepository>().HasKey(ur => new { ur.RepositoryId, ur.UserId });

            modelBuilder.Entity<UserToRepository>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.Repositories)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserToRepository>()
                .HasOne(ur => ur.Repository)
                .WithMany(r => r.Users)
                .HasForeignKey(ur => ur.UserId);
        }
    }
}
