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
    public class OrderInfoViewModel : BaseViewModel
    {
        private OrderInfo orderInfo;
        private Order order;
        public Order CurrOrder { get => order; set => SetProperty(ref order, value); }
        public OrderInfo CurrOrderInfo { get => orderInfo; set => SetProperty(ref orderInfo, value); }
        private ObservableRangeCollection<OrderProductDisplay> _items;
        public ObservableRangeCollection<OrderProductDisplay> Items { get => _items; set => SetProperty(ref _items, value); }
        private Companie _restaurant;
        public Companie Restaurant { get => _restaurant; set => SetProperty(ref _restaurant, value); }
        public Command<OrderProductDisplay> ItemTapped { get; }
        public Command RefreshCommand { get; }
        private List<string> _orderStatuses;
        public List<string> OrderStatus { get => _orderStatuses; set => SetProperty(ref _orderStatuses, value); }
        private bool _isPickerVisible = false;
        public bool IsPickerVisible { get => _isPickerVisible; set => SetProperty(ref _isPickerVisible, value); }
        private bool _isPickerVisible2 = false;
        public bool IsPickerVisible2 { get => _isPickerVisible2; set => SetProperty(ref _isPickerVisible2, value); }
        private bool _hasDriver = false;
        public bool HasDriver { get => _hasDriver; set => SetProperty(ref _hasDriver, value); }
        private bool _ownerViewVis = false;
        public bool OwnerViewVis { get => _ownerViewVis; set => SetProperty(ref _ownerViewVis, value); }
        private bool _orderConfirmed = false;
        public bool OrderConfirmed { get => _orderConfirmed; set => SetProperty(ref _orderConfirmed, value); }
        private Driver orderDriver;
        public Driver CurrOrderDriver { get => orderDriver; set => SetProperty(ref orderDriver, value); }
        public List<string> TimpEstimat { get; set; }
        private string _statusConfirmare;
        public string StatusConfirmare { get => _statusConfirmare; set => SetProperty(ref _statusConfirmare, value); }

        private int orderId;
        private bool _canGiveRating = false;
        public bool CanGiveRating { get => _canGiveRating; set => SetProperty(ref _canGiveRating, value); }
        public Command ChangeRatingClient { get; }
        public event EventHandler GetRatClient = delegate { };

        public OrderInfoViewModel()
        {
            Title = "Detalii Comanda";
            Items = new ObservableRangeCollection<OrderProductDisplay>();
            ItemTapped = new Command<OrderProductDisplay>(async (item) => await OnItemSelected(item));
            RefreshCommand = new Command(RefreshView);
            ChangeRatingClient = new Command(IntermediateClientRating);

            TimpEstimat = new List<string>();
            TimpEstimat.Clear();
            for (int i = 1; i < 100; i++)
            {
                if (i % 5 == 0)
                    TimpEstimat.Add($"{i} min");
            }
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
        public void IntermediateClientRating()
        {
            GetRatClient?.Invoke(this, new EventArgs());
        }
        public async Task<bool> GiveClientRating(int rating)
        {
            if (await OrderService.RateClient(App.userInfo.IsOwner, OrderId, rating))
            {
                if (App.userInfo.IsOwner)
                {
                    CurrOrder.RestaurantGaveRating = true;
                    CurrOrder.RatingClientDeLaRestaurant = rating;
                }
                else
                {
                    CurrOrder.DriverGaveRating = true;
                    CurrOrder.RatingClientDeLaSofer = rating;
                }
                return true;
            }
            return false;
        }
        public async Task<bool> ChangeOrderStatus(int status)
        {
            var changedStatus = OrderStatus[status];
            if (await OrderService.UpdateOrder(OrderId, changedStatus))
            {
                CurrOrder.Status = changedStatus;
                if (CurrOrder.Status == "Preluata")
                    IsPickerVisible2 = true;
                else
                    IsPickerVisible2 = false;
                if (CurrOrder.Status.Contains("Predata Soferului") || CurrOrder.Status.Contains("Livrata") || CurrOrder.Status.Contains("Refuzata"))
                    IsPickerVisible = false;
                return true;

            }
            return false;
        }
        public async Task<bool> EstimateOrder(int status)
        {
            var changedStatus = TimpEstimat[status];
            if (await OrderService.EstimateOrder(OrderId, changedStatus))
            {

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
        public async void RefreshView()
        {
            IsBusy = true;
            try
            {
                if (OrderId > 0)
                {
                    var serverOrders = App.userInfo.IsDriver ? await DataStore.GetServerOrders() : await DataStore.GetServerOrders(App.userInfo.RestaurantRefId);

                    if (serverOrders.Count > 0)
                        LoadOrder(OrderId);
                }

            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load order");
            }
            IsBusy = false;
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
                    RatingClientDeLaSofer = order.RatingClientDeLaSofer,
                    RatingClientDeLaRestaurant = order.RatingClientDeLaRestaurant,
                    RestaurantGaveRating = order.RestaurantGaveRating,
                    DriverGaveRating = order.DriverGaveRating,
                    TotalOrderedInterfata = order.TotalOrdered + " RON",
                    EstimatedTime = order.EstimatedTime,
                    HasUserConfirmedET = order.HasUserConfirmedET,
                    DriverRefId = order.DriverRefId,
                };
                OrderConfirmed = CurrOrder.HasUserConfirmedET != null;
                if (OrderConfirmed)
                    StatusConfirmare = (bool)CurrOrder.HasUserConfirmedET ? "Da" : "Nu";
                Items.Clear();
                var itemsInOrder = new ObservableRangeCollection<OrderProductDisplay>();
                if (order.IsRestaurant)
                {
                    Restaurant = DataStore.GetRestaurant((int)order.RestaurantRefId);
                }
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
                    if (ServerConstants.OrderStatusDriver.Contains(CurrOrder.Status) || CurrOrder.Status == "Anulata")
                        IsPickerVisible = false;
                    else
                        IsPickerVisible = true;
                    if (CurrOrder.Status == "Preluata")
                        IsPickerVisible2 = true;
                    else
                        IsPickerVisible2 = false;
                    if (CurrOrder.Status.Contains("Livrata") || CurrOrder.Status.Contains("Refuzata") || CurrOrder.Status.Contains("Anulata"))
                        CanGiveRating = true;
                    else
                        CanGiveRating = false;
                }
                else
                {
                    OwnerViewVis = false;

                    if (!string.IsNullOrWhiteSpace(CurrOrder.DriverRefId))
                        HasDriver = true;
                    else
                        HasDriver = false;
                    OrderStatus = ServerConstants.OrderStatusDriver;
                    if ((ServerConstants.OrderStatusOwner.Contains(CurrOrder.Status) && !CurrOrder.Status.Contains("Predata Soferului"))
                        || CurrOrder.Status.Contains("Plasata") || CurrOrder.Status.Contains("Livrata") || CurrOrder.Status.Contains("Refuzata"))
                        IsPickerVisible = false;
                    else
                        IsPickerVisible = true;
                    if (CurrOrder.Status.Contains("Livrata") || CurrOrder.Status.Contains("Refuzata"))
                        CanGiveRating = true;
                    else
                        CanGiveRating = false;

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
