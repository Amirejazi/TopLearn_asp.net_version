using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopLearn.Core.DTOs.Course;
using TopLearn.Core.Services.interfaces;
using TopLearn.Web.Models;

namespace TopLearn.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUserService _userService;
        private ICourseService _courseService;

        public HomeController(ILogger<HomeController> logger, IUserService userService, ICourseService courseService)
        {
            _logger = logger;
            _userService = userService;
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            return View(_courseService.GetCourses());
        }

        [Route("OnlinePayment/{id}")]
        public IActionResult OnlinePayment(int id)
        {
            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
                HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"];

                var wallet = _userService.GetWalletByWalletId(id);
                var payment = new ZarinpalSandbox.Payment(wallet.Amount);
                var res = payment.Verification(authority).Result;
                if (res.Status == 100)
                {
                    ViewBag.Code = res.RefId;
                    ViewBag.IsSuccess = true;
                    wallet.IsPay=true;
                    _userService.UpdateWallet(wallet);
                    return View();
                }
            }
            return View();
        }

        public IActionResult GetSubGroup(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "انتخاب کنید",
                    Value = ""
                }
            };

            list.AddRange(_courseService.GetSubGroupsForManage(id));
            return Json(new SelectList(list, "Value", "Text"));
        }

        [HttpPost("file-upload")]
        public IActionResult UploadImage(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();



            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/images/CKeditorUploaded",
                fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);

            }



            var url = $"/images/CKeditorUploaded/{fileName}";


            return Json(new { uploaded = true, url });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}