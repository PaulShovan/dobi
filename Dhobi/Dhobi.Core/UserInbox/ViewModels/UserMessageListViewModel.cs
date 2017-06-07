using Dhobi.Core.UserInbox.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Core.UserInbox.ViewModels
{
    public class UserMessageListViewModel
    {
        public int PageCount;
        public List<UserMessageBasicInformation> Messages;
    }
}
