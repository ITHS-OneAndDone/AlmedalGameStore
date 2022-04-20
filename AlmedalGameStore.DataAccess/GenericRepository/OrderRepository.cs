using AlmedalGameStore.DataAccess.GenericRepository.IGenericRepository;
using AlmedalGameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmedalGameStore.DataAccess.GenericRepository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {

        private ApplicationDbContext _db;

        public OrderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Create(Order obj)
        {
            _db.Orders.Add(obj);
        }

        public void Update(Order obj)
        {
            _db.Orders.Update(obj);
        }

        //public void UpdateStatus(int id, Enum orderStatus, Enum? paymentStatus = null)
        //{
        //    var orderFromDb = _db.Orders.FirstOrDefault(u => u.Id == id);
        //    if (orderFromDb != null)
        //    {
        //        orderFromDb.OrderStatus = orderStatus;
        //        if (paymentStatus != null)
        //        {
        //            orderFromDb.Status = paymentStatus;
        //        }

        //    }
        //}
        public void UpdateStripeId(Guid id, string sessionId)
        {
            var orderFromDb = _db.Orders.FirstOrDefault(u => u.OrderId == id);
            //var orderFromDb = GetAll(u => u.OrderId == id);
            
            orderFromDb.SessionId = sessionId;

        }
    }
}
