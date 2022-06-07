﻿using FoodDeliveryApp.Constants;
using System;

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
        public string Photo { get; set; }
        public Uri GetPhotoUri => string.IsNullOrWhiteSpace(Photo) ?
    new Uri($"{ServerConstants.BaseUrl2}/content/No_image_available.png") :
    new Uri($"{ServerConstants.BaseUrl}/WebImage/GetImage/{Photo}");


    }
}