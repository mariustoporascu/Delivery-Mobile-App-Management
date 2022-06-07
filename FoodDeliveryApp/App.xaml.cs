using FoodDeliveryApp.Models.AuthModels;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OneSignalSDK.Xamarin;
using System;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FoodDeliveryApp
{
    public partial class App : Application
    {
        private static UserModel userInfo;
        public static UserModel UserInfo { get => userInfo; set => userInfo = value; }
        public const string LOGIN_WITH = "LoginWith";
        public const string WEBEMAIL = "WebEmail";
        public const string WEBPASS = "WebPass";
        public static bool isLoggedIn = false;
        public static string FirebaseUserToken = string.Empty;
        public const string FBToken = "FBToken";

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
            OneSignal.Default.Initialize("67b1b944-bcf4-467a-a6ae-4f0f0512b038");
            OneSignal.Default.PromptForPushNotificationsWithUserResponse();
            FirebaseUserToken = OneSignal.Default.DeviceState.userId;
            try
            {
                SecureStorage.SetAsync(App.FBToken, FirebaseUserToken).Wait();

            }
            catch (Exception)
            {

            }
        }

        protected override async void OnStart()
        {
            base.OnStart();
            FirebaseUserToken = await SecureStorage.GetAsync(App.FBToken);
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
