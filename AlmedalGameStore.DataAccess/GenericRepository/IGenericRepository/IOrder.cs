using AlmedalGameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlmedalGameStore.DataAccess.GenericRepository.IGenericRepository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        void Update(Order obj);
        //void UpdateStatus(int id, Enum orderStatus, Enum? paymentStatus = null);
        void UpdateStripeId(Guid id, string sessionId);
    }
}
