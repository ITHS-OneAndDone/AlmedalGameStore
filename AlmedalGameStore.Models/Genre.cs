using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AlmedalGameStore.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

      

        
    }
}
