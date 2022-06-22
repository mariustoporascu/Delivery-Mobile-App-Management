using FoodDeliveryApp.Models.AuthModels;
using FoodDeliveryApp.Models.ShopModels;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class EntryFoodAppViewModel : BaseViewModel
    {
        public Command LogoutCommand { get; }
        public event EventHandler OnLogout = delegate { };
        private string _telNo;
        public string TelNo { get => _telNo; set => SetProperty(ref _telNo, value); }
        private bool _canChangeTelNo;
        public bool CanChangeTelNo { get => _canChangeTelNo; set => SetProperty(ref _canChangeTelNo, value); }
        public Companie Companie { get; set; }
        public EntryFoodAppViewModel()
        {
            Title = "Acasa";
            LogoutCommand = new Command(LogOutFunct);
            TelNo = App.UserInfo.TelNo;
            CanChangeTelNo = App.UserInfo.IsDriver;
        }
        void LogOutFunct()
        {
            App.UserInfo = new UserModel();
            App.isLoggedIn = false;
            SecureStorage.RemoveAll();
            OnLogout(this, new EventArgs());
        }
    }
}
