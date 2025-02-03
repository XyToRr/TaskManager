using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public enum MessageType
    {
        Accept,
        Decline,
        RegisterRequest,
        LoginRequest,
        RoleChange,
        ProjectCreationRequest,
        TaskCreationRequest,
        FindUser,
        TaskAssignment,
        TaskCompletion,
        TaskStateUpdate,
        TaskUpdate,
        AddUserToProject
    }
}
