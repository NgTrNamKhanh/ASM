using ASM.Data;
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
	}
}
