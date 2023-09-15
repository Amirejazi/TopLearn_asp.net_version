using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Services.interfaces;
using TopLearn.DataLayer.Entities.Order;

namespace TopLearn.Web.Pages.Admin.Discounts
{
    public class EditDiscountModel : PageModel
    {
        private IOrderService _orderService;

        public EditDiscountModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [BindProperty] public Discount Discount { get; set; }

        public void OnGet(int id)
        {
            Discount = _orderService.GetDiscountById(id);
        }

        public IActionResult OnPost(string startDate = "", string endDate = "")
        {
            if (startDate != "")
            {
                string[] std = startDate.Split('/');
                Discount.StartDate = new DateTime(int.Parse(std[0]), int.Parse(std[1]), int.Parse(std[2]),
                    new PersianCalendar());
            }

            if (endDate != "")
            {
                string[] edd = endDate.Split('/');
                Discount.EndDate = new DateTime(int.Parse(edd[0]), int.Parse(edd[1]), int.Parse(edd[2]),
                    new PersianCalendar());
            }

            _orderService.UpdateDiscount(Discount);
            return RedirectToPage("Index");
        }
    }
}
