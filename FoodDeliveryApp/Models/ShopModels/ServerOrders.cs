using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FoodDeliveryApp.Models.ShopModels
{
    public class ServerOrder
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public decimal TotalOrdered { get; set; }
        public string CustomerId { get; set; }
        public bool IsRestaurant { get; set; }
        public int RestaurantRefId { get; set; }
        public string DriverRefId { get; set; }
        public string EstimatedTime { get; set; }
        public bool? HasUserConfirmedET { get; set; }
        public bool RestaurantGaveRating { get; set; } = false;
        public bool DriverGaveRating { get; set; } = false;
        public int RatingClientDeLaSofer { get; set; }
        public int RatingClientDeLaRestaurant { get; set; }
        public DateTime Created { get; set; }
        [JsonProperty("productsInOrder")]
        public List<ProductInOrder> ProductsInOrder { get; set; }
        [JsonProperty("orderInfo")]
        public OrderInfo OrderInfos { get; set; }
        [JsonProperty("driver")]
        public Driver OrderDriver { get; set; }
        [JsonProperty("location")]
        public UserLocation DeliveryLocation { get; set; }
    }
}
