using FoodDeliveryApp.ViewModels;
using Xamarin.Forms;

namespace FoodDeliveryApp.Views
{
    public partial class OrdersPage : ContentPage
    {
        OrdersViewModel viewModel;
        public OrdersPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new OrdersViewModel();
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
                StatusPick.SelectedIndex = 0;
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

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            viewModel.SelectedTime = e.NewDate;
            viewModel.FilterBy(e.NewDate, (string)StatusPick.SelectedItem);
        }

        private void Selector_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            viewModel.FilterBy(viewModel.SelectedTime, (string)StatusPick.SelectedItem);
        }
    }
}