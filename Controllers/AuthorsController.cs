using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM.Data;
using ASM.Models;
using Microsoft.AspNetCore.Authorization;

namespace ASM.Controllers
{
    
    public class AuthorsController : Controller
    {
        private readonly ASMContext _context;

        public AuthorsController(ASMContext context)
        {
            _context = context;
        }
		[Authorize(Roles = "Admin")]
		// GET: Authors
		public async Task<IActionResult> Index()
        {
              return _context.Author != null ? 
                          View(await _context.Author.ToListAsync()) :
                          Problem("Entity set 'ASMContext.Author'  is null.");
        }
		public async Task<IActionResult> FindAuthorById(int id)
		{
            var author = await _context.Author.FindAsync(id);
            ViewData["Name"] = author.AuthorName;
            ViewData["Description"] = author.AuthorDescription;
            var products = await _context.Product
                .Include(p => p.AuthorProducts)
                .ThenInclude(ap => ap.Author)
                .ToListAsync();
            return View(products);
		}
		[Authorize(Roles = "Admin")]
		// GET: Authors/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Author == null)
            {
                return NotFound();
            }

            var author = await _context.Author
                .FirstOrDefaultAsync(m => m.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }
		[Authorize(Roles = "Admin")]
		// GET: Authors/Create
		public IActionResult Create()
        {
            return View();
        }
		[Authorize(Roles = "Admin")]
		// POST: Authors/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorId,AuthorName,AuthorDescription,AuthorBirthYear")] Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Author == null)
            {
                return NotFound();
            }

            var author = await _context.Author.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }
		[Authorize(Roles = "Admin")]
		// POST: Authors/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuthorId,AuthorName,AuthorDescription,AuthorBirthYear")] Author author)
        {
            if (id != author.AuthorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.AuthorId))
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
            return View(author);
        }
		[Authorize(Roles = "Admin")]
		// GET: Authors/Delete/5
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Author == null)
            {
                return NotFound();
            }

            var author = await _context.Author
                .FirstOrDefaultAsync(m => m.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }
		[Authorize(Roles = "Admin")]
		// POST: Authors/Delete/5
		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Author == null)
            {
                return Problem("Entity set 'ASMContext.Author'  is null.");
            }
            var author = await _context.Author.FindAsync(id);
            if (author != null)
            {
                _context.Author.Remove(author);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
          return (_context.Author?.Any(e => e.AuthorId == id)).GetValueOrDefault();
        }
    }
}
