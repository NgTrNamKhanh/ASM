using ASM.Models;

namespace ASM.Repository
{
    public interface IUserOrderRepository
    {
        Task<IEnumerable<Order>> UserOrders();
    }
}