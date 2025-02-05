using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public enum MessageType
    {
        Connect,
        Disconnect,
        RegisterAccept,
        RegisterDecline,
        RegisterRequest,
        LoginAccept,
        LoginDecline,
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
