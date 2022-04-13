using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlemedalGameStore.Utility
{
    public class Enums
    {
        public enum OrderStatus
        {
            Received,
            Started,
            Shipped
        }

        public enum PaymentMethod
        {
            Swish,
            CreditCard,
            InStore
        }

        public enum ShippingMethod
        {
            PickUpInStore,
            SendHome
        }
    }
}
