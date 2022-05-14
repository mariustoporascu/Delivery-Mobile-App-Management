using FoodDeliveryApp.Services;
using FoodDeliveryApp.Views;
using System.Threading.Tasks;
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
            Task.Run(async () => await DependencyService.Get<IDataStore>().Init().ConfigureAwait(false));
        }

    }
}
