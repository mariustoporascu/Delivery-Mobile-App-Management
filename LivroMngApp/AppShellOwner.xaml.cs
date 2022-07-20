using LivroMngApp.Services;
using LivroMngApp.Views;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LivroMngApp
{
    public partial class AppShellOwner : Xamarin.Forms.Shell
    {
        public AppShellOwner()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(OrderInfoPage), typeof(OrderInfoPage));
            Routing.RegisterRoute(nameof(GoogleDriveViewerPage), typeof(GoogleDriveViewerPage));
            Routing.RegisterRoute(nameof(GenerateTokenPage), typeof(GenerateTokenPage));
            Routing.RegisterRoute(nameof(ResetPasswordPage), typeof(ResetPasswordPage));
            Routing.RegisterRoute(nameof(PrinterPage), typeof(PrinterPage));

        }

    }
}
