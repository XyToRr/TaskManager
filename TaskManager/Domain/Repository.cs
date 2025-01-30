using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Repository
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual List<TaskModel> Tasks { get; set; } = new();
        public virtual List<UserToRepository> Users { get; set; } = new();
    }
}
