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
        //void UpdateStatus(int id/, string orderStatus, string? paymentStatus = null)/;
        ////void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId);
    }
}
