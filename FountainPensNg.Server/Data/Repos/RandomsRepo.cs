namespace FountainPensNg.Server.Data.Repos;
public class RandomsRepo(FountainPensContext context) {
    public async Task<IEnumerable<InkedUpSuggestion>> Get(int count) {
		// Find not inked pens
        var pens = await context
            .FountainPens
            .Where(p => !p.InkedUps.Where(x => x.IsCurrent).Any())
            .OrderBy(r => EF.Functions.Random())
            .Take(count)
            .ProjectToType<InkedUpDTO>()
            .ToListAsync();
		// Find unused inks 
        var inks = await context
            .Inks
            .Where(i => !i.InkedUps.Where(x => x.IsCurrent).Any())
			.OrderBy(r => EF.Functions.Random())
			.Take(count)
            .ToListAsync();
		// Return x combinations
        return pens.Select((pen, index) => {
            var ink = inks[index];
            return new InkedUpSuggestion(
                FountainPenId: pen.Id,
                PenMaker: pen.PenMaker,
                PenName: pen.PenName,
                InkId: ink.Id,
                InkMaker: ink.Maker,
                InkName: ink.InkName,
                PenColor: pen.PenColor,
                InkColor: ink.Color
            );
        });
	}
}
