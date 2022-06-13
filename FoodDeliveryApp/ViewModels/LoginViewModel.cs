using FoodDeliveryApp.Models.AuthModels;
using FoodDeliveryApp.Services;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string userName;
        private string password;
        public string UserName { get => userName; set => SetProperty(ref userName, value); }
        public string Password { get => password; set => SetProperty(ref password, value); }
        public Command Login { get; }
        public event EventHandler OnSignIn = delegate { };
        public event EventHandler OnSignInFailed = delegate { };
        public LoginViewModel()
        {
            Login = new Command(async () => await AfterSignIn());
        }

        async Task AfterSignIn()
        {
            var authService = DependencyService.Get<IAuthController>();
            string loginResult = await authService.Execute(new UserModel { Email = UserName, Password = Password, FireBaseToken = App.FirebaseUserToken }, Constants.AuthOperations.Login);

            if (!string.IsNullOrWhiteSpace(loginResult) && !loginResult.Contains("Password is wrong.")
                && !loginResult.Contains("Email is wrong or user not existing.") && !loginResult.Contains("Login data invalid."))
            {
                App.isLoggedIn = true;
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                App.UserInfo = JsonConvert.DeserializeObject<UserModel>(loginResult.Trim(), settings);
                App.UserInfo.Email = UserName;
                App.UserInfo.Password = Password;
                SecureStorage.SetAsync(App.WEBEMAIL, UserName).Wait();
                SecureStorage.SetAsync(App.WEBPASS, Password).Wait();
                SecureStorage.SetAsync(App.LOGIN_WITH, "WebLogin").Wait();
                //MessagingCenter.Send<LoginViewModel>(this, "UpdateProfile");
                OnSignIn?.Invoke(this, new EventArgs());

            }
            else
                OnSignInFailed?.Invoke(this, new EventArgs());

        }
    }
}
