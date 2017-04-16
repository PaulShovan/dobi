namespace Dhobi.Core.Manager.DbModels
{
    public class Manager : ManagerBasicInformation
    {
        public ManagerBasicInformation AddedBy;
        public string Password;
        public long JoinDate;
        public string Address;
    }
}
