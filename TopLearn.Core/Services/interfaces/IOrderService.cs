using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.Order;

namespace TopLearn.Core.Services.interfaces
{
    public interface IOrderService
    {
        int AddOrder(string userName, int courseId);
        void UpdatePriceOrder(int orderId);
        Order GetOrderForUserPanel(string userName, int orderId);
        bool FinlayOrder(string userName, int orderId);
    }
}
