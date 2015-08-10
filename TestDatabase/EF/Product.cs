using System.ComponentModel.DataAnnotations;

namespace TestDatabase.EF
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        public decimal PricePerUnit { get; set; }
    }
}
