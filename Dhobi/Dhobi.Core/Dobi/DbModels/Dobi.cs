using Dhobi.Core.Manager.DbModels;

namespace Dhobi.Core.Dobi.DbModels
{
    public class Dobi : DobiBasicInformation
    {
        public string Email;
        public string Address;
        public string EmergencyContactNumber;
        public string PassportNumber;
        public int Age;
        public string Sex;
        public double Salary;
        public ManagerBasicInformation AddedBy;
        public long JoinDate;
    }
}
