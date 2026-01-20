using AutoMapper;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Interfaces.Service;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        public async Task<PaymentDto?> GetByIdAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            return payment != null ? _mapper.Map<PaymentDto>(payment) : null;
        }

        public async Task<IEnumerable<PaymentDto>> GetAllAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }

        public async Task<PaymentDto?> GetByOrderIdAsync(int orderId)
        {
            var payment = await _paymentRepository.GetByOrderIdAsync(orderId);
            return payment != null ? _mapper.Map<PaymentDto>(payment) : null;
        }

        public async Task<PaymentDto?> GetByTransactionIdAsync(string transactionId)
        {
            var payment = await _paymentRepository.GetByTransactionIdAsync(transactionId);
            return payment != null ? _mapper.Map<PaymentDto>(payment) : null;
        }

        public async Task<PaymentDto> CreateAsync(CreatePaymentDto createPaymentDto)
        {
            var payment = _mapper.Map<Payment>(createPaymentDto);
            var createdPayment = await _paymentRepository.AddAsync(payment);
            return _mapper.Map<PaymentDto>(createdPayment);
        }

        public async Task<PaymentDto> UpdateAsync(int id, UpdatePaymentDto updatePaymentDto)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null)
                throw new KeyNotFoundException($"Payment with id {id} not found");

            if (!string.IsNullOrEmpty(updatePaymentDto.TransactionId))
                payment.TransactionId = updatePaymentDto.TransactionId;
            if (!string.IsNullOrEmpty(updatePaymentDto.PaymentMethod))
                payment.PaymentMethod = updatePaymentDto.PaymentMethod;
            if (updatePaymentDto.Amount.HasValue)
                payment.Amount = updatePaymentDto.Amount.Value;
            if (!string.IsNullOrEmpty(updatePaymentDto.PaymentStatus))
                payment.PaymentStatus = updatePaymentDto.PaymentStatus;

            var updatedPayment = await _paymentRepository.UpdateAsync(payment);
            return _mapper.Map<PaymentDto>(updatedPayment);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _paymentRepository.DeleteAsync(id);
        }
    }
}
