using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual List<UserToProject> Repositories { get; set; } = new();
        public virtual List<TaskModel> CreatedTasks { get; set; } = new();
        public virtual List<TaskModel> AssignedTasks { get; set; } = new();
    }
}
