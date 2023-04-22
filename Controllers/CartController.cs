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
        public IActionResult AddItem(int productId, int qty=1)
        {
            return View();
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
