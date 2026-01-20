using AutoMapper;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Interfaces.Service;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public async Task<ReviewDto?> GetByIdAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            return review != null ? _mapper.Map<ReviewDto>(review) : null;
        }

        public async Task<IEnumerable<ReviewDto>> GetAllAsync()
        {
            var reviews = await _reviewRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task<IEnumerable<ReviewDto>> GetByItemIdAsync(int itemId)
        {
            var reviews = await _reviewRepository.GetByItemIdAsync(itemId);
            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task<IEnumerable<ReviewDto>> GetByCustomerIdAsync(int customerId)
        {
            var reviews = await _reviewRepository.GetByCustomerIdAsync(customerId);
            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task<ReviewDto> CreateAsync(CreateReviewDto createReviewDto)
        {
            var review = _mapper.Map<Review>(createReviewDto);
            var createdReview = await _reviewRepository.AddAsync(review);
            return _mapper.Map<ReviewDto>(createdReview);
        }

        public async Task<ReviewDto> UpdateAsync(int id, UpdateReviewDto updateReviewDto)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
                throw new KeyNotFoundException($"Review with id {id} not found");

            if (updateReviewDto.Comment != null)
                review.Comment = updateReviewDto.Comment;
            if (updateReviewDto.Rating.HasValue)
                review.Rating = updateReviewDto.Rating.Value;

            var updatedReview = await _reviewRepository.UpdateAsync(review);
            return _mapper.Map<ReviewDto>(updatedReview);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _reviewRepository.DeleteAsync(id);
        }
    }
}
