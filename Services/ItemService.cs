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
        private readonly IMapper _mapper;

        public ItemService(IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
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
            if (!string.IsNullOrEmpty(updateItemDto.Category))
                item.Category = updateItemDto.Category;
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
            return await _itemRepository.DeleteAsync(id);
        }
    }
}
