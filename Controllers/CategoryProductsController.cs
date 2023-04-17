using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM.Data;
using ASM.Models;

namespace ASM.Controllers
{
    public class CategoryProductsController : Controller
    {
        private readonly ASMContext _context;

        public CategoryProductsController(ASMContext context)
        {
            _context = context;
        }

        // GET: CategoryProducts
        public async Task<IActionResult> Index()
        {
            var aSMContext = _context.CategoryProduct.Include(c => c.Category).Include(c => c.Product);
            return View(await aSMContext.ToListAsync());
        }

        // GET: CategoryProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CategoryProduct == null)
            {
                return NotFound();
            }

            var categoryProduct = await _context.CategoryProduct
                .Include(c => c.Category)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (categoryProduct == null)
            {
                return NotFound();
            }

            return View(categoryProduct);
        }

        // GET: CategoryProducts/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId");
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId");
            return View();
        }

        // POST: CategoryProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,ProductId")] CategoryProduct categoryProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", categoryProduct.CategoryId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", categoryProduct.ProductId);
            return View(categoryProduct);
        }

        // GET: CategoryProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CategoryProduct == null)
            {
                return NotFound();
            }

            var categoryProduct = await _context.CategoryProduct.FindAsync(id);
            if (categoryProduct == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", categoryProduct.CategoryId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", categoryProduct.ProductId);
            return View(categoryProduct);
        }

        // POST: CategoryProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,ProductId")] CategoryProduct categoryProduct)
        {
            if (id != categoryProduct.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryProductExists(categoryProduct.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", categoryProduct.CategoryId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", categoryProduct.ProductId);
            return View(categoryProduct);
        }

        // GET: CategoryProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CategoryProduct == null)
            {
                return NotFound();
            }

            var categoryProduct = await _context.CategoryProduct
                .Include(c => c.Category)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (categoryProduct == null)
            {
                return NotFound();
            }

            return View(categoryProduct);
        }

        // POST: CategoryProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CategoryProduct == null)
            {
                return Problem("Entity set 'ASMContext.CategoryProduct'  is null.");
            }
            var categoryProduct = await _context.CategoryProduct.FindAsync(id);
            if (categoryProduct != null)
            {
                _context.CategoryProduct.Remove(categoryProduct);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryProductExists(int id)
        {
          return (_context.CategoryProduct?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
    }
}
