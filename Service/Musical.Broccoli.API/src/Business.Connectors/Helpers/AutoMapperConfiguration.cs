using System;
using AutoMapper;
using Common.DTOs;
using DataAccessLayer.Entities;

namespace Business.Connectors.Helpers
{
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

            CreateMap<Message, TourDTO>().MaxDepth(3);
            CreateMap<TourDTO, Message>().MaxDepth(3);

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