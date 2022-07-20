using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LivroMngApp.Models.ShopModels
{
    public class ServerOrder
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public decimal TotalOrdered { get; set; }
        public decimal TransportFee { get; set; }
        public string PaymentMethod { get; set; }
        public bool TelephoneOrdered { get; set; }
        [JsonProperty("orderLocation")]
        public UserLocation OrderLocation { get; set; }
        public string CustomerId { get; set; }
        public int CompanieRefId { get; set; }
        public string DriverRefId { get; set; }
        public string Comments { get; set; }

        public string CompanieName { get; set; }
        public string EstimatedTime { get; set; }
        public bool? HasUserConfirmedET { get; set; }
        public bool CompanieGaveRating { get; set; } = false;
        public bool DriverGaveRating { get; set; } = false;
        public int RatingClientDeLaSofer { get; set; }
        public int RatingClientDeLaCompanie { get; set; }
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
