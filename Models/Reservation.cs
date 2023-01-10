using System.ComponentModel.DataAnnotations;

namespace resorty.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Room { get; set; }

        [Display(Name = "Check In")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Check Out")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public string? Status { get; set; }

        [Display(Name = "Price")]
        public double? PriceTotal { get; set; }
    }
}
