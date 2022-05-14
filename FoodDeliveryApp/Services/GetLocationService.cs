using FoodDeliveryApp.Constants;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FoodDeliveryApp.Services
{
    public class GetLocationService
    {
        readonly bool stopping = false;
        readonly HttpClient _httpClient;
        public GetLocationService()
        {
            _httpClient = new HttpClient();
        }
        partial class DriverLocation
        {
            public string Id { get; set; }
            public double CoordX { get; set; }
            public double CoordY { get; set; }
        }
        public async Task Run(CancellationToken token)
        {
            await Task.Run(async () =>
            {
                while (!stopping)
                {
                    token.ThrowIfCancellationRequested();
                    try
                    {
                        await Task.Delay(2000);
                        var request = new GeolocationRequest(GeolocationAccuracy.High);
                        var location = await Geolocation.GetLocationAsync(request);


                        if (location != null)
                        {
                            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodappmanage/driverupdatelocation");
                            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var driverLocation = new DriverLocation
                            {
                                Id = App.userInfo.Id,
                                CoordX = location.Latitude,
                                CoordY = location.Longitude
                            };
                            var json = JsonConvert.SerializeObject(driverLocation);
                            var data = new StringContent(json, Encoding.UTF8, "application/json");
                            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync(uri, data);
                            if (httpResponseMessage.IsSuccessStatusCode)
                            {
                                Debug.WriteLine(await httpResponseMessage.Content.ReadAsStringAsync());
                            }

                            var message = new LocationMessage
                            {
                                Latitude = location.Latitude,
                                Longitude = location.Longitude
                            };

                            Device.BeginInvokeOnMainThread(() =>
                            {
                                MessagingCenter.Send(message, "Location");
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            var errormessage = new LocationErrorMessage();
                            MessagingCenter.Send(errormessage, "LocationError");
                        });
                    }
                }
                return;
            }, token);
        }
    }
}
