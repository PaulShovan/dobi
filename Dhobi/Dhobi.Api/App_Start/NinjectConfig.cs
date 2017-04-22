using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dhobi.DependencyInjection;
using Ninject;

namespace Dhobi.Api.App_Start
{
    public static class NinjectConfig
    {
        public static Lazy<IKernel> CreateKernel = new Lazy<IKernel>(() =>
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            RegisterServices(kernel);

            return kernel;
        });

        private static void RegisterServices(KernelBase kernel)
        {
            new DependencyServiceRegister().Register(kernel);
        }
    }
}