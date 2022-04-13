using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmedalGameStore.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product {get; set;}
        //Tack vare UnitOfWork kan jag komma åt alla repositorys (här är till en dropdown)
        //Selectlistitem behöver en text och value som behöver populeras
        //Vi hämtar alla genres och convertar dem till en selectlist item
        [ValidateNever]
        public IEnumerable<SelectListItem> GenreList { get; set; }
    }
}
