using AutoMapper;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Interfaces.Service;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Services
{
    public class InstantQuoteService : IInstantQuoteService
    {
        private readonly IInstantQuoteRepository _instantQuoteRepository;
        private readonly IMapper _mapper;

        public InstantQuoteService(IInstantQuoteRepository instantQuoteRepository, IMapper mapper)
        {
            _instantQuoteRepository = instantQuoteRepository;
            _mapper = mapper;
        }

        public async Task<InstantQuoteDto?> GetByIdAsync(int id)
        {
            var quote = await _instantQuoteRepository.GetByIdAsync(id);
            return quote != null ? _mapper.Map<InstantQuoteDto>(quote) : null;
        }

        public async Task<IEnumerable<InstantQuoteDto>> GetAllAsync()
        {
            var quotes = await _instantQuoteRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<InstantQuoteDto>>(quotes);
        }

        public async Task<IEnumerable<InstantQuoteDto>> GetByCustomerIdAsync(int customerId)
        {
            var quotes = await _instantQuoteRepository.GetByCustomerIdAsync(customerId);
            return _mapper.Map<IEnumerable<InstantQuoteDto>>(quotes);
        }

        public async Task<InstantQuoteDto> CreateAsync(CreateInstantQuoteDto createInstantQuoteDto)
        {
            var quote = _mapper.Map<InstantQuote>(createInstantQuoteDto);
            var createdQuote = await _instantQuoteRepository.AddAsync(quote);
            return _mapper.Map<InstantQuoteDto>(createdQuote);
        }

        public async Task<InstantQuoteDto> UpdateAsync(int id, UpdateInstantQuoteDto updateInstantQuoteDto)
        {
            var quote = await _instantQuoteRepository.GetByIdAsync(id);
            if (quote == null)
                throw new KeyNotFoundException($"InstantQuote with id {id} not found");

            if (updateInstantQuoteDto.ConvertedOrderId.HasValue)
                quote.ConvertedOrderId = updateInstantQuoteDto.ConvertedOrderId.Value;
            if (!string.IsNullOrEmpty(updateInstantQuoteDto.Items))
                quote.Items = updateInstantQuoteDto.Items;
            if (updateInstantQuoteDto.Tax.HasValue)
                quote.Tax = updateInstantQuoteDto.Tax.Value;
            if (updateInstantQuoteDto.Discount.HasValue)
                quote.Discount = updateInstantQuoteDto.Discount.Value;
            if (updateInstantQuoteDto.DeliveryFee.HasValue)
                quote.DeliveryFee = updateInstantQuoteDto.DeliveryFee.Value;
            if (updateInstantQuoteDto.EstimatedPrice.HasValue)
                quote.EstimatedPrice = updateInstantQuoteDto.EstimatedPrice.Value;

            var updatedQuote = await _instantQuoteRepository.UpdateAsync(quote);
            return _mapper.Map<InstantQuoteDto>(updatedQuote);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _instantQuoteRepository.DeleteAsync(id);
        }
    }
}
