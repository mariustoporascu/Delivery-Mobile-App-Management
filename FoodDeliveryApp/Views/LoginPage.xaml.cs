using FoodDeliveryApp.Models.AuthModels;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.ViewModels;
using Newtonsoft.Json;
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
            BindingContext =  vm = new LoginViewModel();
            if (Device.RuntimePlatform == Device.iOS)
                vm.OnSignIn += OnSignInApple;
            else
                vm.OnSignIn += OnSignIn;
            vm.OnSignInFailed += OnSignInFailed;
            BindingContext = vm;
            if (App.isLoggedIn)
            {
                if (Device.RuntimePlatform == Device.iOS)
                    OnSignInApple(this, new EventArgs());
                else
                    OnSignIn(this, new EventArgs());
            }
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            vm.IsBusy = true;
            await onStart();
            if (App.isLoggedIn)
            {
                if (Device.RuntimePlatform == Device.iOS)
                    OnSignInApple(this, new EventArgs());
                else
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
                    loginResult = await authService.LoginUser(new UserModel { Email = webMail, Password = webPass });
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
                App.userInfo = JsonConvert.DeserializeObject<UserModel>(loginResult.Trim(), settings);
                App.userInfo.Email = finalEmail;
                if (string.IsNullOrEmpty(finalId))
                {
                    App.userInfo.Password = webPass;
                }
            }
        }

        private void OnSignInApple(object sender, EventArgs e)
        {
            try
            {
                this.DisplayToastAsync("Ai fost autentificat.", 2300);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            if (App.userInfo.IsOwner)
                App.Current.MainPage = new AppShellOwner();
            else
                App.Current.MainPage = new AppShellDriver();

        }
        private async void OnSignIn(object sender, EventArgs e)
        {
            try
            {
                await this.DisplayToastAsync("Ai fost autentificat.", 1300);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            if (App.userInfo.IsOwner)
                App.Current.MainPage = new AppShellOwner();
            else
                App.Current.MainPage = new AppShellDriver();
        }

        private async void OnSignInFailed(object sender, EventArgs e)
        {

            await DisplayAlert("Eroare", "Autentificare esuata", "OK");
        }
    }
}