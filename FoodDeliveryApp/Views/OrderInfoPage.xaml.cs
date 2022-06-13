using FoodDeliveryApp.ViewModels;
using System;
using System.Diagnostics;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace FoodDeliveryApp.Views
{
    public partial class OrderInfoPage : ContentPage
    {
        OrderInfoViewModel viewModel;
        public OrderInfoPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new OrderInfoViewModel();
            viewModel.GetRatClient += GetClientRat;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {

                if (viewModel.CurrOrder != null)
                {
                    Selector.SelectedIndex = Selector.ItemsSource.IndexOf(viewModel.CurrOrder.Status);
                    if (viewModel.CurrOrder.EstimatedTime != null)
                    {
                        Selector2.SelectedIndex = Selector2.ItemsSource.IndexOf(viewModel.CurrOrder.EstimatedTime);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void Picker_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            try
            {
                if (Selector.ItemsSource[Selector.SelectedIndex].ToString() == viewModel.CurrOrder.Status)
                    return;
                var prompt = await DisplayAlert("Confirmati", $"Ati selectat {Selector.ItemsSource[Selector.SelectedIndex].ToString()}. Confirmati ca selectia este in regula.", "OK", "Cancel");
                if (prompt)
                {

                    if (await viewModel.ChangeOrderStatus(Selector.SelectedIndex))
                        await DisplayAlert("Succes", "Statusul a fost schimbat.", "OK");

                    else
                        await DisplayAlert("Eroare", "Statusul nu a fost schimbat, reincercati!", "OK");
                }
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
                if (Selector2.ItemsSource[Selector2.SelectedIndex].ToString() == viewModel.CurrOrder.EstimatedTime)
                    return;
                var prompt = await DisplayAlert("Confirmati", $"Ati selectat {Selector2.ItemsSource[Selector2.SelectedIndex].ToString()}. Confirmati ca selectia este in regula.", "OK", "Cancel");
                if (prompt)
                {
                    if (await viewModel.EstimateOrder(Selector2.SelectedIndex))

                        await DisplayAlert("Succes", "Timpul estimat a fost transmis.", "OK");
                    else
                        await DisplayAlert("Eroare", "Timpul estimat nu a fost transmis, reincercati!", "OK");
                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (await viewModel.LockDriverOrder())
                {
                    await DisplayAlert("Succes", "Comanda a fost blocata pentru tine.", "OK");
                    await Shell.Current.Navigation.PopToRootAsync();
                }
                else
                    await DisplayAlert("Eroare", "Comanda nu s-a putut bloca sau ai atins numarul maxim de comenzi, reincercati!", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void GetClientRat(object sender, System.EventArgs e)
        {
            try
            {
                int selected = App.UserInfo.IsOwner ? Rating.SelectedStarValue : Rating2.SelectedStarValue;
                var prompt = await DisplayAlert("Confirmati", $"Ati selectat {selected} stelut{(selected == 1 ? 'a' : 'e')}. Confirmati ca selectia este in regula.", "OK", "Cancel");
                if (prompt)
                {
                    if (await viewModel.GiveClientRating(selected))
                        await DisplayAlert("Succes", "Alegerea ta a fost transmisa.", "OK");
                    else
                        await DisplayAlert("Eroare", "Alegerea ta nu a fost transmisa.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void ModifyClicked(object sender, EventArgs e)
        {
            Navigation.ShowPopup(new ChangeTotalAndLeaveCommPopUp(viewModel.CurrOrder.OrderId));
        }
    }
}