using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Core.OrderModel.ViewModels
{
    public class OrderPickupTimeViewModel
    {
        [Required]
        public string ServiceId { get; set; }
        [Required]
        public string PickupDate { get; set; }
        [Required]
        public string PickupTime { get; set; }
    }
}
