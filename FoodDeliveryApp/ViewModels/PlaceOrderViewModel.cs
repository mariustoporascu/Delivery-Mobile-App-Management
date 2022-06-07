using FoodDeliveryApp.Models.ShopModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class PlaceOrderViewModel : BaseViewModel
    {

        private bool hasValidProfile = false;
        public bool HasValidProfile { get => hasValidProfile; set => SetProperty(ref hasValidProfile, value); }
        public ServerOrder OrderCompanie { get; set; }
        public OrderInfo OrderInfo { get; set; }
        public List<ProductInOrder> ProductsInOrderCompanie { get; set; }
        private decimal totalCompanie = 0.00M;
        public List<CartItem> CartItems { get; set; }
        public Command PlaceFinalOrder { get; set; }
        public event EventHandler OnPlaceOrder = delegate { };
        public event EventHandler OnPlaceOrderFailed = delegate { };
        private UserLocation location;
        private string paymentMethod;
        private string estimated;
        public UserLocation Location { get => location; set => SetProperty(ref location, value); }
        public string PaymentMethod { get => paymentMethod; set => SetProperty(ref paymentMethod, value); }
        public PlaceOrderViewModel(UserLocation location, string paymentMethod, string estimatedTime)
        {
            HasValidProfile = location != null && !string.IsNullOrWhiteSpace(paymentMethod);
            Location = location;
            estimated = estimatedTime;
            PaymentMethod = paymentMethod;
            PlaceFinalOrder = new Command(async () => await OnClickPlaceOrder());
            if (HasValidProfile)
                LoadPageData();
        }

        public void LoadPageData()
        {
            CartItems = DataStore.GetCartItems();
            ProductsInOrderCompanie = new List<ProductInOrder>();
            foreach (var item in CartItems)
            {
                totalCompanie += item.PriceTotal;
                ProductsInOrderCompanie.Add(new ProductInOrder { ProductRefId = item.ProductId, UsedQuantity = item.Cantitate, ClientComments = item.ClientComments });

            }

            if (ProductsInOrderCompanie.Count > 0)
            {
                OrderCompanie = new ServerOrder
                {
                    CustomerId = App.UserInfo.Email,
                    Status = "Plasata",
                    OrderId = 0,
                    Created = DateTime.UtcNow.AddHours(3),
                    CompanieRefId = CartItems[0].CompanieRefId,
                    OrderLocation = Location,
                    TelephoneOrdered = true,
                    EstimatedTime = estimated,
                    HasUserConfirmedET = true,
                    PaymentMethod = PaymentMethod,
                    TotalOrdered = totalCompanie,
                    TransportFee = totalCompanie >= DataStore.GetCompanie(CartItems[0].CompanieRefId)
                        .TransportFees.Find(fee => fee.CityRefId == DataStore.GetAvailableCities().ToList()
                        .Find(city => city.Name == Location.City).CityId)
                        .MinimumOrderValue ? 0 : DataStore.GetCompanie(CartItems[0].CompanieRefId)
                        .TransportFees.Find(fee => fee.CityRefId == DataStore.GetAvailableCities().ToList()
                        .Find(city => city.Name == Location.City).CityId).TransporFee,
                };
            }
            if (OrderCompanie != null)
            {
                OrderInfo = new OrderInfo
                {
                    FullName = Location.LocationName,
                    OrderInfoId = 0,
                    PhoneNo = Location.NrTelefon,
                    Address = String.Concat(Location.BuildingInfo, ", ", Location.Street, ", ", Location.City)
                };

                OrderCompanie.OrderInfos = OrderInfo;
                OrderCompanie.ProductsInOrder = ProductsInOrderCompanie;
            }


        }

        async Task OnClickPlaceOrder()
        {

            var result = await OrderService.CreateOrder(OrderCompanie);
            if (!string.IsNullOrEmpty(result) && result.Contains("Order placed."))
            {
                DataStore.CleanCart();
                OnPlaceOrder?.Invoke(this, new EventArgs());
            }
            else
                OnPlaceOrderFailed?.Invoke(this, new EventArgs());

        }
    }
}
