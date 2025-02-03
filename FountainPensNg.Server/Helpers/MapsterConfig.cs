using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;
using Mapster;
using static FountainPensNg.Server.Data.DTO.SearchResultDTO;

namespace FountainPensNg.Server.Helpers {
	public static class MapsterConfig {
        public static void RegisterMapsterConfiguration(this IServiceCollection services) {

            var config = TypeAdapterConfig.GlobalSettings;

            //TODO: this works but if used with ProjectTo generates an absolutely bonkers query with 40+ joins/subqueries
            config.NewConfig<FountainPen, FountainPenDownloadDTO>()
                .Map(dest => dest.CurrentInk,
                     src => src.InkedUps
                               .Where(x => x.IsCurrent)
                               .Select(x => x.Ink)
                               .FirstOrDefault())
                .Map(dest => dest.CurrentInkId,
                     src => src.InkedUps
                               .Where(x => x.IsCurrent)
                               .Select(x => x.Ink.Id)
                               .FirstOrDefault())
                .Map(dest => dest.CurrentInkRating,
                     src => src.InkedUps
                               .Where(x => x.IsCurrent)
                               .Select(x => x.MatchRating)
                               .FirstOrDefault());


            config.NewConfig<InkedUp, InkedUpDTO>()
                .Map(dest => dest.PenMaker,
                    src => src.FountainPen.Maker)
                .Map(dest => dest.PenName,
                        src => src.FountainPen.ModelName + " " + src.FountainPen.Nib)
                .Map(dest => dest.InkMaker,
                        src => src.Ink.Maker)
                .Map(dest => dest.InkName,
                        src => src.Ink.InkName)
                .Map(dest => dest.InkColor,
                        src => src.Ink.Color)
                .Map(dest => dest.PenColor,
                        src => src.FountainPen.Color);

   
            config.NewConfig<Ink, InkDownloadDTO>() //TODO: this is handled also in one of the controller as projection this is duplicate but needed - refactor
                .Map(dest => dest.OneCurrentPenMaker,
                    src =>
                        src.InkedUps != null && src.InkedUps.Any() ?
                        src.InkedUps.First().FountainPen.Maker : null)
                .Map(dest => dest.OneCurrentPenModelName,
                    src =>
                        src.InkedUps != null && src.InkedUps.Any() ?
                        src.InkedUps.First().FountainPen.ModelName : null);

            config.NewConfig<Paper, SearchResultDTO>()
                .Map(dest => dest.SearchResultType, src => SearchResultTypes.Paper.ToString())
                .Map(dest => dest.Model, src => src.PaperName);
            config.NewConfig<FountainPen, SearchResultDTO>()
                .Map(dest => dest.SearchResultType, src => SearchResultTypes.Pen.ToString())
                .Map(dest => dest.Model, src => src.ModelName);
            config.NewConfig<Ink, SearchResultDTO>()
                .Map(dest => dest.SearchResultType, src => SearchResultTypes.Ink.ToString())
                .Map(dest => dest.Model, src => src.InkName);
        }
    }
}
