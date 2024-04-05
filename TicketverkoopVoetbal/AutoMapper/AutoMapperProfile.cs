using AutoMapper;
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
                    ;

            CreateMap<Stadion, StadionVM>();

            CreateMap<Club, ClubVM>()
                .ForMember(dest => dest.StadionNaam,
                opts => opts.MapFrom(
                    src => src.Thuisstadion.Naam));
        }



    }
}
