using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Interfaces.Repository
{
    public interface IDeliveryRepository : IRepository<Delivery>
    {
        Task<IEnumerable<Delivery>> GetByDeliveryPersonIdAsync(int deliveryPersonId);
        Task<Delivery?> GetByOrderIdAsync(int orderId);
    }
}
