using System.Collections.Generic;

namespace FoodDeliveryApp.Constants
{
    public static class ServerConstants
    {
#if !DEBUG
        public const string BaseUrl = "https://manage.livro.ro/api";
        public const string BaseUrl2 = "https://manage.livro.ro";
#else
        public const string BaseUrl = "http://livro.sytes.net/foodapp/api";
        public const string BaseUrl2 = "http://livro.sytes.net/foodapp";
#endif
        public static List<string> OrderStatusOwner = new List<string>{
            "Preluata","In pregatire","Pregatita pentru livrare","Predata Soferului","Anulata"
            };
        public static List<string> OrderStatusDriver = new List<string>{
            "In curs de livrare","Refuzata","Livrata"
            };
        public const string DefaultOrderStatus = "Plasata";
        public const string Gdpr = "https://manage.livro.ro/files/GDPR.pdf";
        public const string Termeni = "https://manage.livro.ro/files/Termeni.pdf";
        public const string IntrebariOwner = "https://manage.livro.ro/files/Intrebariowner.pdf";
        public const string IntrebariDriver = "https://manage.livro.ro/files/Intrebaridriver.pdf";
        public const string TimeUrl = "https://worldtimeapi.org/api/timezone/Europe/Bucharest";

    }
}
