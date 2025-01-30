using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DeadLine { get; set; }
        public Status Status { get; set; }
        public int CreatedUserId { get; set; }
        public User CreatedUser { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
    public enum Status
    {
        Completed,
        InProgress,
        Rejected
    }
}
