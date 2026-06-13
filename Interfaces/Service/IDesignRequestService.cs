using Tasty_Treat_be.DTOs;

namespace Tasty_Treat_be.Interfaces.Service
{
    public interface IDesignRequestService
    {
        Task<IEnumerable<DesignRequestDto>> GetAllAsync();
        Task<DesignRequestDto?> GetByIdAsync(int id);
        Task<DesignRequestDto> CreateAsync(CreateDesignRequestDto dto);
        Task<DesignRequestDto> UpdateImageAsync(int id, string? imageUrl);
        Task<DesignRequestDto> UpdateStatusAsync(int id, string status, decimal? quotedPrice = null, string? adminMessage = null);
        Task<bool> DeleteAsync(int id);
    }
}
