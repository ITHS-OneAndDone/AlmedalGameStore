using AlmedalGameStore.DataAccess.GenericRepository.IGenericRepository;
using AlmedalGameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmedalGameStore.DataAccess.GenericRepository
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {

        private ApplicationDbContext _db;

        public CartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public int MinusCount(Cart cart, int count)
        {
            cart.Count -= count;
            return cart.Count;
        }

        public int PlusCount(Cart cart, int count)
        {
            cart.Count += count;
            return cart.Count;
        }
    }
}
