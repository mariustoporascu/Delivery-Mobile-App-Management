﻿using FoodDeliveryApp.Views;

using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class EntryFoodAppViewModel : BaseViewModel
    {
        public Command SuperMarketCommand { get; }
        public Command RestauranteCommand { get; }
        public EntryFoodAppViewModel()
        {
            Title = "Acasa";
            SuperMarketCommand = new Command(SuperMarketClicked);
            RestauranteCommand = new Command(RestauranteClicked);
        }

        private async void SuperMarketClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"{nameof(CategoryPage)}?{nameof(CategViewModel.Canal)}=1&{nameof(CategViewModel.RefId)}=0");
        }

        private async void RestauranteClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"{nameof(ListaRestaurantePage)}");
        }
    }
}
