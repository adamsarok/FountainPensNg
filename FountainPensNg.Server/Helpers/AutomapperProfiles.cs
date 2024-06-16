using AutoMapper;
using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;

namespace API;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Ink, InkForListDTO>()
            .ForMember(dest => dest.OneCurrentPen,
                opt => opt.MapFrom(
                    src => src.CurrentPens.FirstOrDefault()
            ));
        CreateMap<InkedUp, InkedUpForListDTO>()
            .ForMember(dest => dest.PenMaker,
                opt => opt.MapFrom(
                    src => src.FountainPen.Maker
            ))
            .ForMember(dest => dest.PenName,
                opt => opt.MapFrom(
                    src => src.FountainPen.ModelName + " " + src.FountainPen.Nib
            ))
            .ForMember(dest => dest.InkMaker,
                opt => opt.MapFrom(
                    src => src.Ink.Maker
            ))
            .ForMember(dest => dest.InkName,
                opt => opt.MapFrom(
                    src => src.Ink.InkName
            ))
            .ForMember(dest => dest.InkColor,
                opt => opt.MapFrom(
                    src => src.Ink.Color
            ))            
            .ForMember(dest => dest.PenColor,
                opt => opt.MapFrom(
                    src => src.FountainPen.Color
            ));
    }
}
