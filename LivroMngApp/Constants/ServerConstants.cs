using System.Collections.Generic;

namespace LivroMngApp.Constants
{
    public static class ServerConstants
    {
#if !DEBUG
        public const string BaseUrl = "https://livromng.topodvlp.website/api";
        public const string BaseUrl2 = "https://livromng.topodvlp.website";
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
        public const string Gdpr = "https://livroprez.topodvlp.website/files/GDPR.pdf";
        public const string Termeni = "https://livroprez.topodvlp.website/files/Termeni.pdf";
        public const string IntrebariOwner = "https://livroprez.topodvlp.website/files/Intrebariowner.pdf";
        public const string IntrebariDriver = "https://livroprez.topodvlp.website/files/Intrebaridriver.pdf";
        public const string TimeUrl = "https://worldtimeapi.org/api/timezone/Europe/Bucharest";

    }
}
