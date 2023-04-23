using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM.Data;
using ASM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ASM.Constants;

namespace ASM.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminsController : Controller
    {
        private readonly ASMContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IServiceProvider _service;

        public AdminsController(ASMContext db, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager, IServiceProvider service)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _service = service;
            
        }

        // GET: Staffs
        public async Task<IActionResult> ViewStaff()
        {
            var staffUsers = await _userManager.GetUsersInRoleAsync("Staff");
            var staffList = staffUsers.Select(u => new
            {
                u.Id,
                u.UserName,
                u.Email,
            }).ToList();
            return View(staffList);
        }
        // GET: Revenue
        public async Task<IActionResult> ViewRevenue()
        {
            var products = await _db.Product
                    .Include(p => p.AuthorProducts)
                    .ThenInclude(ap => ap.Author)
                    .Include(p => p.CategoryProducts)
                    .ThenInclude(pc => pc.Category)
                    .Include(p => p.OrderDetails)
                    .ThenInclude(od => od.Quantity)
                    .ToListAsync();
            return View(products);
        }
        // GET: Admins/Create
        public IActionResult CreateStaff()
        {
            return View();
        }

        //POST: Admins/Create
        //To protect from overposting attacks, enable the specific properties you want to bind to.
        //For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

       [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStaff([Bind("UserName,Email")] IdentityUser staff, string password)
        {
            var userMgr = _service.GetService<UserManager<IdentityUser>>();
            var roleMgr = _service.GetService<RoleManager<IdentityRole>>();
            //if (ModelState.IsValid)
            //{
                var newStaff = new IdentityUser
                {
                    UserName = staff.UserName,
                    Email = staff.Email,
                    EmailConfirmed = true,
                };
                var staffpassword = password;
                await userMgr.CreateAsync(newStaff, password);
                await userMgr.AddToRoleAsync(newStaff, Roles.Staff.ToString());

                return RedirectToAction(nameof(ViewStaff));
            //}
            //return View(staff);
        }

        //// GET: Admins/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Admin == null)
        //    {
        //        return NotFound();
        //    }

        //    var admin = await _context.Admin.FindAsync(id);
        //    if (admin == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["AdminId"] = new SelectList(_context.Account, "AccountId", "Email", admin.AdminId);
        //    return View(admin);
        //}

        //// POST: Admins/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("AdminId,AdminName")] Admin admin)
        //{
        //    if (id != admin.AdminId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(admin);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!AdminExists(admin.AdminId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["AdminId"] = new SelectList(_context.Account, "AccountId", "Email", admin.AdminId);
        //    return View(admin);
        //}

        //// GET: Admins/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Admin == null)
        //    {
        //        return NotFound();
        //    }

        //    var admin = await _context.Admin
        //        .Include(a => a.Account)
        //        .FirstOrDefaultAsync(m => m.AdminId == id);
        //    if (admin == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(admin);
        //}

        //// POST: Admins/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Admin == null)
        //    {
        //        return Problem("Entity set 'ASMContext.Admin'  is null.");
        //    }
        //    var admin = await _context.Admin.FindAsync(id);
        //    if (admin != null)
        //    {
        //        _context.Admin.Remove(admin);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool AdminExists(int id)
        //{
        //  return (_context.Admin?.Any(e => e.AdminId == id)).GetValueOrDefault();
        //}
    }
}
