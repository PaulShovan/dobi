using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace Dhobi.DependencyInjection
{
    public class DependencyResolver
    {
        public static IKernel Kernel { get; set; }
        public void Resolve()
        {
            Kernel = new StandardKernel();
            var serviceRegister = new DependencyServiceRegister();
            serviceRegister.Register(Kernel);
        }
    }
}
