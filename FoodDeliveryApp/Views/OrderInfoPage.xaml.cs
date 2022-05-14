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
        }

        private async void Picker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (await viewModel.ChangeOrderStatus(Selector.SelectedIndex))
                try
                {
                    await this.DisplayToastAsync("Statusul a fost schimbat.", 1300);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            else
                try
                {
                    await this.DisplayToastAsync("Statusul nu a fost schimbat, reincercati!.", 1300);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (await viewModel.LockDriverOrder())
                try
                {
                    await this.DisplayToastAsync("Comanda a fost blocata pentru tine.", 1300);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            else
                try
                {
                    await this.DisplayToastAsync("Comanda nu s-a putut bloca, reincercati!.", 1300);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
        }
    }
}