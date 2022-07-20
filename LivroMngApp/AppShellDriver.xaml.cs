using LivroMngApp.Services;
using LivroMngApp.Views;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LivroMngApp
{
    public partial class AppShellDriver : Xamarin.Forms.Shell
    {
        public AppShellDriver()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(OrderInfoPage), typeof(OrderInfoPage));
            Routing.RegisterRoute(nameof(GoogleDriveViewerPage), typeof(GoogleDriveViewerPage));
            Routing.RegisterRoute(nameof(GenerateTokenPage), typeof(GenerateTokenPage));
            Routing.RegisterRoute(nameof(ResetPasswordPage), typeof(ResetPasswordPage));
            Preferences.Set("LocationServiceRunning", false);

        }

    }
}
