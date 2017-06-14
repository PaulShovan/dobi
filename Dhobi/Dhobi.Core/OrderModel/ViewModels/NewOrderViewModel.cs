﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhobi.Core.OrderModel.ViewModels
{
    public class NewOrderViewModel
    {
        [Required]
        public string Address { get; set; }
        [Required]
        public string Zone { get; set; }

    }
}
