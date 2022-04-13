using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlemedalGameStore.Utility
{
    //gör den statisk för att slippa skapa ett opjekt av klassen
    //håller all statisk details 
    public static class SD
    {
        public const string SessionCart = "SessionShoppingCart";
        public const string Role_Guest = "Guest";
        public const string Role_Admin = "Admin";
        public const string Role_Customer = "Customer";
    }
}
