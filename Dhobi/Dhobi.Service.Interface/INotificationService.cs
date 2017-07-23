using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Service.Interface
{
    public interface INotificationService
    {
        Task<bool> SendNotification();
    }
}
