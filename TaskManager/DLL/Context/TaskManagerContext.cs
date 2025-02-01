using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Context
{
    class TaskManagerContext : DbContext
    {
        public DbSet<Project> Repositories { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<TaskModel> TaskModels { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserToProject> UsersToRepositories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetUpTaskModelLinks(modelBuilder);
            SetUpUserRepositoryManyToMany(modelBuilder);
            SetUpChecksToModels(modelBuilder);
        }

        private void SetUpChecksToModels(ModelBuilder modelBuilder)
        {




            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(256);
                entity.Property(p => p.Description).IsRequired().HasMaxLength(1024);
                entity.Property(p => p.CreatedAt).IsRequired();
            });


            modelBuilder.Entity<TaskComment>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Content).IsRequired().HasMaxLength(1024);

            });


            modelBuilder.Entity<TaskModel>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Name).IsRequired().HasMaxLength(256);
                entity.Property(t => t.Description).IsRequired().HasMaxLength(1024);
                entity.Property(t => t.CreatedAt).IsRequired();
                entity.Property(t => t.DeadLine).IsRequired();

                entity.Property(t => t.Priority).IsRequired().HasConversion<string>();
                entity.Property(t => t.Status).IsRequired().HasConversion<string>();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Login).IsRequired().HasMaxLength(256);
                entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(512);
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(256);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(256);
                entity.Property(u => u.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<UserToRepository>(entity =>
            {

                entity.Property(ur => ur.Role).IsRequired().HasConversion<string>();
            });



        }

        private void SetUpTaskModelLinks(ModelBuilder modelBuilder)
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

        private void SetUpUserRepositoryManyToMany(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserToProject>().HasKey(ur => new { ur.RepositoryId, ur.UserId });

            modelBuilder.Entity<UserToProject>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.Repositories)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserToProject>()
                .HasOne(ur => ur.Repository)
                .WithMany(r => r.Users)
                .HasForeignKey(ur => ur.UserId);
        }
    }
}
