using Dhobi.Core.UserInbox.DbModels;
using Dhobi.Repository.Implementation.Base;
using Dhobi.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Repository.Implementation
{
    public class UserMessageRepository : Repository<UserMessage>, IUserMessageRepository
    {
        public async Task<bool> AddUserMessage(UserMessage message)
        {
            await Collection.InsertOneAsync(message);
            return true;
        }
    }
}
