using FoodDeliveryApp.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FoodDeliveryApp.Models.ShopModels
{
    public class Companie : BaseModel
    {
        public int CompanieId { get; set; }
        public string Name { get; set; }
        public string TelefonNo { get; set; }
        public DateTime Opening { get; set; }
        public int TipCompanieRefId { get; set; }
        public bool IsActive { get; set; }
        private bool _tempClosed = false;
        public bool TemporaryClosed { get => _tempClosed; set => SetProperty(ref _tempClosed, value); }
        [JsonProperty("transportFees")]
        public List<TransportFee> TransportFees { get; set; }
        public Uri GetPhotoUri => string.IsNullOrWhiteSpace(Photo) ?
    new Uri($"{ServerConstants.BaseUrl2}/content/No_image_available.png") :
    new Uri($"{ServerConstants.BaseUrl}/WebImage/GetImage/{Photo}");
        public string Photo { get; set; }
    }
}