using AutoMapper;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ChatMsg mappings
            CreateMap<ChatMsg, ChatMsgDto>();
            CreateMap<CreateChatMsgDto, ChatMsg>();
            CreateMap<UpdateChatMsgDto, ChatMsg>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Order mappings
            CreateMap<Order, OrderDto>();
            CreateMap<CreateOrderDto, Order>();
            CreateMap<UpdateOrderDto, Order>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Item mappings
            CreateMap<Item, ItemDto>();
            CreateMap<CreateItemDto, Item>();
            CreateMap<UpdateItemDto, Item>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // CustomizationOption mappings
            CreateMap<CustomizationOption, CustomizationOptionDto>();
            CreateMap<CreateCustomizationOptionDto, CustomizationOption>();
            CreateMap<UpdateCustomizationOptionDto, CustomizationOption>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // OrderItem mappings
            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<CreateOrderItemDto, OrderItem>();
            CreateMap<UpdateOrderItemDto, OrderItem>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Review mappings
            CreateMap<Review, ReviewDto>();
            CreateMap<CreateReviewDto, Review>();
            CreateMap<UpdateReviewDto, Review>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Delivery mappings
            CreateMap<Delivery, DeliveryDto>();
            CreateMap<CreateDeliveryDto, Delivery>();
            CreateMap<UpdateDeliveryDto, Delivery>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // InstantQuote mappings
            CreateMap<InstantQuote, InstantQuoteDto>();
            CreateMap<CreateInstantQuoteDto, InstantQuote>();
            CreateMap<UpdateInstantQuoteDto, InstantQuote>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Payment mappings
            CreateMap<Payment, PaymentDto>();
            CreateMap<CreatePaymentDto, Payment>();
            CreateMap<UpdatePaymentDto, Payment>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
