using Tasty_Treat_be.DTOs;

namespace Tasty_Treat_be.Interfaces.Service
{
    public interface IReviewService
    {
        Task<ReviewDto?> GetByIdAsync(int id);
        Task<IEnumerable<ReviewDto>> GetAllAsync();
        Task<IEnumerable<ReviewDto>> GetByItemIdAsync(int itemId);
        Task<IEnumerable<ReviewDto>> GetByCustomerIdAsync(int customerId);
        Task<ReviewDto> CreateAsync(CreateReviewDto createReviewDto);
        Task<ReviewDto> UpdateAsync(int id, UpdateReviewDto updateReviewDto);
        Task<bool> DeleteAsync(int id);
    }
}
