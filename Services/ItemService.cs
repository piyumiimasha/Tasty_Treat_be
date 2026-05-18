using AutoMapper;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Interfaces.Service;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IItemFlavourRepository _itemFlavourRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;

        public ItemService(
            IItemRepository itemRepository,
            IItemFlavourRepository itemFlavourRepository,
            IReviewRepository reviewRepository,
            IOrderItemRepository orderItemRepository,
            IMapper mapper)
        {
            _itemRepository = itemRepository;
            _itemFlavourRepository = itemFlavourRepository;
            _reviewRepository = reviewRepository;
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
        }

        public async Task<ItemDto?> GetByIdAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            return item != null ? _mapper.Map<ItemDto>(item) : null;
        }

        public async Task<IEnumerable<ItemDto>> GetAllAsync()
        {
            var items = await _itemRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ItemDto>>(items);
        }

        public async Task<IEnumerable<ItemDto>> GetByCategoryAsync(string category)
        {
            var items = await _itemRepository.GetByCategoryAsync(category);
            return _mapper.Map<IEnumerable<ItemDto>>(items);
        }

        public async Task<ItemDto> CreateAsync(CreateItemDto createItemDto)
        {
            var item = _mapper.Map<Item>(createItemDto);
            item.CategoryId = createItemDto.CategoryId;
            var createdItem = await _itemRepository.AddAsync(item);
            return _mapper.Map<ItemDto>(createdItem);
        }

        public async Task<ItemDto> UpdateAsync(int id, UpdateItemDto updateItemDto)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException($"Item with id {id} not found");

            if (!string.IsNullOrEmpty(updateItemDto.Name))
                item.Name = updateItemDto.Name;
            if (updateItemDto.CategoryId.HasValue)
                item.CategoryId = updateItemDto.CategoryId.Value;
            if (!string.IsNullOrWhiteSpace(updateItemDto.Flavour))
                item.Flavour = updateItemDto.Flavour;
            if (updateItemDto.BasePrice.HasValue)
                item.BasePrice = updateItemDto.BasePrice.Value;
            if (updateItemDto.BasePriceUnit != null)
                item.BasePriceUnit = updateItemDto.BasePriceUnit;
            if (updateItemDto.Description != null)
                item.Description = updateItemDto.Description;
            if (updateItemDto.Flavour != null)
                item.Flavour = updateItemDto.Flavour;

            var updatedItem = await _itemRepository.UpdateAsync(item);
            return _mapper.Map<ItemDto>(updatedItem);
        }

        public async Task<ItemDto> UpdateImageAsync(int id, string? imageUrl)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException($"Item with id {id} not found");

            item.ImageUrl = imageUrl;

            var updatedItem = await _itemRepository.UpdateAsync(item);
            return _mapper.Map<ItemDto>(updatedItem);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null) return false;

            // Remove related records that reference this item
            var flavours = await _itemFlavourRepository.GetByItemIdAsync(id);
            foreach (var f in flavours)
                await _itemFlavourRepository.DeleteAsync(f.ItemFlavourId);

            var reviews = await _reviewRepository.GetByItemIdAsync(id);
            foreach (var r in reviews)
                await _reviewRepository.DeleteAsync(r.ReviewId);

            var orderItems = await _orderItemRepository.FindAsync(oi => oi.ItemId == id);
            foreach (var oi in orderItems)
                await _orderItemRepository.DeleteAsync(oi.OrderItemId);

            return await _itemRepository.DeleteAsync(id);
        }
    }
}
