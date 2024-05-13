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
                    .ForMember(dest => dest.ThuisploegLogoPath,
                    opts => opts.MapFrom(
                        src => src.Thuisploeg.LogoPath))
                    .ForMember(dest => dest.UitploegLogoPath,
                    opts => opts.MapFrom(
                        src => src.Uitploeg.LogoPath))
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

            CreateMap<Abonnement, HistoryAbonnementVM>()
                .ForMember(dest => dest.clubNaam,
                    opts => opts.MapFrom(
                        src => src.Club.Naam));

            CreateMap<CartTicketVM, Ticket>();
            CreateMap<Ticket, TicketVM>()
                .ForMember(dest => dest.Datum,
                    opts => opts.MapFrom(
                        src => src.Match.Datum))
                .ForMember(dest => dest.Startuur,
                    opts => opts.MapFrom(
                        src => src.Match.Startuur));

            CreateMap<CreateStoelVM, Stoeltje>();
            CreateMap<Stoeltje, CreateStoelVM>();

            //API
            CreateMap<Hotel, HotelVM>();
        }
    }
}
