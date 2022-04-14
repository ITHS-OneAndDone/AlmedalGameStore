using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AlemedalGameStore.Utility.Enums;

namespace AlmedalGameStore.Models
{
    public class Order
    {
        
        public int Id { get; set; }

        [Required]
        public Guid OrderId { get; set; }
       
         public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]

        public ApplicationUser ApplicationUser { get; set; }

        [Required] public string Name  { get; set; }

        [Required] public string Street { get; set; }

        [Required][MaxLength(6)] public string PostalCode { get; set; }

        [Required] public DateTime OrderDate { get; set; }

        [Required] public OrderStatus Status { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }

        [Required] public string Address { get; set; }

        [Required] public PaymentMethod PaymentMethod { get; set; }

        [Required] public ShippingMethod ShippingMethod { get; set; }

        [Required] public double OrderTotal { get; set; }

        public int Amount { get; set; }

    }
}
