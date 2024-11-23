using AutoMapper;
using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;
using static FountainPensNg.Server.Data.DTO.SearchResultDTO;

namespace API;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Paper, SearchResultDTO>()
              .ForMember(dest => dest.SearchResultType, opt => opt.MapFrom(o => SearchResultTypes.Paper.ToString()))
              .ForMember(dest => dest.Model, opt => opt.MapFrom(o => o.PaperName));
        CreateMap<FountainPen, SearchResultDTO>()
              .ForMember(dest => dest.SearchResultType, opt => opt.MapFrom(o => SearchResultTypes.Pen.ToString()))
              .ForMember(dest => dest.Model, opt => opt.MapFrom(o => o.ModelName));
        CreateMap<Ink, SearchResultDTO>()
             .ForMember(dest => dest.SearchResultType, opt => opt.MapFrom(o => SearchResultTypes.Ink.ToString()))
             .ForMember(dest => dest.Model, opt => opt.MapFrom(o => o.InkName));

        CreateMap<Paper, PaperDTO>();
        CreateMap<PaperDTO, Paper>();
        CreateMap<FountainPen, FountainPenDownloadDTO>();
        CreateMap<FountainPenDownloadDTO, FountainPen>();
        CreateMap<FountainPen, FountainPenUploadDTO>();
        CreateMap<FountainPenUploadDTO, FountainPen>();
        CreateMap<InkDTO, Ink>();
        CreateMap<Ink, InkDTO>() //TODO: this is handled also in one of the controller as projection this is duplicate but needed - refactor
            .ForMember(dest => dest.OneCurrentPenMaker,
                opt => opt.MapFrom(src =>
                    src.InkedUps != null && src.InkedUps.Any() ? 
                    src.InkedUps.First().FountainPen.Maker : null)  
            )
            .ForMember(dest => dest.OneCurrentPenModelName,
                opt => opt.MapFrom(src => 
                    src.InkedUps != null && src.InkedUps.Any() ? 
                    src.InkedUps.First().FountainPen.ModelName : null)
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
