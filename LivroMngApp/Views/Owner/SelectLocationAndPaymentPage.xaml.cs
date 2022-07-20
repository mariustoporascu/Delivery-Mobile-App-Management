using LivroMngApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LivroMngApp.Views
{
    public partial class SelectLocationAndPaymentPage : ContentPage
    {
        SelectLocationAndPaymentViewModel viewModel;
        public SelectLocationAndPaymentPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new SelectLocationAndPaymentViewModel();
        }

        async void ClickedGoToFinalize(object sender, EventArgs args)
        {
            if (viewModel.Location != null && !string.IsNullOrWhiteSpace(viewModel.SelMethod) && !string.IsNullOrWhiteSpace(viewModel.Estimated))
                await Navigation.PushModalAsync(new PlaceOrderPage(viewModel.Location, viewModel.SelMethod, viewModel.Estimated));
            else
                await DisplayAlert("Eroare", "Nu ai selectat o modalitate de plata si/sau nu ai adaugat detaliile clientului.", "OK");
        }
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            // Page appearance not animated
            await Navigation.PopModalAsync(true);
        }
        private async void PaymentMethods_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            try
            {
                if (SelectorPaymentMethods.ItemsSource[SelectorPaymentMethods.SelectedIndex].ToString() == viewModel.SelMethod)
                    return;
                viewModel.SelMethod = SelectorPaymentMethods.ItemsSource[SelectorPaymentMethods.SelectedIndex].ToString();
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


        }
        private async void FillClientDetailsClicked(object sender, EventArgs eventArgs)
        {
            try
            {
                await Navigation.PushModalAsync(new UserLocationPage(viewModel));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void Picker2_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            try
            {
                if (Selector2.ItemsSource[Selector2.SelectedIndex].ToString() == viewModel.Estimated)
                    return;
                viewModel.Estimated = Selector2.ItemsSource[Selector2.SelectedIndex].ToString();
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
    }
}