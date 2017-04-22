using Dhobi.Business.Implementation;
using Dhobi.Business.Interface;
using Dhobi.Repository.Implementation;
using Dhobi.Repository.Interface;
using Ninject;

namespace Dhobi.DependencyInjection
{
    public class DependencyServiceRegister
    {
        public void Register(IKernel kernel)
        {
            #region Manager
            kernel.Bind<IManagerBusiness>().To<ManagerBusiness>();
            kernel.Bind<IManagerRepository>().To<ManagerRepository>();
            #endregion

            #region Dobi
            kernel.Bind<IDobiBusiness>().To<DobiBusiness>();
            kernel.Bind<IDobiRepository>().To<DobiRepository>();
            #endregion
            #region User
            kernel.Bind<IUserBusiness>().To<UserBusiness>();
            kernel.Bind<IUserRepository>().To<UserRepository>();
            #endregion
            #region SMS
            kernel.Bind<IRegistrationSmsRepository>().To<RegistrationSmsRepository>();
            #endregion
        }
    }
}