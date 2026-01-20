using AutoMapper;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Interfaces.Service;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;

        public OrderItemService(IOrderItemRepository orderItemRepository, IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
        }

        public async Task<OrderItemDto?> GetByIdAsync(int id)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(id);
            return orderItem != null ? _mapper.Map<OrderItemDto>(orderItem) : null;
        }

        public async Task<IEnumerable<OrderItemDto>> GetAllAsync()
        {
            var orderItems = await _orderItemRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderItemDto>>(orderItems);
        }

        public async Task<IEnumerable<OrderItemDto>> GetByOrderIdAsync(int orderId)
        {
            var orderItems = await _orderItemRepository.GetByOrderIdAsync(orderId);
            return _mapper.Map<IEnumerable<OrderItemDto>>(orderItems);
        }

        public async Task<OrderItemDto> CreateAsync(CreateOrderItemDto createOrderItemDto)
        {
            var orderItem = _mapper.Map<OrderItem>(createOrderItemDto);
            var createdOrderItem = await _orderItemRepository.AddAsync(orderItem);
            return _mapper.Map<OrderItemDto>(createdOrderItem);
        }

        public async Task<OrderItemDto> UpdateAsync(int id, UpdateOrderItemDto updateOrderItemDto)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(id);
            if (orderItem == null)
                throw new KeyNotFoundException($"OrderItem with id {id} not found");

            if (updateOrderItemDto.Quantity.HasValue)
                orderItem.Quantity = updateOrderItemDto.Quantity.Value;
            if (updateOrderItemDto.OrderItemPrice.HasValue)
                orderItem.OrderItemPrice = updateOrderItemDto.OrderItemPrice.Value;
            if (updateOrderItemDto.IsAvailable.HasValue)
                orderItem.IsAvailable = updateOrderItemDto.IsAvailable.Value;

            var updatedOrderItem = await _orderItemRepository.UpdateAsync(orderItem);
            return _mapper.Map<OrderItemDto>(updatedOrderItem);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _orderItemRepository.DeleteAsync(id);
        }
    }
}
