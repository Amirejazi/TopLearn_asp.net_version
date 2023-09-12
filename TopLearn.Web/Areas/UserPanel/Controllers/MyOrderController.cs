using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.DTOs.Order;
using TopLearn.Core.Services.interfaces;
using TopLearn.DataLayer.Entities.Order;

namespace TopLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class MyOrderController : Controller
    {
        private IOrderService _orderService;

        public MyOrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View("OrdersList", _orderService.getUserOrders(User.Identity.Name));
        }

        public IActionResult ShowOrder(int id, string type="", bool finaly=false)
        {
            Order order = _orderService.GetOrderForUserPanel(User.Identity.Name, id);
            if (order == null)
            {
                return NotFound();
            }
            ViewBag.finaly = finaly;
            ViewBag.typeDiscount = type;
            return View(order);
        }

        public IActionResult FinalyOrder(int id)
        {
            if (_orderService.FinlayOrder(User.Identity.Name, id))
            {
                return Redirect($"/UserPanel/MyOrder/ShowOrder/{id}?finaly=true");
            }

            return BadRequest();
        }

        public IActionResult UseDiscount(int orderId, string code)
        {
            DiscountUseType type = _orderService.UseDiscount(orderId, code);
            return Redirect($"/UserPanel/MyOrder/ShowOrder/{orderId}?type={type}");
        }
    }
}
