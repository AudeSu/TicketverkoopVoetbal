using AutoMapper;
using TicketverkoopVoetbal.Domains;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.ViewModels;

namespace TicketverkoopVoetbal.AutoMapper
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<Match, MatchVM>()
                    .ForMember(dest => dest.StadionNaam,
                    opts => opts.MapFrom(
                        src => src.Stadion.Naam))
                    .ForMember(dest => dest.ThuisploegNaam,
                    opts => opts.MapFrom(
                        src => src.Thuisploeg.Naam))
                    .ForMember(dest => dest.UitploegNaam,
                    opts => opts.MapFrom(
                        src => src.Uitploeg.Naam))
                    .ForMember(dest => dest.StadionId,
                    opts => opts.MapFrom(
                        src => src.StadionId))
                    .ForMember(dest => dest.ClubId,
                    opts => opts.MapFrom(
                        src => src.ThuisploegId));


            CreateMap<Stadion, StadionVM>();

            CreateMap<Club, ClubVM>()
                .ForMember(dest => dest.StadionNaam,
                opts => opts.MapFrom(
                    src => src.Thuisstadion.Naam))
                .ForMember(dest => dest.StadionID,
                opts => opts.MapFrom(
                    src => src.Thuisstadion.StadionId));

            CreateMap<AspNetUser, AspNetUserVM>();

            CreateMap<CartTicketVM, Ticket>();

            CreateMap<CartAbonnementVM, Abonnement>();
            CreateMap<Abonnement, CartAbonnementVM>();

            CreateMap<CartTicketVM, Ticket>();
            CreateMap<Ticket, TicketVM>();

            CreateMap<CreateStoelVM, Stoeltje>();
            CreateMap<Stoeltje, CreateStoelVM>();

            //API
            CreateMap<Hotel, HotelVM>();




        }       
    }
}
