using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Model
{
    public class ShopingCart
    {
        public int Id { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public int ProductId { get; set; }
        [Range(1, 100, ErrorMessage = "Please Enter A value Between 1 and 1000")]
        public int Count { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

    }
}