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
    public class MyOrdersViewModel : BaseViewModel
    {
        private ObservableRangeCollection<Order> _orders;
        public ObservableRangeCollection<Order> Orders { get => _orders; set => SetProperty(ref _orders, value); }
        private List<string> _orderStatuses;
        public List<string> OrderStatus { get => _orderStatuses; set => SetProperty(ref _orderStatuses, value); }
        public Command LoadOrdersCommand { get; }
        private bool isLoggedIn = false;
        public bool IsLoggedIn { get => isLoggedIn; set => SetProperty(ref isLoggedIn, value); }
        public Command<Order> ItemTapped { get; }
        public DateTime SelectedTime { get; set; } = DateTime.UtcNow.AddHours(3);
        List<ServerOrder> serverOrders;
        List<Order> uiOrders;

        private bool isPageVisible = false;
        public bool IsPageVisible
        {
            get => isPageVisible;
            set => SetProperty(ref isPageVisible, value);
        }
        private bool isLoading = false;
        public MyOrdersViewModel()
        {
            Orders = new ObservableRangeCollection<Order>();
            IsLoggedIn = App.isLoggedIn;
            ItemTapped = new Command<Order>(OnItemSelected);
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
            if (!isLoading)
            {
                isLoading = true;
                IsBusy = true;
                try
                {
                    uiOrders = new List<Order>();

                    serverOrders = App.UserInfo.IsDriver ? await DataStore.GetServerOrders() : await DataStore.GetServerOrders(App.UserInfo.CompanieRefId);

                    foreach (var order in serverOrders)
                        order.CompanieName = DataStore.GetCompanie(order.CompanieRefId).Name;
                    serverOrders = serverOrders.FindAll(o => !string.IsNullOrWhiteSpace(o.DriverRefId) && o.DriverRefId == App.UserInfo.Id);

                    lock (Orders)
                    {
                        foreach (var serverOrder in serverOrders)
                            uiOrders.Add(new Order
                            {
                                OrderId = serverOrder.OrderId,
                                Status = serverOrder.Status,
                                CompanieName = serverOrder.CompanieName,
                                PaymentMethod = serverOrder.PaymentMethod,
                                TotalOrdered = serverOrder.TotalOrdered + serverOrder.TransportFee,
                                Created = serverOrder.Created,
                            });
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
                    isLoading = false;
                }

            }

        }
        public void FilterBy(DateTime time, string status)
        {
            Orders.Clear();
            if (serverOrders != null && status == "Toate")
                Orders.AddRange(uiOrders.FindAll(or => or.Created.Day == time.Day && or.Created.Month == time.Month).OrderBy(or => or.CompanieRefId));
            else if (serverOrders != null)
                Orders.AddRange(uiOrders.FindAll(or => or.Created.Day == time.Day && or.Created.Month == time.Month
                && or.Status == status).OrderBy(or => or.CompanieRefId));
        }
        async void OnItemSelected(Order item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(OrderInfoPage)}?{nameof(OrderInfoViewModel.OrderId)}={item.OrderId}");
        }
    }
}
