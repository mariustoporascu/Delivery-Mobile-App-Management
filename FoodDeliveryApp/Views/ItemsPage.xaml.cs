using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Extensions;
using System.Diagnostics;

namespace FoodDeliveryApp.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel();
            viewModel.FailedToChange += FailedToChange;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadItemsCommand.Execute(null);
            viewModel.CItems = viewModel.DataStore.GetCartItems();
        }

        private async void OnAddItem(object sender, EventArgs e)
        {
            var btnDetails = (ImageButton)sender;
            var cartItem = viewModel.CItems.Find(ci => ci.ProductId == ((Item)btnDetails.CommandParameter).ProductId);
            var item = (Item)btnDetails.CommandParameter;
            if (viewModel.CheckHasAnother())
            {
                var prompt = await DisplayAlert("Confirmati",
                    "Aveti in cos produse de la alta companie. Cosul va fi curatat pentru adaugarea acestui produs", "OK", "Cancel");
                if (prompt)
                {
                    viewModel.DataStore.CleanCart();
                }
                else
                    return;
            }
            Navigation.ShowPopup(new ATCPopUp(cartItem ?? new CartItem
            {
                ProductId = item.ProductId,
                Name = item.Name,
                Gramaj = item.Gramaj,
                Cantitate = 1,
                PriceTotal = item.Price,
                CompanieRefId = App.UserInfo.CompanieRefId
            }));
        }
        private void Entry_Completed(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.SearchItem))
                return;
            viewModel.SearchCommand.Execute(null);
        }
        private async void FailedToChange(object sender, EventArgs e)
        {
            await DisplayAlert("Eroare", "Modificarea nu s-a putut efectua, reincercati!", "OK");
        }

        private async void Switch_Toggled(object sender, EventArgs e)
        {
            try
            {
                var prompt = await DisplayAlert("Confirmati", "Doriti sa modificati statusul pentru acest produs?", "OK", "Cancel");
                if (prompt)
                {
                    var btnDetails = (Button)sender;
                    var item = (Item)btnDetails.CommandParameter;
                    viewModel.ToggleSwitchCommand.Execute(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        //private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (viewModel.SItems.Count > 0)
        //        viewModel.Searching();
        //}
    }
}