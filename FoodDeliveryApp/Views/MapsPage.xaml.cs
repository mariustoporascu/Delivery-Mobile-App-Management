using FoodDeliveryApp.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FoodDeliveryApp.Views
{
    public partial class MapsPage : ContentPage
    {
        MapsViewModel mapsViewModel;
        public MapsPage()
        {
            InitializeComponent();
            BindingContext = mapsViewModel = new MapsViewModel();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await mapsViewModel.LoadMyLocation();
            if (mapsViewModel.pinRoute1.Position != null)
            {
                try
                {
                    if (AppMap.Pins.FirstOrDefault(pin => pin.Label == "Cernavoda") == null)
                        AppMap.Pins.Add(mapsViewModel.pinRoute1);
                    else
                        AppMap.Pins.FirstOrDefault(pin => pin.Label == "Cernavoda").Position = mapsViewModel.pinRoute1.Position;
                    AppMap.MoveToRegion(MapSpan.FromCenterAndRadius(mapsViewModel.pinRoute1.Position, Distance.FromMeters(100)));
                    AppMap.Pins.ToList().RemoveAll(prop => prop.Label != "Cernavoda");
                    var forDelivery = await mapsViewModel.GetUserLocations();
                    if (forDelivery != null)
                        foreach (var location in forDelivery)
                        {
                            AppMap.Pins.Add(new Pin
                            {
                                Label = $"Comanda nr {location.Key}",
                                Type = PinType.Place,
                                Position = new Position(location.Value.CoordX, location.Value.CoordY)
                            });
                        }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

            }
        }

    }
}