using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Core.UserInbox.DbModels
{
    public class UserMessage
    {
        public string MessageId;
        public string ServiceId;
        public string UserId;
        public string Title;
        public string Message;
        public long Time;
        public int Status;
        public int IsDelivered;
        public int Type;
    }
}
