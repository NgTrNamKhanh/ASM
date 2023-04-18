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
    //[Route("api/[controller]")]
    //[ApiController]
    public class ProductsController : Controller
    {
        private readonly ASMContext _context;

        public ProductsController(ASMContext context)
        {
            _context = context;
        }
        [HttpGet]
        //[Route("Index", Name = "GetAllProductsStaff")]
        // GET: Products
        public async Task<IActionResult> Index()
        {
              return _context.Product != null ? 
                          View(await _context.Product.ToListAsync()) :
                          Problem("Entity set 'ASMContext.Product'  is null.");
        }
		//[HttpGet]
		//[Route("List", Name="GetAllProductsCustomer")]
		//GET: List Products 
		public async Task<IActionResult> List()
        {
			var products = await _context.Product
	       .Include(p => p.AuthorProducts)
	       .ThenInclude(ap => ap.Author)
	       .ToListAsync();

			return View(products);
		}
		//[HttpGet]
		//[Route("{id}", Name ="DetailProductCustomer")]
		//GET: Products/Detail/5
		public async Task<IActionResult> Detail(int? id)
		{
			if (id == null || _context.Product == null)
			{
				return NotFound();
			}

			var product = await _context.Product
				.Include(p => p.AuthorProducts)
			    .ThenInclude(ap => ap.Author)
		        .Include(p => p.CategoryProducts)
			    .ThenInclude(pc => pc.Category)
		        .FirstOrDefaultAsync(m => m.ProductId == id);
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}
		//[HttpGet]
		//[Route("{id}", Name = "DetailsProductStaff")]
		// GET: Products/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
				.Include(p => p.AuthorProducts)
				.ThenInclude(ap => ap.Author)
				.Include(p => p.CategoryProducts)
				.ThenInclude(pc => pc.Category)
				.FirstOrDefaultAsync(m => m.ProductId == id);
			if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
		//[HttpGet]
		//[Route("Create", Name = "CreateForm")]
		// GET: Products/Create
		public IActionResult Create()
        {
			ViewData["AuthorList"] = new SelectList(_context.Author, "AuthorId", "AuthorName");
			ViewData["CategoryList"] = new SelectList(_context.Category, "CategoryId", "CategoryName");
			return View();
        }
		// POST: Products/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        //[Route("Create", Name ="CreateProudct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductDescription,ProductPrice,ProductStock,ProductImage, AuthorProducts")] Product product, int[] selectedAuthorIds, int[] selectedCategoryIds)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                // Associate the selected authors with the new product
                if (selectedAuthorIds != null)
                {
                    foreach (int authorId in selectedAuthorIds)
                    {
                        var authorProduct = new AuthorProduct { AuthorId = authorId, ProductId = product.ProductId };
                        _context.AuthorProduct.Add(authorProduct);
                    }
                }

                // Associate the selected categories with the new product
                if (selectedCategoryIds != null)
                {
                    foreach (int categoryId in selectedCategoryIds)
                    {
                        var categoryProduct = new CategoryProduct { CategoryId = categoryId, ProductId = product.ProductId };
                        _context.CategoryProduct.Add(categoryProduct);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Authors = new SelectList(_context.Author, "AuthorId", "AuthorName");
            ViewBag.Categories = new SelectList(_context.Category, "CategoryId", "CategoryName");
            return View(product);
        }

        // GET: Products/Edit/5
        //[HttpGet]
        //[Route("Edit", Name ="EditForm")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Route("Edit", Name ="EditProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductDescription,ProductPrice,ProductStock,ProductImage")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            return View(product);
        }

        // GET: Products/Delete/
        //[HttpGet]
        //[HttpDelete("{id}", Name ="DeleteProductById")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'ASMContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
