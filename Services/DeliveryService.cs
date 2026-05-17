using AutoMapper;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Interfaces.Service;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public DeliveryService(
            IDeliveryRepository deliveryRepository,
            IOrderRepository orderRepository,
            IMapper mapper,
            INotificationService notificationService)
        {
            _deliveryRepository = deliveryRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<DeliveryDto?> GetByIdAsync(int id)
        {
            var delivery = await _deliveryRepository.GetByIdAsync(id);
            return delivery != null ? _mapper.Map<DeliveryDto>(delivery) : null;
        }

        public async Task<IEnumerable<DeliveryDto>> GetAllAsync()
        {
            var deliveries = await _deliveryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryDto>>(deliveries);
        }

        public async Task<IEnumerable<DeliveryDto>> GetByDeliveryPersonIdAsync(int deliveryPersonId)
        {
            var deliveries = await _deliveryRepository.GetByDeliveryPersonIdAsync(deliveryPersonId);
            return _mapper.Map<IEnumerable<DeliveryDto>>(deliveries);
        }

        public async Task<DeliveryDto?> GetByOrderIdAsync(int orderId)
        {
            var delivery = await _deliveryRepository.GetByOrderIdAsync(orderId);
            return delivery != null ? _mapper.Map<DeliveryDto>(delivery) : null;
        }

        public async Task<DeliveryDto> CreateAsync(CreateDeliveryDto createDeliveryDto)
        {
            var delivery = _mapper.Map<Delivery>(createDeliveryDto);
            delivery.AssignedAt = DateTime.UtcNow;
            var createdDelivery = await _deliveryRepository.AddAsync(delivery);

            await _notificationService.NotifyUserAsync(
                createdDelivery.DeliveryPersonId,
                "DeliveryUpdate",
                $"You have been assigned delivery for Order #{createdDelivery.OrderId}",
                createdDelivery.OrderId);

            return _mapper.Map<DeliveryDto>(createdDelivery);
        }

        public async Task<DeliveryDto> UpdateAsync(int id, UpdateDeliveryDto updateDeliveryDto)
        {
            var delivery = await _deliveryRepository.GetByIdAsync(id);
            if (delivery == null)
                throw new KeyNotFoundException($"Delivery with id {id} not found");

            var oldStatus = delivery.DeliveryStatus;

            if (updateDeliveryDto.DeliveryPersonId.HasValue)
                delivery.DeliveryPersonId = updateDeliveryDto.DeliveryPersonId.Value;
            if (!string.IsNullOrEmpty(updateDeliveryDto.DeliveryStatus))
                delivery.DeliveryStatus = updateDeliveryDto.DeliveryStatus;
            if (updateDeliveryDto.DeliveryNotes != null)
                delivery.DeliveryNotes = updateDeliveryDto.DeliveryNotes;
            if (updateDeliveryDto.AssignedAt.HasValue)
                delivery.AssignedAt = updateDeliveryDto.AssignedAt.Value;
            if (updateDeliveryDto.PickedUpAt.HasValue)
                delivery.PickedUpAt = updateDeliveryDto.PickedUpAt.Value;
            if (updateDeliveryDto.DeliveredAt.HasValue)
                delivery.DeliveredAt = updateDeliveryDto.DeliveredAt.Value;

            var updatedDelivery = await _deliveryRepository.UpdateAsync(delivery);

            if (!string.IsNullOrEmpty(updateDeliveryDto.DeliveryStatus) && updateDeliveryDto.DeliveryStatus != oldStatus)
            {
                await _notificationService.NotifyUserAsync(
                    updatedDelivery.DeliveryPersonId,
                    "DeliveryUpdate",
                    $"Delivery status for Order #{updatedDelivery.OrderId} updated to: {updatedDelivery.DeliveryStatus}",
                    updatedDelivery.OrderId);

                if (updateDeliveryDto.DeliveryStatus == "Delivered")
                {
                    var order = await _orderRepository.GetByIdAsync(updatedDelivery.OrderId);
                    if (order != null)
                    {
                        await _notificationService.NotifyUserAsync(
                            order.CustomerId,
                            "DeliveryUpdate",
                            $"Your order #{order.OrderId} has been delivered!",
                            order.OrderId);
                    }
                }
            }

            return _mapper.Map<DeliveryDto>(updatedDelivery);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _deliveryRepository.DeleteAsync(id);
        }
    }
}
