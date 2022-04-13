using AlmedalGameStore.DataAccess.GenericRepository.IGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmedalGameStore.DataAccess.GenericRepository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Genre = new GenreRepository(_db);
            Product = new ProductRepository(_db);
            Order = new OrderRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            Cart = new CartRepository(_db);

        }
        public IGenreRepository Genre { get; private set; }
        public IProductRepository Product { get; private set; }
        public IOrderRepository Order { get; private set; }
        public ICartRepository Cart { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
