using Tasty_Treat_be.DTOs;

namespace Tasty_Treat_be.Interfaces.Service
{
    public interface IPaymentService
    {
        Task<PaymentDto?> GetByIdAsync(int id);
        Task<IEnumerable<PaymentDto>> GetAllAsync();
        Task<PaymentDto?> GetByOrderIdAsync(int orderId);
        Task<PaymentDto?> GetByTransactionIdAsync(string transactionId);
        Task<PaymentDto> CreateAsync(CreatePaymentDto createPaymentDto);
        Task<PaymentDto> UpdateAsync(int id, UpdatePaymentDto updatePaymentDto);
        Task<bool> DeleteAsync(int id);
    }
}
