using Dhobi.DependencyInjection;
using Dhobi.Repository.Implementation;
using Dhobi.Repository.Interface;
using Dhobi.Service.Interface;
using Ninject;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Service.Notification
{
    public class SendNotification : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                //System.Diagnostics.Debugger.Launch();
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                INotificationService service = (INotificationService)dataMap.Get("service");
                System.Diagnostics.Debugger.Launch();
                service.SendNotification();
                //using (StreamWriter writer =
                //new StreamWriter("D:\\log.txt", true))
                //{
                //    writer.WriteLine("Important data line 1");
                //}
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debugger.Launch(); 
            }
        }
    }
}
