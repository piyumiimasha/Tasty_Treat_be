using Tasty_Treat_be.DTOs;

namespace Tasty_Treat_be.Interfaces.Service
{
    public interface IDeliveryService
    {
        Task<DeliveryDto?> GetByIdAsync(int id);
        Task<IEnumerable<DeliveryDto>> GetAllAsync();
        Task<IEnumerable<DeliveryDto>> GetByDeliveryPersonIdAsync(int deliveryPersonId);
        Task<DeliveryDto?> GetByOrderIdAsync(int orderId);
        Task<DeliveryDto> CreateAsync(CreateDeliveryDto createDeliveryDto);
        Task<DeliveryDto> UpdateAsync(int id, UpdateDeliveryDto updateDeliveryDto);
        Task<bool> DeleteAsync(int id);
    }
}
