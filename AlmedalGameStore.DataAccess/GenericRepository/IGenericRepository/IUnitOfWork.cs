using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmedalGameStore.DataAccess.GenericRepository.IGenericRepository
{
    //Skapa alla repositorys här 
    //Så man kan hålla hanterar all databas operations istället för varje idnividuellt repository
    public interface IUnitOfWork
    {
        IGenreRepository Genre { get; }
        IProductRepository Product { get; }
        //global metod
        ICartRepository Cart { get; }
        IOrderRepository Order { get; }
        IApplicationUserRepository ApplicationUser { get; }
        void Save();
    }
}
