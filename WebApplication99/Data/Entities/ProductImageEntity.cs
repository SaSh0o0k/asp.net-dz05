using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication99.Data.Entities
{
    [Table("tblProductImages")]
    public class ProductImageEntity
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(255)]
        public string Name { get; set; }
        public int Priotity { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public ProductEntity Product { get; set; }
    }
}
