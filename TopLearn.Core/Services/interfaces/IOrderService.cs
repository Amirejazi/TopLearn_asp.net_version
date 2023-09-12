using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.DTOs.Order;
using TopLearn.DataLayer.Entities.Order;

namespace TopLearn.Core.Services.interfaces
{
    public interface IOrderService
    {
        int AddOrder(string userName, int courseId);
        void UpdatePriceOrder(int orderId);
        Order GetOrderForUserPanel(string userName, int orderId);
        Order GetOrdeById(int orderId);
        void UpdateOrder(Order order);
        bool FinlayOrder(string userName, int orderId);
        List<Order> getUserOrders(string userName);

        #region Discount

        DiscountUseType UseDiscount(int orderId, string code);

        #endregion
    }
}
