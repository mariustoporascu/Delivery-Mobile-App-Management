using LivroMngApp.Models.ShopModels;
using LivroMngApp.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LivroMngApp.ViewModels
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
        public Command LoadItemsCommand { get; }
        public Command SearchCommand { get; }
        public Command<Item> ToggleSwitchCommand { get; }
        public event EventHandler FailedToChange = delegate { };

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
            SearchCommand = new Command(Searching);
            ToggleSwitchCommand = new Command<Item>(async (item) => await ChangeStatus(item));
        }
        async Task ChangeStatus(Item item)
        {
            if (item == null)
                return;
            if (await OrderService.ToggleProduct(App.UserInfo.CompanieRefId, item.ProductId))
            {
                item.IsAvailable = !item.IsAvailable;
                item.StatusAvail = item.IsAvailable ? "Disponibil" : "Indisponibil";
                item.Color = item.IsAvailable ? Color.Green : Color.Red;
            }
            else
                FailedToChange?.Invoke(this, new EventArgs());
        }
        void ExecuteLoadItemsCommand()
        {

            try
            {
                SearchItem = string.Empty;
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

        public bool CheckHasAnother()
        {
            var hasAnotherCompany = CItems.Find(ci => ci.CompanieRefId != App.UserInfo.CompanieRefId);
            if (hasAnotherCompany != null)
                return true;
            return false;
        }

    }
}