using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Services.interfaces;
using TopLearn.DataLayer.Entities.Order;

namespace TopLearn.Web.Pages.Admin.Discounts
{
    //[PermissionChecker(15)]
    public class IndexModel : PageModel
    {
        private IOrderService _orderService;

        public IndexModel(IOrderService orderService)
        {
            _orderService = orderService;
        }
        
        public List<Discount> Discounts { get; set; }

        public void OnGet()
        {
            Discounts = _orderService.GetAllDiscount();
        }
    }
}
