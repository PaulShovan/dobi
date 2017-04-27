using System.ComponentModel.DataAnnotations;

namespace Dhobi.Core.PromoOffer.ViewModels
{
    public class PromoViewModel
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public long StartDate { get; set; }
        [Required]
        public long EndDate { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
}
