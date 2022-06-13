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