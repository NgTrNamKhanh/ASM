﻿using System;
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
    [Authorize(Roles = "Admin")]
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
            return View(staffUsers);
        }
        // GET: Revenue
        public async Task<IActionResult> ViewRevenue()
        {
            var orderDetails = await _db.OrderDetail.Include(od => od.Product).ToListAsync();
            var hello = 1;
            // Group the OrderDetails records by ProductId, and calculate the total revenue for each product
            var productRevenue = orderDetails
                .GroupBy(od => od.ProductId)
                //.Include(od => od.Product)
                .Select(g => new ProductRevenueViewModel
                {
                    ProductName = g.First().Product.ProductName,
                    Revenue = g.Sum(od => od.Quantity * od.Price)
                })
                .OrderByDescending(pr => pr.Revenue)
                .ToList();

            // Pass the productRevenue data to the view
            return View(productRevenue);
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

        // GET: Staff 
        public async Task<IActionResult> EditStaff(string? id)
        {
            if (id == null || _userManager.GetUsersInRoleAsync("Staff") == null)
            {
                return NotFound();
            }

            var staff = await _userManager.FindByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            ViewData["StaffId"] = new SelectList(staff.Id, staff.UserName, staff.Email);
            return View(staff);
        }



        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStaff([Bind("Id,UserName,Email")] IdentityUser staff)
        {
            var userMgr = _service.GetService<UserManager<IdentityUser>>();
            var roleMgr = _service.GetService<RoleManager<IdentityRole>>();
            
            //if (id != staff.Id)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    await userMgr.UpdateAsync(staff);

                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!AdminExists(admin.AdminId))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
                return RedirectToAction(nameof(ViewStaff));
            }
            ViewData["StaffId"] = new SelectList(staff.UserName, staff.Email);
            return View(staff);
        }

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
