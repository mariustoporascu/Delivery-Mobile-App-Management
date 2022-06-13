using FoodDeliveryApp.Constants;
using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.ViewModels;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodDeliveryApp.Views
{
    public partial class EntryFoodAppPage : ContentPage
    {
        EntryFoodAppViewModel viewModel;
        bool locationGranted = false;
        public EntryFoodAppPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new EntryFoodAppViewModel();
            viewModel.OnLogout += LogOut;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (App.UserInfo.IsOwner)
            {
                Header.Text = $"Administrare comenzi {viewModel.DataStore.GetCompanie(App.UserInfo.CompanieRefId).Name}.";
                Info.Text = "Aici va puteti gestiona comenzile primite, vizualiza detalii despre soferul care a preluat comanda si nu numai.";
            }
            else
            {
                viewModel.TelNo = App.UserInfo.TelNo;
                viewModel.CanChangeTelNo = App.UserInfo.IsDriver;
                Header.Text = $"Administrare comenzi {App.UserInfo.Email}.";
                Info.Text = "Aici va puteti gestiona comenzile, bloca anumite comenzi pentru a fi livrate de catre tine, vizualiza pe harta destinatiile pentru livrari si nu numai.";
                await SetupLocation();

            }
        }
        private void LogOut(object sender, EventArgs e)
        {
            StopService();
            App.Current.MainPage = new LoginPage();
        }
        private async void ChangeNr(object sender, EventArgs e)
        {
            if (await viewModel.AuthController.ChangePhone(viewModel.TelNo) && NrTelefonEntry.IsValid)
            {
                await DisplayAlert("Succes", "Numarul de telefon a fost actualizat", "OK");
                App.UserInfo.TelNo = viewModel.TelNo;
            }
            else
                await DisplayAlert("Eroare", "Numarul de telefon nu a fost actualizat, reincercati", "OK");

        }
        private void CheckFieldNrTelefon(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!Regex.IsMatch(NrTelefon.Text, @"^\d+$"))
                {
                    NrTelefonEntry.IsNotValid = true;
                    NrTelefonEntry.IsValid = false;
                }
                if (!NrTelefonEntry.IsValid)
                {
                    NrTelefon.TextColor = Color.Red;
                    return;
                }
                NrTelefon.TextColor = Color.Black;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
        }
        private async Task SetupLocation()
        {
            locationGranted = await GetLocationPermissions();

            if (Device.RuntimePlatform == Device.Android && locationGranted)
            {
                if (Preferences.Get("LocationServiceRunning", false) == false)
                {
                    MessagingCenter.Subscribe<LocationMessage>(this, "Location", message =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {

                            Debug.WriteLine($"{message.Latitude}, {message.Longitude}, {DateTime.Now.ToLongTimeString()}");
                        });
                    });

                    MessagingCenter.Subscribe<StopServiceMessage>(this, "ServiceStopped", message =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Debug.WriteLine("Location Service has been stopped!");
                        });
                    });

                    MessagingCenter.Subscribe<LocationErrorMessage>(this, "LocationError", message =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Debug.WriteLine("There was an error updating location!");
                        });
                    });

                    StartService();
                }

            }
        }
        private void StartService()
        {
            var startServiceMessage = new StartServiceMessage();
            MessagingCenter.Send(startServiceMessage, "ServiceStarted");
            Preferences.Set("LocationServiceRunning", true);
            Debug.WriteLine("Location Service has been started!");
        }

        private void StopService()
        {
            var stopServiceMessage = new StopServiceMessage();
            MessagingCenter.Send(stopServiceMessage, "ServiceStopped");
            Preferences.Set("LocationServiceRunning", false);
        }

        private async Task<bool> GetLocationPermissions()
        {
            var status = await Xamarin.Essentials.Permissions.CheckStatusAsync<Xamarin.Essentials.Permissions.LocationAlways>();
            if (status == Xamarin.Essentials.PermissionStatus.Granted)
                return true;
            var getPerm = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.LocationAlways>();
            if (getPerm == Xamarin.Essentials.PermissionStatus.Granted)
                return true;
            else
            {
                await this.DisplayToastAsync("Pentru contul de sofer avem nevoie de access la locatia telefonului.", 2300);
                return false;
            }
        }
    }
}