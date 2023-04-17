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
    public class AuthorProductsController : Controller
    {
        private readonly ASMContext _context;

        public AuthorProductsController(ASMContext context)
        {
            _context = context;
        }

        // GET: AuthorProducts
        public async Task<IActionResult> Index()
        {
            var aSMContext = _context.AuthorProduct.Include(a => a.Author).Include(a => a.Product);
            return View(await aSMContext.ToListAsync());
        }

        // GET: AuthorProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AuthorProduct == null)
            {
                return NotFound();
            }

            var authorProduct = await _context.AuthorProduct
                .Include(a => a.Author)
                .Include(a => a.Product)
                .FirstOrDefaultAsync(m => m.AuthorId == id);
            if (authorProduct == null)
            {
                return NotFound();
            }

            return View(authorProduct);
        }

        // GET: AuthorProducts/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Author, "AuthorId", "AuthorId");
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId");
            return View();
        }

        // POST: AuthorProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorId,ProductId")] AuthorProduct authorProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(authorProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Author, "AuthorId", "AuthorId", authorProduct.AuthorId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", authorProduct.ProductId);
            return View(authorProduct);
        }

        // GET: AuthorProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AuthorProduct == null)
            {
                return NotFound();
            }

            var authorProduct = await _context.AuthorProduct.FindAsync(id);
            if (authorProduct == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Author, "AuthorId", "AuthorId", authorProduct.AuthorId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", authorProduct.ProductId);
            return View(authorProduct);
        }

        // POST: AuthorProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuthorId,ProductId")] AuthorProduct authorProduct)
        {
            if (id != authorProduct.AuthorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(authorProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorProductExists(authorProduct.AuthorId))
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
            ViewData["AuthorId"] = new SelectList(_context.Author, "AuthorId", "AuthorId", authorProduct.AuthorId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductId", authorProduct.ProductId);
            return View(authorProduct);
        }

        // GET: AuthorProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AuthorProduct == null)
            {
                return NotFound();
            }

            var authorProduct = await _context.AuthorProduct
                .Include(a => a.Author)
                .Include(a => a.Product)
                .FirstOrDefaultAsync(m => m.AuthorId == id);
            if (authorProduct == null)
            {
                return NotFound();
            }

            return View(authorProduct);
        }

        // POST: AuthorProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AuthorProduct == null)
            {
                return Problem("Entity set 'ASMContext.AuthorProduct'  is null.");
            }
            var authorProduct = await _context.AuthorProduct.FindAsync(id);
            if (authorProduct != null)
            {
                _context.AuthorProduct.Remove(authorProduct);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorProductExists(int id)
        {
          return (_context.AuthorProduct?.Any(e => e.AuthorId == id)).GetValueOrDefault();
        }
    }
}
