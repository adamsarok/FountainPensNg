namespace FountainPensNg.Server.Data.Repos;

public class RandomsRepo(FountainPensContext context) {
	public async Task<IEnumerable<InkedUpSuggestion>> Get(int count) {
		if (count <= 0) return [];
		var pens = await context.FountainPens.AsNoTracking()
			.Where(p => !p.InkedUps.Any(x => x.IsCurrent)).ToListAsync();
		var inks = await context.Inks.AsNoTracking()
			.Where(i => i.Ml > 0 && !i.InkedUps!.Any(x => x.IsCurrent))
			.Select(i => new {
				Ink = i,
				LastUsed = i.InkedUps!.Max(x => (DateTime?)x.InkedAt)
			}).ToListAsync();
		var history = await context.InkedUps.AsNoTracking()
			.GroupBy(x => new { x.FountainPenId, x.InkId })
			.Select(g => new { g.Key.FountainPenId, g.Key.InkId, Count = g.Count(),
                LastUsed = g.Max(x => x.InkedAt),
                MatchRating = g.Where(x => x.MatchRating > 0).Average(x => (double?)x.MatchRating) })
			.ToListAsync();
		var frequencies = history.ToDictionary(x => (x.FountainPenId, x.InkId), x => x);
		var penLastUsed = history.GroupBy(x => x.FountainPenId)
            .ToDictionary(g => g.Key, g => (DateTime?)g.Max(x => x.LastUsed));
        var candidates = (from pen in pens
			from ink in inks
			select new SuggestionStrategies.Candidate(
				new InkedUpSuggestion(pen.Id, pen.Maker, pen.ModelName,
					ink.Ink.Id, ink.Ink.Maker, ink.Ink.InkName, pen.Color,
					ink.Ink.Color, pen.Nib, ink.LastUsed),
				frequencies.GetValueOrDefault((pen.Id, ink.Ink.Id))?.Count ?? 0,
                pen.Rating, ink.Ink.Rating, penLastUsed.GetValueOrDefault(pen.Id),
                frequencies.GetValueOrDefault((pen.Id, ink.Ink.Id))?.MatchRating)).ToList();
		return SuggestionStrategies.Select(candidates, count);
	}

}
