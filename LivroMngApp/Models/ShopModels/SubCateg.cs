﻿using LivroMngApp.Constants;
using System;

namespace LivroMngApp.Models.ShopModels
{
    public class SubCateg
    {
        public int SubCategoryId { get; set; }
        public string Name { get; set; }
        public int CategoryRefId { get; set; }
        public Uri GetPhotoUri => string.IsNullOrWhiteSpace(Photo) ?
    new Uri($"{ServerConstants.BaseUrl2}/content/No_image_available.png") :
    new Uri($"{ServerConstants.BaseUrl}/WebImage/GetImage/{Photo}");
        public string Photo { get; set; }
    }
}