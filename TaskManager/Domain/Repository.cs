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
        public List<TaskModel> Tasks;
        public List<UserToRepository> Users { get; set; }
    }
}
