using LivroMngApp.Constants;
using LivroMngApp.Models.ShopModels;
using LivroMngApp.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LivroMngApp.ViewModels
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
        public Command RefreshCommand { get; }
        private ObservableRangeCollection<string> _orderStatuses;
        public ObservableRangeCollection<string> OrderStatus { get => _orderStatuses; set => SetProperty(ref _orderStatuses, value); }
        private bool _isPickerVisible = false;
        public bool IsPickerVisible { get => _isPickerVisible; set => SetProperty(ref _isPickerVisible, value); }
        private bool _isPickerVisible2 = false;
        public bool IsPickerVisible2 { get => _isPickerVisible2; set => SetProperty(ref _isPickerVisible2, value); }
        private bool _hasDriver = false;
        public bool HasDriver { get => _hasDriver; set => SetProperty(ref _hasDriver, value); }
        private bool _ownerViewVis = false;
        public bool OwnerViewVis { get => _ownerViewVis; set => SetProperty(ref _ownerViewVis, value); }
        private bool _hasEstimatedTime = false;
        public bool HasEstimatedTime { get => _hasEstimatedTime; set => SetProperty(ref _hasEstimatedTime, value); }
        private bool _canChangeNextStatus = false;
        public bool CanChangeNextStatus { get => _canChangeNextStatus; set => SetProperty(ref _canChangeNextStatus, value); }
        private bool _hasComments = false;
        public bool HasComments { get => _hasComments; set => SetProperty(ref _hasComments, value); }
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
        public StringBuilder PrintingInfo = new StringBuilder();
        public Command ChangeRatingClient { get; }
        public event EventHandler GetRatClient = delegate { };

        public OrderInfoViewModel()
        {
            Title = "Detalii Comanda";
            Items = new ObservableRangeCollection<OrderProductDisplay>();
            RefreshCommand = new Command(RefreshView);
            ChangeRatingClient = new Command(IntermediateClientRating);
            OrderStatus = new ObservableRangeCollection<string>();
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
            try
            {
                if (await OrderService.RateClient(App.UserInfo.IsOwner, OrderId, rating))
                {
                    if (App.UserInfo.IsOwner)
                    {
                        CurrOrder.CompanieGaveRating = true;
                        CurrOrder.RatingClientDeLaCompanie = rating;
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
            catch (Exception)
            {
                return false;
            }

        }
        public async Task<bool> ChangeOrderStatus(int status)
        {
            try
            {
                var changedStatus = OrderStatus[status];
                if (await OrderService.UpdateOrder(OrderId, changedStatus, App.UserInfo.IsOwner))
                {
                    CurrOrder.Status = changedStatus;

                    OrderStatus.Clear();
                    if (App.UserInfo.IsOwner)
                    {
                        var statusesToAdd = new List<string>();
                        if (CurrOrder.Status == "Plasata")
                            statusesToAdd.Add(ServerConstants.OrderStatusOwner[0]);
                        else
                            statusesToAdd.Add(ServerConstants.OrderStatusOwner[ServerConstants.OrderStatusOwner.IndexOf(CurrOrder.Status) + 1]);
                        statusesToAdd.Add(ServerConstants.OrderStatusOwner[ServerConstants.OrderStatusOwner.Count - 1]);
                        OrderStatus.AddRange(statusesToAdd);
                    }
                    else
                    {
                        var statusesToAdd = new List<string>();
                        if (CurrOrder.Status == "In curs de livrare")
                        {
                            statusesToAdd.Add(ServerConstants.OrderStatusDriver[1]);
                            statusesToAdd.Add(ServerConstants.OrderStatusDriver[2]);
                        }
                        else
                            statusesToAdd.Add(ServerConstants.OrderStatusDriver[0]);
                        OrderStatus.AddRange(statusesToAdd);
                    }

                    if (CurrOrder.Status == "Preluata")
                        IsPickerVisible2 = true;
                    else
                        IsPickerVisible2 = false;
                    if (CurrOrder.Status == "Preluata" && !OrderConfirmed)
                        CanChangeNextStatus = false;
                    else
                        CanChangeNextStatus = true;
                    if (CurrOrder.Status.Contains("Predata Soferului") || CurrOrder.Status.Contains("Livrata") || CurrOrder.Status.Contains("Refuzata"))
                        IsPickerVisible = false;
                    return true;

                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public async Task<bool> EstimateOrder(int status)
        {
            try
            {
                var changedStatus = TimpEstimat[status];
                if (await OrderService.EstimateOrder(OrderId, changedStatus))
                {
                    HasEstimatedTime = true;
                    CanChangeNextStatus = false;
                    return true;

                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public async Task<bool> LockDriverOrder()
        {
            try
            {
                if (await OrderService.LockDriverOrder(App.UserInfo.Email, OrderId))
                {
                    HasDriver = true;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public async void RefreshView()
        {
            IsBusy = true;
            try
            {
                if (OrderId > 0)
                {
                    var serverOrders = App.UserInfo.IsDriver ? await DataStore.GetServerOrders() : await DataStore.GetServerOrders(App.UserInfo.CompanieRefId);

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
                    CompanieRefId = order.CompanieRefId,
                    TotalOrdered = order.TotalOrdered,
                    TelephoneOrdered = order.TelephoneOrdered,
                    TransportFee = order.TransportFee,
                    RatingClientDeLaSofer = order.RatingClientDeLaSofer,
                    Comments = order.Comments,
                    RatingClientDeLaCompanie = order.RatingClientDeLaCompanie,
                    PaymentMethod = order.PaymentMethod,
                    CompanieGaveRating = order.CompanieGaveRating,
                    DriverGaveRating = order.DriverGaveRating,
                    TotalOrderedInterfata = order.TotalOrdered + " RON",
                    EstimatedTime = order.EstimatedTime,
                    HasUserConfirmedET = order.HasUserConfirmedET,
                    DriverRefId = order.DriverRefId,
                };

                HasEstimatedTime = !string.IsNullOrWhiteSpace(order.EstimatedTime);
                OrderConfirmed = order.HasUserConfirmedET != null;
                if (OrderConfirmed)
                    StatusConfirmare = (bool)CurrOrder.HasUserConfirmedET ? "Da" : "Nu";
                Items.Clear();
                var itemsInOrder = new ObservableRangeCollection<OrderProductDisplay>();

                Restaurant = DataStore.GetCompanie((int)order.CompanieRefId);
                HasComments = !string.IsNullOrWhiteSpace(order.Comments);
                foreach (var prodInOrder in order.ProductsInOrder)
                {
                    var item = DataStore.GetItem(prodInOrder.ProductRefId);

                    itemsInOrder.Add(new OrderProductDisplay
                    {
                        ProductId = item.ProductId,
                        Name = item.Name,
                        GramajInterfata = item.GramajInterfata,
                        PretInterfata = (item.Price * prodInOrder.UsedQuantity).ToString() + " RON",
                        Cantitate = prodInOrder.UsedQuantity,
                        ClientComments = prodInOrder.ClientComments,
                        HasComments = !string.IsNullOrWhiteSpace(prodInOrder.ClientComments)
                    });
                }
                Items.AddRange(itemsInOrder);

                PrintingInfo.Clear();
                PrintingInfo.AppendLine($"COMANDA NR: {CurrOrder.OrderId}");
                PrintingInfo.AppendLine();
                foreach (var item in Items)
                {
                    PrintingInfo.AppendLine($"{item.Cantitate} X {item.Name}");
                    if (item.HasComments)
                        PrintingInfo.AppendLine($"Comentarii: {item.ClientComments}");
                }
                PrintingInfo.AppendLine();
                PrintingInfo.AppendLine($"TOTAL: {CurrOrder.TotalOrdered}");

                if (App.UserInfo.IsOwner)
                {
                    OwnerViewVis = true;

                    OrderStatus.Clear();
                    var statusesToAdd = new List<string>();
                    if (CurrOrder.Status == "Plasata")
                        statusesToAdd.Add(ServerConstants.OrderStatusOwner[0]);
                    else
                        statusesToAdd.Add(ServerConstants.OrderStatusOwner[ServerConstants.OrderStatusOwner.IndexOf(CurrOrder.Status) + 1]);
                    statusesToAdd.Add(ServerConstants.OrderStatusOwner[ServerConstants.OrderStatusOwner.Count - 1]);

                    OrderStatus.AddRange(statusesToAdd);


                    if (!string.IsNullOrWhiteSpace(CurrOrder.DriverRefId))
                    {
                        HasDriver = true;
                        CurrOrderDriver = order.OrderDriver;
                    }
                    else
                    {
                        HasDriver = false;
                    }
                    if (ServerConstants.OrderStatusDriver.Contains(CurrOrder.Status) || CurrOrder.Status == "Anulata" || CurrOrder.Status == "Predata Soferului")
                        IsPickerVisible = false;
                    else
                        IsPickerVisible = true;
                    if (CurrOrder.Status == "Preluata")
                        IsPickerVisible2 = true;
                    else
                        IsPickerVisible2 = false;
                    if ((CurrOrder.Status.Contains("Livrata") || CurrOrder.Status.Contains("Refuzata") || CurrOrder.Status.Contains("Anulata")) && !CurrOrder.TelephoneOrdered)
                        CanGiveRating = true;
                    else
                        CanGiveRating = false;
                    if (CurrOrder.Status == "Preluata" && !OrderConfirmed)
                        CanChangeNextStatus = false;
                    else
                        CanChangeNextStatus = true;
                }
                else
                {
                    OwnerViewVis = false;

                    if (!string.IsNullOrWhiteSpace(CurrOrder.DriverRefId))
                        HasDriver = true;
                    else
                        HasDriver = false;

                    OrderStatus.Clear();
                    var statusesToAdd = new List<string>();
                    if (CurrOrder.Status == "In curs de livrare")
                    {
                        statusesToAdd.Add(ServerConstants.OrderStatusDriver[1]);
                        statusesToAdd.Add(ServerConstants.OrderStatusDriver[2]);
                    }
                    else
                        statusesToAdd.Add(ServerConstants.OrderStatusDriver[0]);
                    OrderStatus.AddRange(statusesToAdd);


                    CanChangeNextStatus = true;
                    if ((ServerConstants.OrderStatusOwner.Contains(CurrOrder.Status) && !CurrOrder.Status.Contains("Predata Soferului") || CurrOrder.DriverRefId != App.UserInfo.Id)
                        || CurrOrder.Status.Contains("Plasata") || CurrOrder.Status.Contains("Livrata") || CurrOrder.Status.Contains("Refuzata"))
                        IsPickerVisible = false;
                    else
                        IsPickerVisible = true;
                    if ((CurrOrder.Status.Contains("Livrata") || CurrOrder.Status.Contains("Refuzata")) && !CurrOrder.TelephoneOrdered)
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

    }
}
