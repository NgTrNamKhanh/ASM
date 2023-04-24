using ASM.Constants;
using ASM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASM.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<IdentityUser> _signinManager;

        public HomeController(ILogger<HomeController> logger, SignInManager<IdentityUser> signinManager)
		{
			_logger = logger;
            _signinManager = signinManager;
		}
        public IActionResult Index()
		{
			if (_signinManager.IsSignedIn(User)) 
			{
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Authors");
                }
                else if (User.IsInRole("Staff"))
                {
                    return RedirectToAction("ViewOrders", "Staffs");
                }
            }
            return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}