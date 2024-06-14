using AutoMapper;
using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;

namespace API;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Ink, InkListDTO>()
            .ForMember(dest => dest.OneCurrentPen,
                opt => opt.MapFrom(
                    src => src.CurrentPens.FirstOrDefault()
            ));
           
    }
}
