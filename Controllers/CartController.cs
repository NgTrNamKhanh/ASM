using ASM.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASM.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepo;

        public CartController(ICartRepository cartRepo) 
        {
            _cartRepo = cartRepo;
        }
        public async Task<IActionResult> AddItem(int productId, int qty=1, int redirect=0)
        {
            if (redirect == 0)
                return Ok();
            return RedirectToAction("GetUserCart");
        }
        public IActionResult RemoveItem(int productId)
        {
            return View();
        }
        public IActionResult GetUserCart()
        {
            return View();
        }
        public IActionResult GetCartCount()
        {
            return View();
        }
        public IActionResult GetTotalItemInCart()
        {
            return View();
        }
    }
}
