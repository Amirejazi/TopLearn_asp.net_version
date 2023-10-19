using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using TopLearn.Core.Services.interfaces;
using TopLearn.DataLayer.Entities.Order;

namespace TopLearn.Web.Pages.Admin.Discounts
{
    //[PermissionChecker(16)]
    public class CreateDiscountModel : PageModel
    {
        private IOrderService _orderService;

        public CreateDiscountModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [BindProperty]
        public Discount Discount { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPost(string startDate="", string endDate="")
        { 
            if(startDate != "")
            {
                string[] std = startDate.Split('/');
                Discount.StartDate = new DateTime(int.Parse(std[0]), int.Parse(std[1]), int.Parse(std[2]), new PersianCalendar());
            }
            if (endDate != "")
            {
                string[] edd = endDate.Split('/');
                Discount.EndDate = new DateTime(int.Parse(edd[0]), int.Parse(edd[1]), int.Parse(edd[2]), new PersianCalendar());
            }

            if (_orderService.IsExistCode(Discount.DiscountCode))
            {
                return Page();
            }

            _orderService.AddDiscount(Discount);
            return RedirectToPage("Index");

        }

        // Admin/Discounts/Creatediscount?handler=checkcode
        // Admin/Discounts/Creatediscount/Checkcode
        
        public IActionResult OnGetCheckCode(string code)
        {
            return Content(_orderService.IsExistCode(code).ToString());
        }
    }
}
