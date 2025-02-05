﻿using LivroMngApp.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace LivroMngApp.Views
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
            if (App.UserInfo.IsDriver)
                AppMap.IsShowingUser = true;
            else
                AppMap.IsShowingUser = false;
            await mapsViewModel.LoadMyLocation();
            if (mapsViewModel.pinRoute1.Position != null)
            {
                try
                {
                    AppMap.Pins.Clear();
                    if (AppMap.Pins.FirstOrDefault(pin => pin.Label == "Cernavoda") == null)
                        AppMap.Pins.Add(mapsViewModel.pinRoute1);
                    else
                        AppMap.Pins.FirstOrDefault(pin => pin.Label == "Cernavoda").Position = mapsViewModel.pinRoute1.Position;
                    AppMap.MoveToRegion(MapSpan.FromCenterAndRadius(mapsViewModel.pinRoute1.Position, Distance.FromMeters(100)));
                    var forDelivery = await mapsViewModel.GetUserLocations();
                    if (forDelivery != null)
                    {
                        var index = 0;
                        foreach (var location in forDelivery)
                        {
                            var order = mapsViewModel.DataStore.GetOrder(location.Key);
                            var companie = mapsViewModel.DataStore.GetCompanie(order.CompanieRefId);
                            if (AppMap.Pins.FirstOrDefault(pinz => pinz.Position.Latitude == location.Value.CoordX
                                 && pinz.Position.Longitude == location.Value.CoordY) != null)
                            {
                                var oldPin = AppMap.Pins.FirstOrDefault(pinz => pinz.Position.Latitude == location.Value.CoordX
                                && pinz.Position.Longitude == location.Value.CoordY);
                                oldPin.Address = oldPin.Address + ", " + (!string.IsNullOrWhiteSpace(order.DriverRefId) ?
                                    $"Comanda {(App.UserInfo.IsDriver ? companie.Name : "")} nr {location.Key}" : $"Comanda {(App.UserInfo.IsDriver ? companie.Name : "")} fara livrator nr {location.Key}");
                            }
                            else
                            {
                                AppMap.Pins.Add(new Pin
                                {
                                    Label = $"Locatia {index + 1}",
                                    Address = !string.IsNullOrWhiteSpace(order.DriverRefId) ?
                                    $"Comanda {(App.UserInfo.IsDriver ? companie.Name : "")} nr {location.Key}" : $"Comanda {(App.UserInfo.IsDriver ? companie.Name : "")} fara livrator nr {location.Key}",
                                    Type = PinType.Place,
                                    Position = new Position(location.Value.CoordX, location.Value.CoordY),

                                });
                                index++;
                            }


                        }
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