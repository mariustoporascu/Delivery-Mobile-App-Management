using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace FoodDeliveryApp.Views
{
    public partial class ChangeTotalAndLeaveCommPopUp : Popup
    {
        ChangeTotalAndLeaveCommPopUpVM viewModel;
        public ChangeTotalAndLeaveCommPopUp(int orderId)
        {
            InitializeComponent();
            BindingContext = viewModel = new ChangeTotalAndLeaveCommPopUpVM(orderId);
        }

        void OnDismissButtonClicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }

        async void ModifyClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Order.Comments))
            {
                await Shell.Current.DisplayAlert("Eroare", "Trebuie sa adaugi un comentariu legat de modificare.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(viewModel.Order.TotalOrdered.ToString()))
            {
                await Shell.Current.DisplayAlert("Eroare", "Campul total nu poate fi gol.", "OK");
                return;
            }
            if (await viewModel.OrderService.ModifyOrder(viewModel.Order.OrderId, viewModel.Order.Comments, viewModel.Order.TotalOrdered))
            {
                await Shell.Current.DisplayAlert("Succes", "Modificarile au fost aplicate.", "OK");
                Dismiss(null);
            }
            else
            {
                await Shell.Current.DisplayAlert("Eroare", "Modificarea nu a avut succes, reincercati.", "OK");
                return;
            }
        }
    }
}