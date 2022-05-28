using FoodDeliveryApp.Constants;
using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class MyOrdersViewModel : BaseViewModel
    {
        private ObservableRangeCollection<ServerOrder> _orders;
        public ObservableRangeCollection<ServerOrder> Orders { get => _orders; set => SetProperty(ref _orders, value); }
        private List<string> _orderStatuses;
        public List<string> OrderStatus { get => _orderStatuses; set => SetProperty(ref _orderStatuses, value); }
        public Command LoadOrdersCommand { get; }
        private bool isLoggedIn = false;
        public bool IsLoggedIn { get => isLoggedIn; set => SetProperty(ref isLoggedIn, value); }
        public Command<ServerOrder> ItemTapped { get; }
        public DateTime SelectedTime { get; set; } = DateTime.UtcNow.AddHours(3);
        List<ServerOrder> serverOrders;

        private bool isPageVisible = false;
        public bool IsPageVisible
        {
            get => isPageVisible;
            set => SetProperty(ref isPageVisible, value);
        }
        public MyOrdersViewModel()
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
        }
        public async Task ExecuteLoadOrdersCommand()
        {
            IsBusy = true;
            try
            {
                serverOrders = App.userInfo.IsDriver ? await DataStore.GetServerOrders().ConfigureAwait(false) : await DataStore.GetServerOrders(App.userInfo.RestaurantRefId).ConfigureAwait(false);

                /*if (Device.RuntimePlatform == Device.Android)
                    serverOrders = await DataStore.GetServerOrders(email).ConfigureAwait(false);
                else
                    serverOrders = DataStore.GetServerOrders(email).ConfigureAwait(false).GetAwaiter().GetResult();*/

                serverOrders = serverOrders.FindAll(o => !string.IsNullOrWhiteSpace(o.DriverRefId) && o.DriverRefId == App.userInfo.Id);

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
                Orders.AddRange(serverOrders.FindAll(or => or.Created.Day == time.Day && or.Created.Month == time.Month));
            else if (serverOrders != null)
                Orders.AddRange(serverOrders.FindAll(or => or.Created.Day == time.Day && or.Created.Month == time.Month
                && or.Status == status));
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
