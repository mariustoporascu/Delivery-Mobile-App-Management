using FoodDeliveryApp.ViewModels;
using Xamarin.Forms;

namespace FoodDeliveryApp.Views
{
    public partial class MyOrdersPage : ContentPage
    {
        MyOrdersViewModel viewModel;
        public MyOrdersPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new MyOrdersViewModel();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (App.isLoggedIn)
            {
                viewModel.IsLoggedIn = true;
                await viewModel.ExecuteLoadOrdersCommand();
                ItemsListView.ItemsSource = viewModel.Orders;
                if (viewModel.Orders.Count > 0)
                {
                    ItemsListView.ScrollTo(0, position: ScrollToPosition.Start);
                }
            }
            else
            {
                viewModel.IsLoggedIn = false;
                viewModel.IsBusy = false;
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ItemsListView.ItemsSource = null;
        }

    }
}