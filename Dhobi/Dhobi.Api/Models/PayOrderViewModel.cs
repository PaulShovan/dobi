using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dhobi.Api.Models
{
    public class PayOrderViewModel
    {
        [Required]
        public string ServiceId;
        [Required]
        public decimal Amount;
    }
}