using FoodDeliveryApp.ViewModels;
using System;
using Xamarin.Forms;

namespace FoodDeliveryApp.Views
{
    public partial class EntryFoodAppPage : ContentPage
    {
        EntryFoodAppViewModel viewModel;
        public EntryFoodAppPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new EntryFoodAppViewModel();
            viewModel.OnLogout += LogOut;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (App.userInfo.IsOwner)
                Info.Text = "Aici va puteti gestiona comenzile primite, vizualiza detalii despre soferul care a preluat comanda si nu numai.";
            else
                Info.Text = "Aici va puteti gestiona comenzile, bloca anumite comenzi pentru a fi livrate de catre tine, vizualiza pe harta destinatiile pentru livrari si nu numai.";

        }
        private void LogOut(object sender, EventArgs e)
        {
            App.Current.MainPage = new LoginPage();
        }
    }
}