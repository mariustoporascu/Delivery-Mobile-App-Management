using FoodDeliveryApp.Models.ShopModels;
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
            Label = "Cernavoda",
            Type = PinType.Generic
        };
        Dictionary<int, UserLocation> userLocations = new Dictionary<int, UserLocation>();
        public MapsViewModel()
        {
            geoCoder = new Geocoder();
        }

        public async Task LoadMyLocation()
        {
            IEnumerable<Position> aproxLocation = await geoCoder.GetPositionsForAddressAsync("Dacia, Cernavoda, Constanta, Romania");
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
            serverOrders = App.UserInfo.IsDriver ? await DataStore.GetServerOrders() : await DataStore.GetServerOrders(App.UserInfo.CompanieRefId);
            foreach (ServerOrder serverOrder in serverOrders)
            {
                if (serverOrder.Status != "Livrata" && serverOrder.Status != "Refuzata"
                    && serverOrder.Status != "Anulata")
                {
                    if (App.UserInfo.IsDriver && serverOrder.Status != "Plasata" && serverOrder.Status != "Preluata"
                        && (string.IsNullOrWhiteSpace(serverOrder.DriverRefId) || serverOrder.DriverRefId == App.UserInfo.Id))
                        userLocations.Add(serverOrder.OrderId, serverOrder.DeliveryLocation);
                    else if (App.UserInfo.IsOwner)
                    {
                        userLocations.Add(serverOrder.OrderId, serverOrder.DeliveryLocation);
                    }
                }

            }
            return userLocations;
        }
    }
}
