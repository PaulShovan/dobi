namespace Dhobi.Common
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
        OnTheWay = 8,
        Delivered = 9
    }
}
