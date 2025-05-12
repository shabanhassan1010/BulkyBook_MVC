using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Model
{
    public class ShopingCart
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Add this
        public int Id { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        public int ProductId { get; set; }
        [Range(1, 100, ErrorMessage = "Please Enter A value Between 1 and 1000")]
        public int Count { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser? ApplicationUser { get; set; }
        public string? ApplicationUserId { get; set; }
        [NotMapped]
        public double Price { get; set; }
    }
}