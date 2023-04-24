using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM.Data;
using ASM.Models;
using ASM.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace ASM.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class ProductsController : Controller
    {
        private readonly ASMContext _context;
        private static List<Product> TempSearch;

        public ProductsController(ASMContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Staff")]
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

			if (TempSearch != null)
			{
				var searchResults = TempSearch;
				ViewBag.Result = "Found " + searchResults.Count + " book";
				return View(searchResults);
			}
			else
			{
				return View(products);
			}
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
        [Authorize(Roles = "Staff")]

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
        [Authorize(Roles = "Staff")]

        public IActionResult Create()
        {
            var viewModel = new Product
            {
                Categories = _context.Category.ToList(),
                Authors = _context.Author.ToList()
            };
            return View(viewModel);
        }
        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Staff")]

        [HttpPost]
        //[Route("Create", Name ="CreateProudct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product viewModel)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    ProductName = viewModel.ProductName,
                    ProductDescription = viewModel.ProductDescription,
                    ProductPrice = viewModel.ProductPrice,
                    ProductStock = viewModel.ProductStock,
                    ProductImage = viewModel.ProductImage
                };

                _context.Add(product);
                await _context.SaveChangesAsync();

                foreach (var categoryId in viewModel.SelectedCategoryIds)
                {
                    var categoryProduct = new CategoryProduct
                    {
                        CategoryId = categoryId,
                        ProductId = product.ProductId
                    };

                    _context.Add(categoryProduct);
                }

                foreach (var authorId in viewModel.SelectedAuthorIds)
                {
                    var authorProduct = new AuthorProduct
                    {
                        AuthorId = authorId,
                        ProductId = product.ProductId
                    };

                    _context.Add(authorProduct);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            viewModel.Categories = _context.Category.ToList();
            viewModel.Authors = _context.Author.ToList();

            
            return View(viewModel);
        }

        // GET: Products/Edit/5
        //[HttpGet]
        //[Route("Edit", Name ="EditForm")]
        [Authorize(Roles = "Staff")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
            .Include(p => p.AuthorProducts)
                .ThenInclude(ap => ap.Author)
            .Include(p => p.CategoryProducts)
                .ThenInclude(cp => cp.Category)
            .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            // Create a list of all available authors and categories for the view to display
            var allAuthors = await _context.Author.ToListAsync();
            var allCategories = await _context.Category.ToListAsync();
            product.Authors = allAuthors;
            product.Categories = allCategories;

            // Create a list of selected author and category IDs for the view to display
            var selectedAuthorIds = product.AuthorProducts.Select(ap => ap.AuthorId).ToList();
            var selectedCategoryIds = product.CategoryProducts.Select(cp => cp.CategoryId).ToList();
            product.SelectedAuthorIds = selectedAuthorIds;
            product.SelectedCategoryIds = selectedCategoryIds;
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Staff")]

        [HttpPost]
        //[Route("Edit", Name ="EditProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductDescription,ProductPrice,ProductStock,ProductImage")] Product product, List<int> SelectedAuthorIds, List<int> SelectedCategoryIds)
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

                    // Update the authors of the product
                    var existingAuthorIds = _context.AuthorProduct.Where(ap => ap.ProductId == id).Select(ap => ap.AuthorId).ToList();
                    var newAuthorIds = SelectedAuthorIds.Except(existingAuthorIds);
                    var deletedAuthorIds = existingAuthorIds.Except(SelectedAuthorIds);

                    foreach (var authorId in newAuthorIds)
                    {
                        _context.Add(new AuthorProduct { ProductId = id, AuthorId = authorId });
                    }

                    foreach (var authorId in deletedAuthorIds)
                    {
                        var authorProduct = await _context.AuthorProduct.FirstOrDefaultAsync(ap => ap.ProductId == id && ap.AuthorId == authorId);
                        _context.AuthorProduct.Remove(authorProduct);
                    }

                    // Update the categories of the product
                    var existingCategoryIds = _context.CategoryProduct.Where(cp => cp.ProductId == id).Select(cp => cp.CategoryId).ToList();
                    var newCategoryIds = SelectedCategoryIds.Except(existingCategoryIds);
                    var deletedCategoryIds = existingCategoryIds.Except(SelectedCategoryIds);

                    foreach (var categoryId in newCategoryIds)
                    {
                        _context.Add(new CategoryProduct { ProductId = id, CategoryId = categoryId });
                    }

                    foreach (var categoryId in deletedCategoryIds)
                    {
                        var categoryProduct = await _context.CategoryProduct.FirstOrDefaultAsync(cp => cp.ProductId == id && cp.CategoryId == categoryId);
                        _context.CategoryProduct.Remove(categoryProduct);
                    }

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
            // Create a list of all available authors and categories for the view to display
            var allAuthors = await _context.Author.ToListAsync();
            var allCategories = await _context.Category.ToListAsync();
            product.Authors = allAuthors;
            product.Categories = allCategories;

            // Create a list of selected author and category IDs for the view to display
            product.SelectedAuthorIds = SelectedAuthorIds;
            product.SelectedCategoryIds = SelectedCategoryIds;

            return View(product);
        }

        // GET: Products/Delete/
        //[HttpGet]
        //[HttpDelete("{id}", Name ="DeleteProductById")]
        [Authorize(Roles = "Staff")]

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
        [Authorize(Roles = "Staff")]

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

        [HttpPost]
        [ActionName("SearchProduct")]
        public IActionResult SearchProduct(string searchString)
        { 
			var products = _context.Product.Where(p => p.ProductName.ToLower()
                .Contains(searchString.ToLower()))
                .Include(p => p.AuthorProducts)
			   .ThenInclude(ap => ap.Author).ToList();
			TempSearch= products;
			return RedirectToAction("List", "Products");
        }
        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
