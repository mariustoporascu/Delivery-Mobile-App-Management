using FoodDeliveryApp.Views;
using Xamarin.Forms;

namespace FoodDeliveryApp
{
    public partial class AppShellOwner : Xamarin.Forms.Shell
    {
        public AppShellOwner()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(OrderInfoPage), typeof(OrderInfoPage));
            Routing.RegisterRoute(nameof(ProductInOrderPage), typeof(ProductInOrderPage));
        }

    }
}
