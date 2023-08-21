using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.DTOs;
using TopLearn.Core.Services.interfaces;

namespace TopLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class WalletController1 : Controller
    {
        private IUserService _userservice;

        public WalletController1(IUserService userService)
        {
            _userservice = userService;
        }

        [Route("UserPanel/Wallet")]
        public IActionResult Wallet()
        {
            ViewData["ListWallets"] = _userservice.GetWalletsUser(User.Identity.Name);
            return View();
        }

        [HttpPost("UserPanel/Wallet")]
        public IActionResult Wallet(ChargeWalletViewModel chargeWallet)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ListWallets"] = _userservice.GetWalletsUser(User.Identity.Name);
                return View(chargeWallet);
            }
            int WalletId = _userservice.ChargeWallet(User.Identity.Name, "شارژ حساب", chargeWallet.Amount);

            #region Online Payment

            var payment = new ZarinpalSandbox.Payment(chargeWallet.Amount);
            var res = payment.PaymentRequest("شارژ کیف پول", "https://localhost:7003/OnlinePayment/" + WalletId);

            if (res.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/"+ res.Result.Authority);
            }

            #endregion
            return null;
        }
    }
}
