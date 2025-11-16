namespace FountainPensNg.Server.Data.Repos;
public class RandomsRepo(FountainPensContext context) {
    public async Task<IEnumerable<InkedUpSuggestion>> Get(int count) {
		// Find not inked pens
        var pens = await context
            .FountainPens
            .Where(p => !p.InkedUps.Where(x => x.IsCurrent).Any())
            .OrderBy(r => EF.Functions.Random())
            .Take(count)
            .ToListAsync();
		// Find unused inks 
        var inks = await context
            .Inks
            .Where(i => !i.InkedUps.Where(x => x.IsCurrent).Any())
			.OrderBy(r => EF.Functions.Random())
			.Take(count)
            .ToListAsync();
		// Return x combinations
        List<InkedUpSuggestion> results = new();
		for (int i = 0; i < pens.Count; i++) {
            if (inks.Count > i) {
                var ink = inks[i];
                var pen = pens[i];
				results.Add(new InkedUpSuggestion(
                    FountainPenId: pen.Id,
                    PenMaker: pen.Maker,
                    PenName: pen.ModelName,
                    InkId: ink.Id,
                    InkMaker: ink.Maker,
                    InkName: ink.InkName,
                    PenColor: pen.Color,
                    InkColor: ink.Color
                ));
            }
        };
        return results;
	}
}
