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
            var cartCount = await _cartRepo.AddItem(productId, qty);
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> RemoveItem(int productId)
        {
            var cartCount = await _cartRepo.RemoveItem(productId);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepo.GetUserCart();
            return View(cart);
        }
        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem = await _cartRepo.GetCartItemCount();
            return Ok(cartItem);
        }

        public async Task<IActionResult> CheckOut()
        {
            bool isCheckOut = await _cartRepo.DoCheckOut();
            if (!isCheckOut) 
            {
                throw new Exception("Something wrong happned");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
