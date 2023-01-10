using System.ComponentModel.DataAnnotations;

namespace resorty.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Display(Name = "Room")]
        public string Name { get; set; }
        public string Floor { get; set; }
        public int? Price { get; set; }
        public string? Status { get; set; }
    }
}
