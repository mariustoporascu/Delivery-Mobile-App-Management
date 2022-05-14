using FoodDeliveryApp.Models.MapsModels;
using FoodDeliveryApp.Models.ShopModels;
using FoodDeliveryApp.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace FoodDeliveryApp.ViewModels
{
    public class MapsViewModel : BaseViewModel
    {
        public Geocoder geoCoder;
        public Pin pinRoute1 = new Pin
        {
            Label = "Cernavoda"
        };
        Dictionary<int, UserLocation> userLocations = new Dictionary<int, UserLocation>();
        public MapsViewModel()
        {
            geoCoder = new Geocoder();
        }

        public async Task LoadMyLocation()
        {
            IEnumerable<Position> aproxLocation = await geoCoder.GetPositionsForAddressAsync("Centru, Cernavoda, Romania");
            if (aproxLocation.Count() > 0)
            {
                Position position1 = aproxLocation.FirstOrDefault();
                pinRoute1.Position = position1;
            }
        }
        public async Task<Dictionary<int, UserLocation>> GetUserLocations()
        {
            userLocations.Clear();
            List<ServerOrder> serverOrders;
            serverOrders = App.userInfo.IsDriver ? await DataStore.GetServerOrders().ConfigureAwait(false) : await DataStore.GetServerOrders(App.userInfo.RestaurantRefId).ConfigureAwait(false);
            foreach (ServerOrder serverOrder in serverOrders)
            {
                if (serverOrder.Status != "Livrata" && serverOrder.Status != "Refuzata"
                    && serverOrder.Status != "Anulata" && App.userInfo.IsDriver ? serverOrder.DriverRefId == App.userInfo.Id : true)
                    userLocations.Add(serverOrder.OrderId, serverOrder.DeliveryLocation);

            }
            return userLocations;
        }
    }
}
