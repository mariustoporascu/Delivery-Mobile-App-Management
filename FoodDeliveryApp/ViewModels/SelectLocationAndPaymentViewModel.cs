using FoodDeliveryApp.Constants;
using FoodDeliveryApp.Models.AuthModels;
using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
{
    public class SelectLocationAndPaymentViewModel : BaseViewModel
    {
        private string _city = string.Empty;
        private string _buildinginfo = string.Empty;
        private string _street = string.Empty;
        private double _coordX;
        private double _coordY;
        private string _name = string.Empty;
        private string _nrTelefon = string.Empty;
        public List<string> TimpEstimat { get; set; }
        public string Estimated;
        public string LocationName { get => _name; set => SetProperty(ref _name, value); }
        public string NrTelefon { get => _nrTelefon; set => SetProperty(ref _nrTelefon, value); }
        public string City { get => _city; set => SetProperty(ref _city, value); }
        public string BuildingInfo { get => _buildinginfo; set => SetProperty(ref _buildinginfo, value); }
        public string Street { get => _street; set => SetProperty(ref _street, value); }
        public double CoordX { get => _coordX; set => SetProperty(ref _coordX, value); }
        public double CoordY { get => _coordY; set => SetProperty(ref _coordY, value); }
        public List<string> AvailableCities { get; set; }

        public UserLocation Location;
        public Command SaveLocation { get; }
        public List<string> PaymentMethods { get; set; }
        public string SelMethod;

        public SelectLocationAndPaymentViewModel()
        {
            Title = "Locatie si modalitate plata";
            PaymentMethods = new List<string>();
            PaymentMethods.Add("Cash la livrare");
            PaymentMethods.Add("Card la livrare");
            AvailableCities = new List<string>();
            AvailableCities.AddRange(DataStore.GetAvailableCities().ToList().Select(city => city.Name));

            SaveLocation = new Command(OnSaveLocation);
            TimpEstimat = new List<string>();
            TimpEstimat.Clear();
            for (int i = 1; i < 100; i++)
            {
                if (i % 5 == 0)
                    TimpEstimat.Add($"{i} min");
            }

        }
        void OnSaveLocation()
        {
            Location = new UserLocation
            {
                LocationName = "Comanda telefon",
                NrTelefon = NrTelefon,
                City = City,
                Street = Street,
                BuildingInfo = BuildingInfo,
                CoordX = CoordX,
                CoordY = CoordY,
            };
        }
    }
}
