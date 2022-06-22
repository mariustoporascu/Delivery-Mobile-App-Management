using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.ViewModels;
using System;
using System.Diagnostics;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace FoodDeliveryApp.Views
{
    public partial class PlaceOrderPage : ContentPage
    {
        PlaceOrderViewModel vm;
        public PlaceOrderPage(UserLocation location, string paymentMethod, string estimated)
        {
            InitializeComponent();
            BindingContext = vm = new PlaceOrderViewModel(location, paymentMethod, estimated);
            /*if (Device.RuntimePlatform == Device.iOS)
                vm.OnPlaceOrder += OnPlaceOrderApple;
            else
            {*/
            vm.OnPlaceOrder += OnPlaceOrder;
            vm.OnPlaceOrderFailed += OnPlaceOrderFailed;

            //}

        }
        private async void OnPlaceOrderApple(object sender, EventArgs e)
        {
            try
            {
                await this.DisplayToastAsync("Comanda a fost plasata", 2300);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            await Shell.Current.Navigation.PopToRootAsync();
        }
        private async void OnPlaceOrder(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.DisplayToastAsync("Comanda a fost plasata", 1500);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            await Shell.Current.Navigation.PopToRootAsync();
        }
        private async void OnPlaceOrderFailed(object sender, EventArgs e)
        {
            try
            {
                await this.DisplayAlert("Eroare", "Comanda nu s-a putut plasa, va rugam sa reincercati", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            // Page appearance not animated
            await Navigation.PopModalAsync(true);
        }
    }
}