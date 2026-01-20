using Tasty_Treat_be.DTOs;

namespace Tasty_Treat_be.Interfaces.Service
{
    public interface IInstantQuoteService
    {
        Task<InstantQuoteDto?> GetByIdAsync(int id);
        Task<IEnumerable<InstantQuoteDto>> GetAllAsync();
        Task<IEnumerable<InstantQuoteDto>> GetByCustomerIdAsync(int customerId);
        Task<InstantQuoteDto> CreateAsync(CreateInstantQuoteDto createInstantQuoteDto);
        Task<InstantQuoteDto> UpdateAsync(int id, UpdateInstantQuoteDto updateInstantQuoteDto);
        Task<bool> DeleteAsync(int id);
    }
}
