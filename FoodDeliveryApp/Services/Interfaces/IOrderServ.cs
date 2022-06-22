using FoodDeliveryApp.Models.ShopModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Services
{
    public interface IOrderServ
    {
        Task<string> CreateOrder(ServerOrder order);

        Task<bool> UpdateOrder(int orderId, string status, bool isOwner);
        Task<bool> RateClient(bool isOwner, int orderId, int rating);
        Task<bool> EstimateOrder(int orderId, string estTime);
        Task<bool> LockDriverOrder(string email, int orderId);
        Task<bool> UpdateProductsInOrder(List<ProductInOrder> productsInOrder);
        Task<bool> ModifyOrder(int orderId, string comment, decimal newTotal);
        Task<bool> ToggleOrdering(int companieId);
        Task<bool> ToggleProduct(int companieId, int productId);
    }
}
