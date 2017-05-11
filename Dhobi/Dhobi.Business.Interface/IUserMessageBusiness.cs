using Dhobi.Core.UserInbox.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Business.Interface
{
    public interface IUserMessageBusiness
    {
        Task<bool> AddUserMessage(string userId, int messageType);
    }
}
