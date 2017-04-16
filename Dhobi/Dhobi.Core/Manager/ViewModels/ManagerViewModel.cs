using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dhobi.Core.Manager.ViewModels
{
    public class ManagerViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Not a valid email")]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public List<string> Roles { get; set; }
    }
}
