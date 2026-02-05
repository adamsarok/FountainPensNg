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

		// Find unused inks, prioritizing those never used or used a long time ago
		var inks = await context
			.Inks
			.Where(i => !i.InkedUps.Where(x => x.IsCurrent).Any())
			.Select(i => new {
				Ink = i,
				LastUsed = i.InkedUps.Any() ? i.InkedUps.Max(iu => iu.InkedAt) : (DateTime?)null,
				RandomValue = EF.Functions.Random()
			})
			.ToListAsync();

		// Sort with priority: never used first, then by oldest use date, with random tie-breaking
		var sortedInks = inks
			.OrderBy(i => i.LastUsed.HasValue ? 0 : -1) // Never used inks first (-1)
			.ThenBy(i => i.LastUsed ?? DateTime.MinValue) // Then by oldest use date
			.ThenBy(i => i.RandomValue) // Add randomness for tie-breaking
			.Take(count)
			.Select(i => i.Ink)
			.ToList();

		// Return x combinations
		List<InkedUpSuggestion> results = new();
		for (int i = 0; i < pens.Count; i++) {
			if (sortedInks.Count > i) {
				var ink = sortedInks[i];
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
		}

		return results;
	}
}
