﻿using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Context
{
    public class TaskManagerContext : DbContext
    {
        public DbSet<Project> Repositories { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<TaskModel> TaskModels { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserToProject> UsersToRepositories { get; set; }

        public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetUpTaskModelLinks(modelBuilder);
            SetUpUserRepositoryManyToMany(modelBuilder);
            SetUpChecksToModels(modelBuilder);
        }

        private void SetUpChecksToModels(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project> ()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(256);

            modelBuilder.Entity<Project>()
                .Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(1024);           
        }

        private void SetUpTaskModelLinks(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskModel>()
                .HasOne(t => t.Repository)
                .WithMany(r => r.Tasks)
                .HasForeignKey(t => t.RepositoryId);

            modelBuilder.Entity<TaskModel>()
                .HasOne(t => t.CreatedUser)
                .WithMany(u => u.CreatedTasks)
               .HasForeignKey(t => t.CreatedUserId)
               .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<TaskModel>()
                .HasOne(t => t.AssignedUser)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssignedUserId)
                .OnDelete(DeleteBehavior.SetNull);

            
            modelBuilder.Entity<TaskComment>()
                .HasOne(tc => tc.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(tc => tc.TaskId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<TaskComment>()
                .HasOne(tc => tc.User)
                .WithMany(u => u.WrittenComments)
                .HasForeignKey(tc => tc.UserId)
                .OnDelete(DeleteBehavior.Restrict);
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
                .HasForeignKey(ur => ur.RepositoryId);
        }
    }
}
