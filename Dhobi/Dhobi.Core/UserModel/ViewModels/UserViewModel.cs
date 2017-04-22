using System.ComponentModel.DataAnnotations;

namespace Dhobi.Core.UserModel.ViewModels
{
    public class UserViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}
