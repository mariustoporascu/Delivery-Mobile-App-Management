using FoodDeliveryApp.Constants;
using FoodDeliveryApp.Models.ShopModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Services
{
    public class OrderServ : IOrderServ
    {
        private HttpClient _httpClient;

        public OrderServ()
        {
            _httpClient = new HttpClient();
        }
        private void TryAddHeaders()
        {
            try
            {
                bool authkey = _httpClient.DefaultRequestHeaders.TryGetValues("authkey", out var val);
                bool authid = _httpClient.DefaultRequestHeaders.TryGetValues("authid", out var val2);
                if (!authid && !authkey)
                {
                    _httpClient.DefaultRequestHeaders.Add("authkey", App.UserInfo.LoginToken);
                    _httpClient.DefaultRequestHeaders.Add("authid", App.UserInfo.Email);
                }
                else
                {
                    _httpClient.DefaultRequestHeaders.Remove("authkey");
                    _httpClient.DefaultRequestHeaders.Remove("authid");
                    _httpClient.DefaultRequestHeaders.Add("authkey", App.UserInfo.LoginToken);
                    _httpClient.DefaultRequestHeaders.Add("authid", App.UserInfo.Email);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
        public async Task<bool> UpdateOrder(int orderId, string status, bool isOwner)
        {
            TryAddHeaders();

            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodappmanage/updatestatus/{orderId}&{status}&{isOwner}");
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(uri);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var respInfo = await httpResponseMessage.Content.ReadAsStringAsync();
                if (respInfo.Contains("Order status updated."))
                    return true;

            }
            return false;
        }
        public async Task<bool> RateClient(bool isOwner, int orderId, int rating)
        {
            TryAddHeaders();

            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodappmanage/ratingclient/{isOwner}&{orderId}&{rating}");
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(uri);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return true;

            }
            return false;
        }
        public async Task<bool> EstimateOrder(int orderId, string estTime)
        {
            TryAddHeaders();

            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodappmanage/setesttime/{orderId}&{estTime}");
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(uri);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var respInfo = await httpResponseMessage.Content.ReadAsStringAsync();
                if (respInfo.Contains("estTime : "))
                    return true;

            }
            return false;
        }
        public async Task<bool> LockDriverOrder(string email, int orderId)
        {
            TryAddHeaders();

            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodappmanage/driverlockorder/{email}&{orderId}");
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(uri);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var respInfo = await httpResponseMessage.Content.ReadAsStringAsync();
                if (respInfo.Contains("Order locked."))
                    return true;

            }
            return false;
        }


        public async Task<bool> UpdateProductsInOrder(List<ProductInOrder> productsInOrder)
        {
            TryAddHeaders();

            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodappmanage/adjustproduct");
            var json = JsonConvert.SerializeObject(productsInOrder);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync(uri, data);
            Debug.WriteLine(httpResponseMessage.IsSuccessStatusCode);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ModifyOrder(int orderId, string comment, decimal newTotal)
        {
            TryAddHeaders();

            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodappmanage/adjustOrder/{orderId}&{comment}&" + newTotal.ToString("N2", CultureInfo.InvariantCulture));
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(uri);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var respInfo = await httpResponseMessage.Content.ReadAsStringAsync();
                if (respInfo.Contains("Comanda a fost modificata"))
                    return true;

            }
            return false;
        }
        public async Task<string> CreateOrder(ServerOrder order)
        {
            TryAddHeaders();
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/createorder");
            var json = JsonConvert.SerializeObject(order);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync(uri, data);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var respInfo = await httpResponseMessage.Content.ReadAsStringAsync();
                return respInfo;
            }
            return string.Empty;
        }

        public async Task<bool> ToggleOrdering(int companieId)
        {
            TryAddHeaders();

            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodappmanage/toggleordering/{companieId}");
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(uri);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var respInfo = await httpResponseMessage.Content.ReadAsStringAsync();
                if (respInfo.Contains("Campul a fost modificat"))
                    return true;

            }
            return false;
        }

        public async Task<bool> ToggleProduct(int companieId, int productId)
        {
            TryAddHeaders();

            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodappmanage/toggleproduct/{companieId}&{productId}");
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(uri);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var respInfo = await httpResponseMessage.Content.ReadAsStringAsync();
                if (respInfo.Contains("Campul a fost modificat"))
                    return true;

            }
            return false;
        }
    }
}
