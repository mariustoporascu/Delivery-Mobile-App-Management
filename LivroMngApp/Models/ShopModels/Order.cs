using System;

namespace LivroMngApp.Models.ShopModels
{
    public class Order : BaseModel
    {
        public int OrderId { get; set; }
        public string _status;
        public string Status { get => _status; set => SetProperty(ref _status, value); }
        public decimal TotalOrdered { get; set; }
        public decimal TransportFee { get; set; }
        public bool TelephoneOrdered { get; set; }
        public UserLocation UserLocation { get; set; }
        public string TotalOrderedInterfata { get; set; }
        public string CustomerId { get; set; }
        private string _estimatedTime;
        public string Comments { get; set; }
        public string PaymentMethod { get; set; }
        public string CompanieName { get; set; }

        public string EstimatedTime { get => _estimatedTime; set => SetProperty(ref _estimatedTime, value); }
        public bool? HasUserConfirmedET { get; set; }
        public int CompanieRefId { get; set; }
        public string DriverRefId { get; set; }
        public DateTime Created { get; set; }

        private bool _driverGaveRating;
        public bool DriverGaveRating { get => _driverGaveRating; set => SetProperty(ref _driverGaveRating, value); }

        private bool _restaurantGaveRating;
        public bool CompanieGaveRating { get => _restaurantGaveRating; set => SetProperty(ref _restaurantGaveRating, value); }
        //public int _ratingClient;
        private int _ratingClientDeLaSofer;
        private int _ratingClientDeLaRestaurant;
        //public int RatingClient { get => _ratingClient; set => SetProperty(ref _ratingClient, value); }
        public int RatingClientDeLaSofer { get => _ratingClientDeLaSofer; set => SetProperty(ref _ratingClientDeLaSofer, value); }
        public int RatingClientDeLaCompanie { get => _ratingClientDeLaRestaurant; set => SetProperty(ref _ratingClientDeLaRestaurant, value); }

    }
}
