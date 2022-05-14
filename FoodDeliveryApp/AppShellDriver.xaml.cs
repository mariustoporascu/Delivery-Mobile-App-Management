using FoodDeliveryApp.Constants;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.Views;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodDeliveryApp
{
    public partial class AppShellDriver : Xamarin.Forms.Shell
    {
        public AppShellDriver()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(OrderInfoPage), typeof(OrderInfoPage));
            Routing.RegisterRoute(nameof(ProductInOrderPage), typeof(ProductInOrderPage));
            Task.Run(async () => await DependencyService.Get<IDataStore>().Init().ConfigureAwait(false));
            var locationPermission = GetLocationPermissions().GetAwaiter().GetResult();

            if (Device.RuntimePlatform == Device.Android && locationPermission)
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
