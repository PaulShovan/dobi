using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Core.Dobi.ViewModels
{
    public class DobiViewModel
    {
        [Required]
        public string Name;
        [Required]
        public string Phone;
        [Required]
        public string Email;
        [Required]
        public string Address;
        public string EmergencyContactNumber;
        public string PassportNumber;
        [Required]
        public string IcNumber;
        public string DrivingLicense;
        public int Age;
        public string Sex;
        public double Salary;
    }
}
