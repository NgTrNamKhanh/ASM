using ASM.Models;
using Microsoft.EntityFrameworkCore;

namespace ASM.Data.Services
{
    public class AuthorProductsService : IAuthorProductsService
    {
        private readonly ASMContext _context;
        public AuthorProductsService(ASMContext context) 
        {  
            _context = context; 
        }
        public async Task AddAsycn(AuthorProduct authorProduct)
        {
            await _context.AuthorProduct.Add(authorProduct);
            await _context.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AuthorProduct> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<AuthorProduct?> GetById(int productId)
        {
            var result = await _context.AuthorProduct.FirstOrDefaultAsync(n => n.ProductId == productId);
            return result;
        }

        public async Task UpdateAsync(int productId, int authorId)
        {
            throw new NotImplementedException();
        }
    }
}
