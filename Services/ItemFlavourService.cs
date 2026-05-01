using AutoMapper;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Interfaces.Service;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Services
{
    public class ItemFlavourService : IItemFlavourService
    {
        private readonly IItemFlavourRepository _repo;
        private readonly IItemRepository _itemRepo;
        private readonly IMapper _mapper;

        public ItemFlavourService(IItemFlavourRepository repo, IItemRepository itemRepo, IMapper mapper)
        {
            _repo = repo;
            _itemRepo = itemRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ItemFlavourDto>> GetByItemIdAsync(int itemId)
        {
            var flavours = await _repo.GetByItemIdAsync(itemId);
            return _mapper.Map<IEnumerable<ItemFlavourDto>>(flavours);
        }

        public async Task<ItemFlavourDto?> GetByIdAsync(int id)
        {
            var flavour = await _repo.GetByIdAsync(id);
            return flavour != null ? _mapper.Map<ItemFlavourDto>(flavour) : null;
        }

        public async Task<ItemFlavourDto> CreateAsync(int itemId, CreateItemFlavourDto dto)
        {
            var item = await _itemRepo.GetByIdAsync(itemId);
            if (item == null)
                throw new KeyNotFoundException($"Item with id {itemId} not found");

            var flavour = new ItemFlavour
            {
                ItemId = itemId,
                Name = dto.Name,
                ExtraPrice = dto.ExtraPrice,
            };

            var created = await _repo.AddAsync(flavour);
            return _mapper.Map<ItemFlavourDto>(created);
        }

        public async Task<ItemFlavourDto> UpdateAsync(int id, UpdateItemFlavourDto dto)
        {
            var flavour = await _repo.GetByIdAsync(id);
            if (flavour == null)
                throw new KeyNotFoundException($"ItemFlavour with id {id} not found");

            if (dto.Name != null) flavour.Name = dto.Name;
            if (dto.ExtraPrice.HasValue) flavour.ExtraPrice = dto.ExtraPrice.Value;

            var updated = await _repo.UpdateAsync(flavour);
            return _mapper.Map<ItemFlavourDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
