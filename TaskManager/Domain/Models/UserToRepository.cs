﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class UserToRepository
    {
        public int RepositoryId { get; set; }
        public Project Repository { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }

    public enum Role
    {
        Owner,
        Admin,
        Worker
    }
}
