namespace FountainPensNg.Server.Helpers;
public static class MapsterConfig {
    public static void RegisterMapsterConfiguration(this IServiceCollection services) {

        var config = TypeAdapterConfig.GlobalSettings;

        //TODO: fountainPen mapping already broke silently. check if other mappings still work, if not remove ALL custom configs and map manually

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