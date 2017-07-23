using Dhobi.Business.Implementation;
using Dhobi.Business.Interface;
using Dhobi.Repository.Implementation;
using Dhobi.Repository.Interface;
using Dhobi.Service.Implementation;
using Dhobi.Service.Interface;
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
            kernel.Bind<IUserSmsRepository>().To<UserSmsRepository>();
            #endregion
            #region PromoOffer
            kernel.Bind<IPromoOfferRepository>().To<PromoOfferRepository>();
            kernel.Bind<IPromoOfferBusiness>().To<PromoOfferBusiness>();
            #endregion
            #region Device
            kernel.Bind<IDeviceStausRepository>().To<DeviceStatusRepository>();
            kernel.Bind<IDeviceStatusBusiness>().To<DeviceStatusBusiness>();
            #endregion
            #region Order
            kernel.Bind<IOrderRepository>().To<OrderRepository>();
            kernel.Bind<IOrderBusiness>().To<OrderBusiness>();
            #endregion
            #region LocationService
            kernel.Bind<ILocationService>().To<LocationService>();
            #endregion
            #region UserMessage
            kernel.Bind<IUserMessageRepository>().To<UserMessageRepository>();
            kernel.Bind<IUserMessageBusiness>().To<UserMessageBusiness>();
            #endregion
            #region AvailableLocation
            kernel.Bind<IAvailableLoacationRepository>().To<AvailableLoacationRepository>();
            kernel.Bind<IAvailableLocationBusiness>().To<AvailableLocationBusiness>();
            #endregion
            #region OrderService
            kernel.Bind<IOrderServiceRepository>().To<OrderServiceRepository>();
            kernel.Bind<IOrderServiceBusiness>().To<OrderServiceBusiness>();
            #endregion
            #region Notification
            kernel.Bind<INotificationRepository>().To<NotificationRepository>();
            kernel.Bind<INotificationService>().To<NotificationService>();
            #endregion

        }
    }
}