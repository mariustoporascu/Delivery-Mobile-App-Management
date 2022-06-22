using FoodDeliveryApp.Constants;
using System;
using Xamarin.Forms;

namespace FoodDeliveryApp.Models.ShopModels
{
    public class Item : BaseModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Gramaj { get; set; }
        public string GramajInterfata { get; set; }
        public decimal Price { get; set; }
        public string PretInterfata { get; set; }
        public int MeasuringUnitId { get; set; }
        public int SubCategoryRefId { get; set; }
        private Color _color;
        public Color Color { get => _color; set => SetProperty(ref _color, value); }
        private bool _available = false;
        public bool IsAvailable { get => _available; set => SetProperty(ref _available, value); }
        private string _statusAvail = string.Empty;
        public string StatusAvail { get => _statusAvail; set => SetProperty(ref _statusAvail, value); }
        public string Photo { get; set; }
        public Uri GetPhotoUri => string.IsNullOrWhiteSpace(Photo) ?
    new Uri($"{ServerConstants.BaseUrl2}/content/No_image_available.png") :
    new Uri($"{ServerConstants.BaseUrl}/WebImage/GetImage/{Photo}");


    }
}