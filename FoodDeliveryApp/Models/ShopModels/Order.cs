namespace FoodDeliveryApp.Models.ShopModels
{
    public class Order : BaseModel
    {
        public int OrderId { get; set; }
        public string _status;
        public string Status { get => _status; set => SetProperty(ref _status, value); }
        public decimal TotalOrdered { get; set; }
        public string TotalOrderedInterfata { get; set; }
        public string CustomerId { get; set; }
        public bool IsRestaurant { get; set; } = false;
        public int RestaurantRefId { get; set; }
        public string DriverRefId { get; set; }

    }
}
