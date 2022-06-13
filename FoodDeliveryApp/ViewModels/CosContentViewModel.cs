using FoodDeliveryApp.Constants;
using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class CosContentViewModel : BaseViewModel
    {
        private List<Item> SItems;

        private ObservableCollection<CartItem> _items;
        public ObservableCollection<CartItem> Items { get => _items; set => SetProperty(ref _items, value); }
        private bool isPageVisible = false;
        public bool IsPageVisible
        {
            get => isPageVisible;
            set => SetProperty(ref isPageVisible, value);
        }
        private bool isLoggedIn = false;
        public bool IsLoggedIn
        {
            get => isLoggedIn;
            set => SetProperty(ref isLoggedIn, value);
        }
        private decimal _total;
        public decimal Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }
        public Command LoadItemsCommand { get; }
        public Command MinusCommand { get; }
        public Command PlusCommand { get; }
        public Command DeleteCommand { get; }

        public CosContentViewModel()
        {
            Title = "Cos cumparaturi";
            Items = new ObservableCollection<CartItem>();
            LoadItemsCommand = new Command(ExecuteLoadItemsCommand);
            SItems = new List<Item>();

            MinusCommand = new Command<CartItem>(OnMinus);
            PlusCommand = new Command<CartItem>(OnPlus);
            DeleteCommand = new Command<CartItem>(OnDelete);
        }

        void ExecuteLoadItemsCommand()
        {

            try
            {
                Items.Clear();
                Total = 0;
                var items = DataStore.GetCartItems();
                if (SItems.Count == 0)
                    SItems.AddRange(DataStore.GetItems(0, 0));
                foreach (var item in items)
                {
                    Items.Add(item);
                    Total += item.PriceTotal;
                }
                if (items.Count > 0)
                    IsPageVisible = true;
                else
                    IsPageVisible = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        void OnDelete(CartItem item)
        {
            Items.Remove(item);
            DataStore.DeleteFromCart(item);
            RefreshCanExecutes();

        }
        void OnMinus(CartItem item)
        {
            if (item == null)
                return;
            item.Cantitate--;
            item.PriceTotal = item.Cantitate * SItems.Find(prod => prod.ProductId == item.ProductId).Price;
            if (item.Cantitate == 0)
            {
                DataStore.DeleteFromCart(item);
                Items.Remove(item);
            }
            else
                DataStore.SaveCart(item);
            RefreshCanExecutes();
        }
        void OnPlus(CartItem item)
        {
            if (item == null)
                return;
            item.Cantitate++;
            item.PriceTotal = item.Cantitate * SItems.Find(prod => prod.ProductId == item.ProductId).Price;

            DataStore.SaveCart(item);
            RefreshCanExecutes();
        }
        void RefreshCanExecutes()
        {
            Total = 0;
            foreach (var item in Items)
            {
                Total += item.PriceTotal;
            }
            if (Items.Count > 0)
                IsPageVisible = true;
            else
                IsPageVisible = false;

        }
    }
}
