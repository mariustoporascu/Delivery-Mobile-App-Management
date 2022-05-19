using FoodDeliveryApp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodDeliveryApp
{
    public partial class AppShellDriver : Xamarin.Forms.Shell
    {
        public AppShellDriver()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(OrderInfoPage), typeof(OrderInfoPage));
            Routing.RegisterRoute(nameof(ProductInOrderPage), typeof(ProductInOrderPage));
            Preferences.Set("LocationServiceRunning", false);
        }


    }
}
