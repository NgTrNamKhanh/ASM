using ASM.Data;
using ASM.Models;
using ASM.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASM.Controllers
{
    [Authorize(Roles = "Staff")]
    public class StaffsController : Controller
    {
        private readonly ASMContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StaffsController(ASMContext dbContext, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> ViewOrders()
        {
			var orders = await _dbContext.Order
							.Include(x => x.OrderStatus)
							.Include(x => x.OrderDetails)
                            .ThenInclude(y => y.Product)
							.ToListAsync();
			return View(orders);
        }
		[HttpPost]
		[ActionName("VerifyOrder")]
		public async Task<IActionResult> VerifyOrder(int? id)
        {
            var staffId = GetUserId();
            if (string.IsNullOrEmpty(staffId))
            {
                throw new Exception("User not logged in");
            }
            if (id == null || _dbContext.Order == null)
			{
				return NotFound();
			}
			var order = await _dbContext.Order
			.Include(p => p.OrderStatus)
			.FirstOrDefaultAsync(m => m.OrderId == id);
            // Update the order status
            order.OrderStatusID = 3;
            order.StaffId = staffId;

            _dbContext.Update(order);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("ViewOrders");
		}

        [HttpPost]
        [ActionName("ShippedOrder")]
        public async Task<IActionResult> ShippedOrder(int? id)
        {
            var staffId = GetUserId();
            if (string.IsNullOrEmpty(staffId))
            {
                throw new Exception("User not logged in");
            }
            if (id == null || _dbContext.Order == null)
            {
                return NotFound();
            }
            var order = await _dbContext.Order
            .Include(p => p.OrderStatus)
            .FirstOrDefaultAsync(m => m.OrderId == id);
            // Update the order status
            order.OrderStatusID = 2;
            order.StaffId = staffId;

            _dbContext.Update(order);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("ViewOrders");
        }
        [HttpPost]
        [ActionName("PendingOrder")]
        public async Task<IActionResult> PendingOrder(int? id)
        {
            var staffId = GetUserId();
            if (string.IsNullOrEmpty(staffId))
            {
                throw new Exception("User not logged in");
            }
            if (id == null || _dbContext.Order == null)
            {
                return NotFound();
            }
            var order = await _dbContext.Order
            .Include(p => p.OrderStatus)
            .FirstOrDefaultAsync(m => m.OrderId == id);
            // Update the order status
            order.OrderStatusID = 1;
            order.StaffId = staffId;

            _dbContext.Update(order);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("ViewOrders");
        }
        [HttpPost]
        [ActionName("CancelledOrder")]
        public async Task<IActionResult> CancelledOrder(int? id)
        {
            var staffId = GetUserId();
            if (string.IsNullOrEmpty(staffId))
            {
                throw new Exception("User not logged in");
            }
            if (id == null || _dbContext.Order == null)
            {
                return NotFound();
            }
            var order = await _dbContext.Order
            .Include(p => p.OrderStatus)
            .FirstOrDefaultAsync(m => m.OrderId == id);
            // Update the order status
            order.OrderStatusID = 4;
            order.StaffId = staffId;

            _dbContext.Update(order);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("ViewOrders");
        }
        [HttpPost]
        [ActionName("ReturnedOrder")]
        public async Task<IActionResult> ReturnedOrder(int? id)
        {
            var staffId = GetUserId();
            if (string.IsNullOrEmpty(staffId))
            {
                throw new Exception("User not logged in");
            }
            if (id == null || _dbContext.Order == null)
            {
                return NotFound();
            }
            var order = await _dbContext.Order
            .Include(p => p.OrderStatus)
            .FirstOrDefaultAsync(m => m.OrderId == id);
            // Update the order status
            order.OrderStatusID = 5;
            order.StaffId = staffId;

            _dbContext.Update(order);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("ViewOrders");
        }
        [HttpPost]
        [ActionName("RefundOrder")]
        public async Task<IActionResult> RefundOrder(int? id)
        {
            var staffId = GetUserId();
            if (string.IsNullOrEmpty(staffId))
            {
                throw new Exception("User not logged in");
            }
            if (id == null || _dbContext.Order == null)
            {
                return NotFound();
            }
            var order = await _dbContext.Order
            .Include(p => p.OrderStatus)
            .FirstOrDefaultAsync(m => m.OrderId == id);
            // Update the order status
            order.OrderStatusID = 6;
            order.StaffId = staffId;

            _dbContext.Update(order);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("ViewOrders");
        }
        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;

        }
    }
}
