using FoodDeliveryApp.Constants;
using FoodDeliveryApp.Models.AuthModels;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.ViewModels;
using Newtonsoft.Json;
using OneSignalSDK.Xamarin;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodDeliveryApp.Views
{
    public partial class LoginPage : ContentPage
    {
        LoginViewModel vm;
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = vm = new LoginViewModel();
            /*if (Device.RuntimePlatform == Device.iOS)
                vm.OnSignIn += OnSignInApple;
            else*/
            vm.OnSignIn += OnSignIn;
            vm.OnSignInFailed += OnSignInFailed;
            BindingContext = vm;
            if (App.isLoggedIn)
            {
                /*if (Device.RuntimePlatform == Device.iOS)
                    OnSignInApple(this, new EventArgs());
                else*/
                OnSignIn(this, new EventArgs());
            }
        }
        private async void PasswordForgotClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new GenerateTokenPage());
        }
        private async void TermeniClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new GoogleDriveViewerPage(ServerConstants.Termeni));

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void GDPRclicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new GoogleDriveViewerPage(ServerConstants.Gdpr));

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (string.IsNullOrWhiteSpace(App.FirebaseUserToken))
            {
                App.FirebaseUserToken = OneSignal.Default.DeviceState.userId;
                try
                {
                    SecureStorage.SetAsync(App.FBToken, App.FirebaseUserToken).Wait();

                }
                catch (Exception)
                {

                }
            }
            vm.IsBusy = true;
            await onStart();
            if (App.isLoggedIn)
            {
                /*if (Device.RuntimePlatform == Device.iOS)
                    OnSignInApple(this, new EventArgs());
                else*/
                OnSignIn(this, new EventArgs());
            }
            vm.IsBusy = false;
        }
        async Task onStart()
        {
            string loginResult = string.Empty;
            string finalEmail = string.Empty;
            string finalId = string.Empty;
            var authService = DependencyService.Get<IAuthController>();
            var webMail = await SecureStorage.GetAsync(App.WEBEMAIL);
            var webPass = await SecureStorage.GetAsync(App.WEBPASS);
            var lWith = await SecureStorage.GetAsync(App.LOGIN_WITH);
            if (!string.IsNullOrEmpty(lWith))
            {
                if (lWith.Equals("WebLogin"))
                {
                    vm.UserName = webMail;
                    vm.Password = webPass;
                    loginResult = await authService.Execute(new UserModel { Email = webMail, Password = webPass, FireBaseToken = App.FirebaseUserToken }, Constants.AuthOperations.Login);
                    finalEmail = webMail;
                }
            }

            if (loginResult != string.Empty && !loginResult.Contains("Password is wrong.")
                && !loginResult.Contains("Email is wrong or user not existing.") && !loginResult.Contains("Login data invalid."))
            {
                App.isLoggedIn = true;
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                App.UserInfo = JsonConvert.DeserializeObject<UserModel>(loginResult.Trim(), settings);
                App.UserInfo.Email = finalEmail;
                if (string.IsNullOrEmpty(finalId))
                {
                    App.UserInfo.Password = webPass;
                }
            }
        }

        private async void OnSignInApple(object sender, EventArgs e)
        {
            try
            {
                this.DisplayToastAsync("Ai fost autentificat.", 2300);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            vm.IsBusy = true;
            await DependencyService.Get<IDataStore>().Init();
            vm.IsBusy = false;
            if (App.UserInfo.IsOwner)
                App.Current.MainPage = new AppShellOwner();
            else
                App.Current.MainPage = new AppShellDriver();

        }
        private async void OnSignIn(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.DisplayToastAsync("Ai fost autentificat.", 1500);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            vm.IsBusy = true;
            await DependencyService.Get<IDataStore>().Init();
            if (App.UserInfo.IsOwner)
                App.Current.MainPage = new AppShellOwner();
            else
                App.Current.MainPage = new AppShellDriver();
            vm.IsBusy = false;
        }

        private async void OnSignInFailed(object sender, EventArgs e)
        {

            await DisplayAlert("Eroare", "Autentificare esuata", "OK");
        }
    }
}