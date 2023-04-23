using ASM.Data;
using ASM.Models;
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

        public StaffsController(ASMContext dbContext)
        {
            _dbContext = dbContext;
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
		public async Task<IActionResult> VerifyOrder(int? id)
        {
			if (id == null || _dbContext.Order == null)
			{
				return NotFound();
			}
			var order = await _dbContext.Order
			.Include(p => p.OrderStatus)
			.FirstOrDefaultAsync(m => m.OrderId == id);
			var newOrder = new Order
			{
				OrderId = order.OrderId,
				CustomerId = order.CustomerId,
				OrderDate = order.OrderDate,
				OrderTotalPrice = order.OrderTotalPrice,
				OrderStatusID = 3,
			};
			_dbContext.Update(order);
			return RedirectToAction("ViewOrders");
		}
	}
}
