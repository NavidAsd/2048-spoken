using _2048webApp.Models;
using Application.Interface.Pattern;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _2048webApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAllServices _allServices;

        public HomeController(ILogger<HomeController> logger,
            IAllServices allservices)
        {
            _logger = logger;
            _allServices = allservices;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var account = await _allServices.GetAccountDataService.GetAccountDataAsync();
            var result = new AccountViewModel();
            if (account != null)
            {
                var verifyToken = await _allServices.VerifyActiveTokenService.VerifyToeknAsync(account.token);
                if (verifyToken.Success)
                {
                    result.Token = account.token;
                    return View(result);
                }
            }
            var NewToken = await _allServices.GenerateNewTokenService.ExecuteAsync(new Application.Services.AccountLogin.RequestGenerateNewTokenDto
            {
                username = account.username,
                password = account.password,
            });
            if (NewToken.Success)
            {
                result.Token = NewToken.Enything.token;
                await _allServices.GetAccountDataService.UpdateTokenAsync(result.Token);
            }
            else
                result.Message = "برای استفاده از سرویس فرمان گفتاری لطفا اتصال اینترنت خود را بررسی کنید";
            return View(result);
        }


    }
}
