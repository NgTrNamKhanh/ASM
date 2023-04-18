using ASM.Models;

namespace ASM.Data.Services
{
    public interface IAuthorProductsService
    {
        //Task <IEnumerable<AuthorProduct>> GetAll();
        Task <AuthorProduct> GetById(int productId);
        Task AddAsycn(AuthorProduct authorProduct);
        Task <AuthorProduct> UpdateAsync(int productId, int authorId);
        void Delete(int id);
    }
}
