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
}
