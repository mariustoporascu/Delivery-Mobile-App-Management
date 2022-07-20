using LivroMngApp.Models.ShopModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivroMngApp.ViewModels
{
    public class ChangeTotalAndLeaveCommPopUpVM : BaseViewModel
    {
        public Order Order { get; set; }
        public ChangeTotalAndLeaveCommPopUpVM(int orderId)
        {
            var order = DataStore.GetOrder(orderId);
            Order = new Order
            {
                OrderId = order.OrderId,
                TotalOrdered = order.TotalOrdered,
                Comments = order.Comments,
            };
        }
    }
}
