using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using FoodDeliveryApp.Constants;

namespace FoodDeliveryApp.ViewModels
{
    [QueryProperty(nameof(OrderId), nameof(OrderId))]
    internal class OrderInfoViewModel : BaseViewModel
    {
        private OrderInfo orderInfo;
        private Order order;
        public Order CurrOrder { get => order; set => SetProperty(ref order, value); }
        public OrderInfo CurrOrderInfo { get => orderInfo; set => SetProperty(ref orderInfo, value); }
        private ObservableRangeCollection<OrderProductDisplay> _items;
        public ObservableRangeCollection<OrderProductDisplay> Items { get => _items; set => SetProperty(ref _items, value); }
        public Command<OrderProductDisplay> ItemTapped { get; }
        private List<string> _orderStatuses;
        public List<string> OrderStatus { get => _orderStatuses; set => SetProperty(ref _orderStatuses, value); }
        private bool _isPickerVisible = false;
        public bool IsPickerVisible { get => _isPickerVisible; set => SetProperty(ref _isPickerVisible, value); }
        private bool _hasDriver = false;
        public bool HasDriver { get => _hasDriver; set => SetProperty(ref _hasDriver, value); }
        private bool _ownerViewVis = false;
        public bool OwnerViewVis { get => _ownerViewVis; set => SetProperty(ref _ownerViewVis, value); }
        private Driver orderDriver;
        public Driver CurrOrderDriver { get => orderDriver; set => SetProperty(ref orderDriver, value); }

        private int orderId;

        public OrderInfoViewModel()
        {
            Title = "Detalii Comanda";
            Items = new ObservableRangeCollection<OrderProductDisplay>();
            ItemTapped = new Command<OrderProductDisplay>(async (item) => await OnItemSelected(item));
        }
        public int OrderId
        {
            get
            {
                return orderId;
            }
            set
            {
                orderId = value;
                LoadOrder(value);
            }
        }
        public async Task<bool> ChangeOrderStatus(int status)
        {
            var changedStatus = OrderStatus[status];
            if (await OrderService.UpdateOrder(OrderId, changedStatus))
            {
                CurrOrder.Status = changedStatus;
                if (CurrOrder.Status.Contains("Predata Soferului"))
                    IsPickerVisible = false;
                return true;

            }
            return false;
        }
        public async Task<bool> LockDriverOrder()
        {
            if (await OrderService.LockDriverOrder(App.userInfo.Email, OrderId))
            {
                HasDriver = true;
                return true;
            }
            return false;
        }

        public void LoadOrder(int orderId)
        {
            try
            {
                var order = DataStore.GetOrder(orderId);
                Title = "Detalii Comanda nr. " + orderId;
                CurrOrderInfo = order.OrderInfos;
                CurrOrder = new Order
                {
                    OrderId = orderId,
                    CustomerId = order.CustomerId,
                    Status = order.Status,
                    IsRestaurant = order.IsRestaurant,
                    RestaurantRefId = order.RestaurantRefId,
                    TotalOrdered = order.TotalOrdered,
                    TotalOrderedInterfata = order.TotalOrdered + " RON",
                    DriverRefId = order.DriverRefId,
                };
                Items.Clear();
                var itemsInOrder = new ObservableRangeCollection<OrderProductDisplay>();
                foreach (var prodInOrder in order.ProductsInOrder)
                {
                    var item = DataStore.GetItem(prodInOrder.ProductRefId);
                    itemsInOrder.Add(new OrderProductDisplay
                    {
                        ProductId = item.ProductId,
                        Name = item.Name,
                        GramajInterfata = item.GramajInterfata,
                        PretInterfata = (item.Price * prodInOrder.UsedQuantity).ToString() + " RON",
                        Cantitate = prodInOrder.UsedQuantity
                    });
                }
                Items.AddRange(itemsInOrder);

                if (App.userInfo.IsOwner)
                {
                    OwnerViewVis = true;
                    OrderStatus = ServerConstants.OrderStatusOwner;
                    if (!string.IsNullOrWhiteSpace(CurrOrder.DriverRefId))
                    {
                        HasDriver = true;
                        CurrOrderDriver = order.OrderDriver;
                    }
                    else
                    {
                        HasDriver = false;
                    }
                    if (ServerConstants.OrderStatusDriver.Contains(CurrOrder.Status))
                        IsPickerVisible = false;
                    else
                        IsPickerVisible = true;
                }
                else
                {
                    OwnerViewVis = false;
                    if (!string.IsNullOrWhiteSpace(CurrOrder.DriverRefId))
                        HasDriver = true;
                    else
                        HasDriver = false;
                    OrderStatus = ServerConstants.OrderStatusDriver;
                    if ((ServerConstants.OrderStatusOwner.Contains(CurrOrder.Status) && !CurrOrder.Status.Contains("Predata Soferului")) || CurrOrder.Status.Contains("Plasata"))
                        IsPickerVisible = false;
                    else
                        IsPickerVisible = true;
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load order");
            }
        }
        async Task OnItemSelected(OrderProductDisplay item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ProductInOrderPage)}?{nameof(ProductInOrderViewModel.ItemId)}={item.ProductId}");
        }
    }
}
