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
            CreateMap<AddressDTO, Address>().ForMember(x => x.Id, y => y.Ignore());

            #endregion

            #region Checkpoint

            CreateMap<CheckPoint, CheckPointDTO>();
            CreateMap<CheckPointDTO, CheckPoint>().ForMember(x => x.Id, y => y.Ignore());

            #endregion

            #region Message

            CreateMap<Message, TourDTO>();
            CreateMap<TourDTO, Message>().ForMember(x => x.Id, y => y.Ignore());

            #endregion

            #region Movement

            CreateMap<Movement, MovementDTO>();
            CreateMap<MovementDTO, Movement>().ForMember(x => x.Id, y => y.Ignore());

            #endregion

            #region Order

            CreateMap<Order, OrderDTO>();
            CreateMap<OrderDTO, Order>().ForMember(x => x.Id, y => y.Ignore());

            #endregion

            #region PaymentInfo

            CreateMap<PaymentInfo, PaymentInfoDTO>();
            CreateMap<PaymentInfoDTO, PaymentInfo>().ForMember(x => x.Id, y => y.Ignore());

            #endregion

            #region Promotion

            CreateMap<Promotion, PromotionDTO>();
            CreateMap<PromotionDTO, Promotion>().ForMember(x => x.Id, y => y.Ignore());

            #endregion

            #region Rating

            CreateMap<Rating, RatingDTO>();
            CreateMap<RatingDTO, Rating>().ForMember(x => x.Id, y => y.Ignore());

            #endregion

            #region Reservation

            CreateMap<Reservation, ReservationDTO>();
            CreateMap<ReservationDTO, Reservation>().ForMember(x => x.Id, y => y.Ignore());

            #endregion

            #region Tour

            CreateMap<Tour, TourDTO>();
            CreateMap<TourDTO, Tour>().ForMember(x => x.Id, y => y.Ignore());

            #endregion

            #region User

            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

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