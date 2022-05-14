﻿using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class OrdersViewModel : BaseViewModel
    {
        private ObservableRangeCollection<ServerOrder> _orders;
        public ObservableRangeCollection<ServerOrder> Orders { get => _orders; set => SetProperty(ref _orders, value); }
        public Command LoadOrdersCommand { get; }
        private bool isLoggedIn = false;
        public bool IsLoggedIn { get => isLoggedIn; set => SetProperty(ref isLoggedIn, value); }
        public Command<ServerOrder> ItemTapped { get; }

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

            LoadOrdersCommand = new Command(async () => await ExecuteLoadOrdersCommand());
        }
        public async Task ExecuteLoadOrdersCommand()
        {
            IsBusy = true;
            try
            {
                List<ServerOrder> serverOrders;
                serverOrders = App.userInfo.IsDriver ? await DataStore.GetServerOrders().ConfigureAwait(false) : await DataStore.GetServerOrders(App.userInfo.RestaurantRefId).ConfigureAwait(false);

                /*if (Device.RuntimePlatform == Device.Android)
                    serverOrders = await DataStore.GetServerOrders(email).ConfigureAwait(false);
                else
                    serverOrders = DataStore.GetServerOrders(email).ConfigureAwait(false).GetAwaiter().GetResult();*/
                if (!string.IsNullOrEmpty(App.userInfo.Id))
                    serverOrders = serverOrders.FindAll(o => string.IsNullOrWhiteSpace(o.DriverRefId) && o.DriverRefId != App.userInfo.Id);

                lock (Orders)
                {
                    Orders.Clear();
                    if (serverOrders != null)
                        Orders.AddRange(serverOrders);
                    if (Orders.Count > 0)
                    {
                        IsPageVisible = true;
                    }
                    else
                        IsPageVisible = false;
                }
                Task.Delay(2000).Wait();

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
        async void OnItemSelected(ServerOrder item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(OrderInfoPage)}?{nameof(OrderInfoViewModel.OrderId)}={item.OrderId}");
        }
    }
}
