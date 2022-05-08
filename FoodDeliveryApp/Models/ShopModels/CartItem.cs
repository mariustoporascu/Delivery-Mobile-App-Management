﻿namespace FoodDeliveryApp.Models.ShopModels
{
    public class CartItem : BaseModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Gramaj { get; set; }
        public decimal Price { get; set; }
        public int? ShopId { get; set; }
        public int Canal { get; set; }

        private int _cantitate;
        public int Cantitate
        {
            get => _cantitate;
            set => SetProperty(ref _cantitate, value);
        }
    }
}