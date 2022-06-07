using FoodDeliveryApp.Constants;
using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class OrdersViewModel : BaseViewModel
    {
        private List<string> _orderStatuses;
        public List<string> OrderStatus { get => _orderStatuses; set => SetProperty(ref _orderStatuses, value); }
        private ObservableRangeCollection<ServerOrder> _orders;
        public ObservableRangeCollection<ServerOrder> Orders { get => _orders; set => SetProperty(ref _orders, value); }
        public Command LoadOrdersCommand { get; }
        private bool isLoggedIn = false;
        public bool IsLoggedIn { get => isLoggedIn; set => SetProperty(ref isLoggedIn, value); }
        public Command<ServerOrder> ItemTapped { get; }
        public DateTime SelectedTime { get; set; } = DateTime.UtcNow.AddHours(3);
        List<ServerOrder> serverOrders;
        private bool canSeeExtraInfo = false;
        public bool CanSeeExtraInfo
        {
            get => canSeeExtraInfo;
            set => SetProperty(ref canSeeExtraInfo, value);
        }
        private bool isPageVisible = false;
        public bool IsPageVisible
        {
            get => isPageVisible;
            set => SetProperty(ref isPageVisible, value);
        }
        public OrdersViewModel()
        {
            Orders = new ObservableRangeCollection<ServerOrder>();
            IsLoggedIn = App.isLoggedIn;
            ItemTapped = new Command<ServerOrder>(OnItemSelected);
            OrderStatus = new List<string>();
            OrderStatus.Clear();
            OrderStatus.Add("Toate");
            OrderStatus.Add(ServerConstants.DefaultOrderStatus);
            OrderStatus.AddRange(ServerConstants.OrderStatusOwner);
            OrderStatus.AddRange(ServerConstants.OrderStatusDriver);
            LoadOrdersCommand = new Command(async () => await ExecuteLoadOrdersCommand());
            CanSeeExtraInfo = App.UserInfo.IsDriver;
        }
        public async Task ExecuteLoadOrdersCommand()
        {
            IsBusy = true;
            try
            {

                serverOrders = App.UserInfo.IsDriver ? await DataStore.GetServerOrders() : await DataStore.GetServerOrders(App.UserInfo.CompanieRefId);
                foreach (var order in serverOrders)
                    order.CompanieName = DataStore.GetCompanie(order.CompanieRefId).Name;
                /*if (Device.RuntimePlatform == Device.Android)
                    serverOrders = await DataStore.GetServerOrders(email);
                else
                    serverOrders = DataStore.GetServerOrders(email).GetAwaiter().GetResult();*/

                var companii = DataStore.GetCompanii(0).ToList();
                if (!string.IsNullOrEmpty(App.UserInfo.Id))
                    serverOrders = serverOrders.FindAll(o => string.IsNullOrWhiteSpace(o.DriverRefId) && o.DriverRefId != App.UserInfo.Id &&
                    (companii.Find(comp => comp.CompanieId == o.CompanieRefId).TipCompanieRefId == 1 ? o.Status != "Plasata" && o.Status != "Preluata" : true));


                lock (Orders)
                {
                    FilterBy(SelectedTime, "Toate");
                    if (Orders.Count > 0)
                    {
                        IsPageVisible = true;
                    }
                    else
                        IsPageVisible = false;
                }
                await Task.Delay(1000);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public void FilterBy(DateTime time, string status)
        {
            Orders.Clear();
            if (serverOrders != null && status == "Toate")
                Orders.AddRange(serverOrders.FindAll(or => or.Created.Day == time.Day && or.Created.Month == time.Month).OrderBy(or => or.CompanieRefId));
            else if (serverOrders != null)
                Orders.AddRange(serverOrders.FindAll(or => or.Created.Day == time.Day && or.Created.Month == time.Month
                && or.Status == status).OrderBy(or => or.CompanieRefId));
        }

        async void OnItemSelected(ServerOrder item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(OrderInfoPage)}?{nameof(OrderInfoViewModel.OrderId)}={item.OrderId}");
        }
    }
}
