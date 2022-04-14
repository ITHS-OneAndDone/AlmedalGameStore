using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmedalGameStore.Models.ViewModels
{
    public class CartVM
    {
        public IEnumerable<Cart> ListCart { get; set; }

        public Order Order { get; set; }
    }
}
