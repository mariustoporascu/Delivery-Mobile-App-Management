using System.Collections.Generic;

namespace FoodDeliveryApp.Constants
{
    public static class ServerConstants
    {
        public const string BaseUrl = "https://fooddelivapp.somee.com";
        public static List<string> OrderStatusOwner = new List<string>{
            "Preluata","Pregatita pentru livrare","Predata Soferului","Anulata"
            };
        public static List<string> OrderStatusDriver = new List<string>{
            "In curs de livrare","Refuzata","Livrata"
            };
        public const string DefaultOrderStatus = "Plasata";
    }
}
