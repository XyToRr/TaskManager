using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual List<TaskModel> Tasks { get; set; } = new();
        public virtual List<UserToProject> Users { get; set; } = new();
    }
}
