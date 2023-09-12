using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return View();
        }

        public IActionResult ShowOrder(int id, bool finaly=false)
        {
            Order order = _orderService.GetOrderForUserPanel(User.Identity.Name, id);
            if (order == null)
            {
                return NotFound();
            }
            ViewBag.finaly = finaly;
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
    }
}
