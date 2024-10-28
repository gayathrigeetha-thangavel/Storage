using System.ComponentModel.DataAnnotations;

namespace Storage.Models
{
    public class Product
    {

        public int Id { get; set; }

        [Display(Name = "Product Name")]
        public required string Name { get; set; }

        [Range(100, 50000)]
        public int Price { get; set; }

        [DataType(DataType.Date)] 
        public DateTime OrderDate { get; set; }

        [StringLength(20)]
        public string Category { get; set; }

        public string Shelf { get; set; }

        public int ProductCount { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
       
    }

} 
