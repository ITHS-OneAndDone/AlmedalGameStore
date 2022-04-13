using AlmedalGameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlmedalGameStore.DataAccess.GenericRepository.IGenericRepository
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        int PlusCount(Cart shoppingCart, int count);
        int MinusCount(Cart shoppingCart, int count);


    }
}
