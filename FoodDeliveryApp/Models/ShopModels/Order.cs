using System;

namespace FoodDeliveryApp.Models.ShopModels
{
    public class Order : BaseModel
    {
        public int OrderId { get; set; }
        public string _status;
        public string Status { get => _status; set => SetProperty(ref _status, value); }
        public decimal TotalOrdered { get; set; }
        public decimal TransportFee { get; set; }

        public string TotalOrderedInterfata { get; set; }
        public string CustomerId { get; set; }
        private string _estimatedTime;
        public string EstimatedTime { get => _estimatedTime; set => SetProperty(ref _estimatedTime, value); }
        public bool? HasUserConfirmedET { get; set; }
        public bool IsRestaurant { get; set; } = false;
        public int RestaurantRefId { get; set; }
        public string DriverRefId { get; set; }
        public DateTime Created { get; set; }

        private bool _driverGaveRating;
        public bool DriverGaveRating { get => _driverGaveRating; set => SetProperty(ref _driverGaveRating, value); }

        private bool _restaurantGaveRating;
        public bool RestaurantGaveRating { get => _restaurantGaveRating; set => SetProperty(ref _restaurantGaveRating, value); }
        //public int _ratingClient;
        private int _ratingClientDeLaSofer;
        private int _ratingClientDeLaRestaurant;
        //public int RatingClient { get => _ratingClient; set => SetProperty(ref _ratingClient, value); }
        public int RatingClientDeLaSofer { get => _ratingClientDeLaSofer; set => SetProperty(ref _ratingClientDeLaSofer, value); }
        public int RatingClientDeLaRestaurant { get => _ratingClientDeLaRestaurant; set => SetProperty(ref _ratingClientDeLaRestaurant, value); }

    }
}
