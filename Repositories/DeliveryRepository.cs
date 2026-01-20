using Microsoft.EntityFrameworkCore;
using Tasty_Treat_be.Data;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Repositories
{
    public class DeliveryRepository : Repository<Delivery>, IDeliveryRepository
    {
        public DeliveryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Delivery>> GetByDeliveryPersonIdAsync(int deliveryPersonId)
        {
            return await _dbSet.Where(d => d.DeliveryPersonId == deliveryPersonId).ToListAsync();
        }

        public async Task<Delivery?> GetByOrderIdAsync(int orderId)
        {
            return await _dbSet.FirstOrDefaultAsync(d => d.OrderId == orderId);
        }
    }
}
