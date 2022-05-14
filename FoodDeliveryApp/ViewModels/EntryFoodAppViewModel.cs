using FoodDeliveryApp.Models.AuthModels;
using FoodDeliveryApp.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class EntryFoodAppViewModel : BaseViewModel
    {
        public Command LogoutCommand { get; }
        public event EventHandler OnLogout = delegate { };
        public EntryFoodAppViewModel()
        {
            Title = "Acasa";
            LogoutCommand = new Command(LogOutFunct);
        }
        void LogOutFunct()
        {
            App.userInfo = new UserModel();
            App.isLoggedIn = false;
            SecureStorage.RemoveAll();
            OnLogout(this, new EventArgs());
        }
    }
}
