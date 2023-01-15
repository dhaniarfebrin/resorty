using System.ComponentModel.DataAnnotations;

namespace resorty.Models
{
    public class Bedroom
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        public int Price { get; set; }

        [Display(Name = "Available")]
        public int Stocks { get; set; }
    }
}
