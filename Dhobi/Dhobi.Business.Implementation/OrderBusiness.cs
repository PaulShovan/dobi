﻿using Dhobi.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dhobi.Core.OrderModel.DbModels;
using Dhobi.Repository.Interface;
using Dhobi.Core.OrderModel.ViewModels;
using Dhobi.Service.Interface;
using Dhobi.Core.UserModel.DbModels;
using Dhobi.Core.PromoOffer.ViewModels;
using Dhobi.Common;

namespace Dhobi.Business.Implementation
{
    public class OrderBusiness : IOrderBusiness
    {
        private const int OrderServiceIdLength = 10;
        private IOrderRepository _orderRepository;
        private IPromoOfferBusiness _promoBusiness;
        private IUserMessageBusiness _userMessageBusiness;
        public OrderBusiness(IOrderRepository orderRepository, IPromoOfferBusiness promoOfferBusiness, IUserMessageBusiness userMessageBusiness)
        {
            _orderRepository = orderRepository;
            _promoBusiness = promoOfferBusiness;
            _userMessageBusiness = userMessageBusiness;
        }
        private async Task<PromoOfferBasicInformation> GetPromoOffer()
        {
            var promo = await _promoBusiness.GetPromoOfferForUser();
            if(promo == null)
            {
                return null;
            }
            return new PromoOfferBasicInformation
            {
                PromoText = promo.Text,
                Amount = promo.Amount
            };
        }
        public async Task<bool> AddNewOrder(NewOrderViewModel order, User orderedBy, string zone)
        {
            try
            {
                var totalOrder = await _orderRepository.GetOrderCount();
                var newServiceId = (totalOrder + 1).ToString().PadLeft(OrderServiceIdLength, '0');
                var newOrder = new Order
                {
                    ServiceId = newServiceId,
                    OrderBy = orderedBy,
                    Address = order.Address,
                    Zone = zone,
                    Promotion = await GetPromoOffer(),
                    Status = (int)OrderStatus.New,
                    OrderPlacingTime = Utilities.GetPresentDateTime()
                };
                var addMessageResponse = await _userMessageBusiness.AddUserMessage(orderedBy.UserId, (int)MessageType.NewOrder);
                var addOrderResponse = await _orderRepository.AddNewOrder(newOrder);
                //TODO Send Notification
                return addMessageResponse && addOrderResponse;
            }
            catch (Exception ex)
            {
                throw new Exception("Error placing order." + ex);
            }
        }
    }
}
