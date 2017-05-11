using Dhobi.Core.UserInbox.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Repository.Interface
{
    public interface IUserMessageRepository
    {
        Task<bool> AddUserMessage(UserMessage message);
    }
}
