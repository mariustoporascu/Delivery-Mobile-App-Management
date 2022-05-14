using FoodDeliveryApp.Constants;
using FoodDeliveryApp.Models.ShopModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Services
{
    public class OrderServ : IOrderServ
    {
        private HttpClient _httpClient = new HttpClient();
        public async Task<bool> UpdateOrder(int orderId, string status)
        {

            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodappmanage/updatestatus/{orderId}/{status}");
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(uri).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var respInfo = await httpResponseMessage.Content.ReadAsStringAsync();
                if (respInfo.Contains("Order status updated."))
                    return true;

            }
            return false;
        }
        public async Task<bool> LockDriverOrder(string email, int orderId)
        {

            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodappmanage/driverlockorder/{email}/{orderId}");
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(uri).ConfigureAwait(false);

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
            Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodappmanage/adjustproduct");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
    }
}
