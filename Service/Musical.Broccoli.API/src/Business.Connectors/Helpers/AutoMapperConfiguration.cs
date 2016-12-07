using System;
using System.Linq;
using AutoMapper;
using Common.DTOs;
using DataAccessLayer.Entities;
using System.Collections.Generic;

namespace Business.Connectors.Helpers
{
    /// <summary>
    /// Sets the different mappings that are used througout Automapper
    /// </summary>
    public class AutoMapperConfiguration : Profile
    {
        protected override void Configure()
        {
            #region Address

            CreateMap<Address, AddressDTO>();
            CreateMap<AddressDTO, Address>();

            #endregion

            #region Checkpoint

            CreateMap<CheckPoint, CheckPointDTO>();
            CreateMap<CheckPointDTO, CheckPoint>();

            #endregion

            #region Message

            CreateMap<Message, MessageDTO>().ForMember(dest => dest.Receivers,
               opts => opts.MapFrom(src => src.MessageHasRecievers.Select(x=>x.Reciever).ToList())).MaxDepth(3);
            CreateMap<MessageDTO, Message>().ForMember(dest => dest.MessageHasRecievers,
               opts => opts.MapFrom(src =>src.Receivers.Select(x=> new MessageHasReciever { MessageId= src.Id,RecieverId=x.Id}).ToList())).MaxDepth(3);

            #endregion

            #region Movement

            CreateMap<Movement, MovementDTO>().MaxDepth(3);
            CreateMap<MovementDTO, Movement>().MaxDepth(3);

            #endregion

            #region Order

            CreateMap<Order, OrderDTO>().MaxDepth(3);
            CreateMap<OrderDTO, Order>().MaxDepth(3);

            #endregion

            #region PaymentInfo

            CreateMap<PaymentInfo, PaymentInfoDTO>();
            CreateMap<PaymentInfoDTO, PaymentInfo>();

            #endregion

            #region Promotion

            CreateMap<Promotion, PromotionDTO>();
            CreateMap<PromotionDTO, Promotion>();

            #endregion

            #region Rating

            CreateMap<Rating, RatingDTO>();
            CreateMap<RatingDTO, Rating>();

            #endregion

            #region Reservation

            CreateMap<Reservation, ReservationDTO>().MaxDepth(3);
            CreateMap<ReservationDTO, Reservation>().MaxDepth(3);

            #endregion

            #region Tour

            CreateMap<Tour, TourDTO>().MaxDepth(3);
            CreateMap<TourDTO, Tour>().MaxDepth(3);

            #endregion

            #region User

            CreateMap<User, UserDTO>().MaxDepth(3);
            CreateMap<UserDTO, User>().MaxDepth(3);

            #endregion

            #region Func<t,bool>

            CreateMap<Func<AddressDTO, bool>, Func<Address, bool>>();

            #endregion

            #region Session

            CreateMap<Session, SessionDTO>()
                .ForMember(dest => dest.AuthToken, opt => opt.MapFrom(src => src.AuthToken.ToString()));
            CreateMap<SessionDTO, Session>()
                .ForMember(dest => dest.AuthToken,
                    opt => opt.MapFrom(src => Guid.Parse(src.AuthToken)));

            #endregion
        }
    }
}