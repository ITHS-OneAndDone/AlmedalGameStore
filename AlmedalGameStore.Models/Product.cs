using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmedalGameStore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        [Required,Range(1,100000)]
        public double Price { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
        public string SystemReq { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int PgRating { get; set; }

        public string Publisher { get; set; }
        [Required]
        public int GenreId { get; set; }
        [ValidateNever]
        public Genre Genre { get; set; }
    }
}
