using ASM.Constants;
using ASM.Data;
using ASM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ASM.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<IdentityUser> _signinManager;
		private readonly ASMContext _context;

        public HomeController(ILogger<HomeController> logger, SignInManager<IdentityUser> signinManager, ASMContext context)
		{
			_logger = logger;
            _signinManager = signinManager;
			_context = context;
		}
        public async Task<IActionResult> Index()
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
			var genres = await _context.Category.ToListAsync();
			ViewBag.Genres = genres;
            var products = await _context.Product
                .Include(p => p.AuthorProducts)
                .ThenInclude(ap => ap.Author)
                .ToListAsync();
            return View(products);
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