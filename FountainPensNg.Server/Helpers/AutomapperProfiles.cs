using AutoMapper;
using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;

namespace API;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<FountainPen, FountainPenDownloadDTO>();
        CreateMap<FountainPenDownloadDTO, FountainPen>();
        CreateMap<FountainPen, FountainPenUploadDTO>();
        CreateMap<FountainPenUploadDTO, FountainPen>();
        CreateMap<InkDTO, Ink>();
        CreateMap<Ink, InkDTO>()
            .ForMember(dest => dest.OneCurrentPenMaker,
                opt => opt.MapFrom(src =>
                    src.CurrentPens != null && src.CurrentPens.Any() ? src.CurrentPens.First().Maker : null)  
            )
            .ForMember(dest => dest.OneCurrentPenModelName,
                opt => opt.MapFrom(src => 
                    src.CurrentPens != null && src.CurrentPens.Any() ? src.CurrentPens.First().ModelName : null)
            );
        CreateMap<InkedUpDTO, InkedUp>();
        CreateMap<InkedUp, InkedUpDTO>()
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
