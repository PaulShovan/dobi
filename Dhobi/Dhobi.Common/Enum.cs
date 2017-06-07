﻿namespace Dhobi.Common
{
    public enum SmsStatus
    {
        Unapproved = 0,
        Approved = 1
    }
    public enum ResponseStatus
    {
        Ok = 200,
        BadRequest = 400,
        NotFound = 404
    }
    public enum PromoStatus
    {
        Active = 1,
        InActive = 2
    }
    public enum SmsType
    {
        NewRegistration = 1,
        NewLogin = 2
    }
    public enum DeviceOnlineStatus
    {
        Active = 1,
        Inactive = 2
    }
    public enum OrderStatus
    {
        New = 1,
        Acknowledged = 2,
        Confirmed = 3,
        Cancelled = 4,
        PickedUp = 5,
        InProgress = 6,
        Processed = 7,
        Deliverable = 8,
        OnTheWay = 9,
        Delivered = 10
    }
    public enum MessageStatus
    {
        Unread = 1,
        Read = 2
    }
    public enum MessageDeliveryStatus
    {
        NotDelivered = 1,
        Delivered = 2
    }
    public enum MessageType
    {
        NewOrder = 1
    }
    public enum LocationStatus
    {
        Inactive = 0,
        Active = 1
    }
}
