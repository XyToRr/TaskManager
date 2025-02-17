﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeadLine { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public int RepositoryId { get; set; }
        public Project Repository { get; set; }
        public int CreatedUserId { get; set; }
        public User CreatedUser { get; set; }
        public int? AssignedUserId { get; set; }
        public User AssignedUser { get; set; }
        public List<TaskComment> Comments { get; set; } = new();
    }

    public enum Status
    {
        Completed,
        InProgress,
        Rejected
    }

    public enum Priority
    {
        Low,
        Medium,
        High,
        Critical
    }
}
