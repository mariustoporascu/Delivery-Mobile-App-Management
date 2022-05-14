using FoodDeliveryApp.Models.AuthModels;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.Views;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FoodDeliveryApp
{
    public partial class App : Application
    {
        public static UserModel userInfo = new UserModel();
        public const string LOGIN_WITH = "LoginWith";
        public const string WEBEMAIL = "WebEmail";
        public const string WEBPASS = "WebPass";
        public static bool isLoggedIn = false;

        public bool PromptToConfirmExit
        {
            get
            {
                bool promptToConfirmExit = false;
                if (Shell.Current.Navigation.NavigationStack.Count == 1)
                {
                    promptToConfirmExit = true;
                }
                return promptToConfirmExit;
            }
        }
        public App()
        {
            InitializeComponent();

            DependencyService.Register<IDataStore, MockDataStore>();
            DependencyService.Register<IAuthController, AuthService>();
            DependencyService.Register<IOrderServ, OrderServ>();
            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        public bool IsToastExitConfirmation
        {
            get => Preferences.Get(nameof(IsToastExitConfirmation), false);
            set => Preferences.Set(nameof(IsToastExitConfirmation), value);
        }
    }
}
