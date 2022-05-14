using FoodDeliveryApp.Models.ShopModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Services
{
    public interface IOrderServ
    {
        Task<bool> UpdateOrder(int orderId, string status);
        Task<bool> LockDriverOrder(string email, int orderId);
        Task<bool> UpdateProductsInOrder(List<ProductInOrder> productsInOrder);
    }
}
