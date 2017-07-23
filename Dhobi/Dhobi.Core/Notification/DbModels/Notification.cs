using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Core.Notification.DbModels
{
    public class Notification
    {
        public string NotificationId;
        public int Type;
        public string Text;
        public string Title;
        public string MessageId;
        public long Time;
        public string SenderUserName;
        public string SenderUserId;
        public string ReceiverUserName;
        public string ReceiverUserId;
        public int Status;
    }
}
