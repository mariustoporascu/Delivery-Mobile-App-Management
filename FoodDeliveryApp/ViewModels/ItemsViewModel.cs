﻿using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{

    public class ItemsViewModel : BaseViewModel
    {
        private ObservableRangeCollection<Grouping<SubCateg, Item>> _itemsSubCateg;
        public ObservableRangeCollection<Grouping<SubCateg, Item>> ItemsSubCateg { get => _itemsSubCateg; set => SetProperty(ref _itemsSubCateg, value); }
        public List<Item> SItems { get; set; }
        public List<SubCateg> SSubCateg { get; set; }
        public List<Categ> SCateg { get; set; }
        public List<CartItem> CItems { get; set; }
        private string searchItem = "";
        private Item _selectedItem;
        public Command LoadItemsCommand { get; }
        /*public Command<Item> MinusCommand { get; }
        public Command<Item> PlusCommand { get; }*/
        public Command SearchCommand { get; }
        public Command<Item> ItemTapped { get; }

        public string SearchItem
        {
            get => searchItem;
            set => SetProperty(ref searchItem, value);
        }

        public ItemsViewModel()
        {
            Title = "Produse";
            ItemsSubCateg = new ObservableRangeCollection<Grouping<SubCateg, Item>>();
            CItems = new List<CartItem>();
            SItems = new List<Item>();
            SCateg = new List<Categ>();
            SSubCateg = new List<SubCateg>();
            LoadItemsCommand = new Command(ExecuteLoadItemsCommand);
            ItemTapped = new Command<Item>((item) => OnItemSelected(item));
            /*MinusCommand = new Command<Item>(OnMinus);
            PlusCommand = new Command<Item>(OnPlus);*/
            SearchCommand = new Command(Searching);
        }

        void ExecuteLoadItemsCommand()
        {

            try
            {
                SearchItem = "";
                ItemsSubCateg.Clear();
                var newListSub = new ObservableRangeCollection<Grouping<SubCateg, Item>>();
                if (SItems.Count == 0)
                {
                    var items = DataStore.GetItems(App.UserInfo.CompanieRefId, 0);
                    foreach (var item in items)
                    {
                        /*item.Cantitate = 0;*/
                        SItems.Add(item);
                    }
                }
                if (SCateg.Count == 0)
                {
                    var items = DataStore.GetCategories(App.UserInfo.CompanieRefId);
                    foreach (var item in items)
                    {
                        SCateg.Add(item);
                    }
                }
                if (SSubCateg.Count == 0)
                {
                    var items = DataStore.GetSubCategories(0);
                    foreach (var item in items)
                    {
                        SSubCateg.Add(item);
                    }
                }

                foreach (var categ in SCateg)
                    foreach (var subCateg in SSubCateg)
                    {
                        if (subCateg.CategoryRefId == categ.CategoryId)
                        {
                            if (SItems.FindAll(item => item.SubCategoryRefId == subCateg.SubCategoryId).Count > 0)
                            {
                                newListSub.Add(new Grouping<SubCateg, Item>(subCateg, SItems.FindAll(item => item.SubCategoryRefId == subCateg.SubCategoryId)));
                            }
                        }
                    }


                ItemsSubCateg.AddRange(newListSub);
                CItems = DataStore.GetCartItems();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        void Searching()
        {
            if (string.IsNullOrWhiteSpace(SearchItem))
                return;
            try
            {
                ItemsSubCateg.Clear();
                var newListSub = new ObservableCollection<Grouping<SubCateg, Item>>();
                var items = SItems.FindAll(item => item.Name.ToLower().Contains(searchItem.ToLower())
                        || item.Description.ToLower().Contains(searchItem.ToLower()));

                foreach (var categ in SCateg)
                    foreach (var subCateg in SSubCateg)
                    {
                        if (subCateg.CategoryRefId == categ.CategoryId)
                        {
                            if (items.FindAll(item => item.SubCategoryRefId == subCateg.SubCategoryId).Count > 0)
                            {
                                newListSub.Add(new Grouping<SubCateg, Item>(subCateg, items.FindAll(item => item.SubCategoryRefId == subCateg.SubCategoryId)));
                            }
                        }
                    }


                ItemsSubCateg.AddRange(newListSub);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        /*void OnMinus(Item itemVM)
        {
            if (CItems == null)
                return;
            var item = CItems.Find(citem => citem.ProductId == itemVM.ProductId);
            if (itemVM == null || item == null)
                return;
            item.Cantitate--;
            item.PriceTotal = item.Cantitate * itemVM.Price;
            itemVM.Cantitate--;
            if (item.Cantitate == 0)
            {
                CItems.Remove(item);
                DataStore.DeleteFromCart(item);
            }
            else
                DataStore.SaveCart(item);

        }*/
        public bool CheckHasAnother()
        {
            var hasAnotherCompany = CItems.Find(ci => ci.CompanieRefId != App.UserInfo.CompanieRefId);
            if (hasAnotherCompany != null)
                return true;
            return false;
        }
        /*void OnPlus(Item itemVM)
        {
            if (CItems == null)
                return;
            if (itemVM == null)
                return;

            var item = CItems.Find(citem => citem.ProductId == itemVM.ProductId);

            if (item == null)
            {
                item = new CartItem
                {
                    ProductId = itemVM.ProductId,
                    Gramaj = itemVM.Gramaj,
                    Name = itemVM.Name,
                    Cantitate = itemVM.Cantitate,
                    CompanieRefId = RefId
                };
                CItems.Add(item);
            }
            item.Cantitate++;
            item.PriceTotal = item.Cantitate * itemVM.Price;
            itemVM.Cantitate++;
            DataStore.SaveCart(item);

        }*/
        async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.ProductId}&{nameof(ItemDetailViewModel.RefId)}={App.UserInfo.CompanieRefId}");
        }
    }
}