using Dhobi.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Service.Notification
{
    public partial class Service1 : ServiceBase
    {
        NotificationScheduler scheduler;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            InitializeDependencyInjection();
            scheduler = new NotificationScheduler();
            scheduler.Start();
        }

        protected override void OnStop()
        {
            if (scheduler != null)
            {
                scheduler.Stop();
            }
        }
        private static void InitializeDependencyInjection()
        {
            try
            {
                var dependencyResolver = new DependencyResolver();
                dependencyResolver.Resolve();
            }
            catch (Exception)
            {

            }
        }
    }
}
