namespace FountainPensNg.Server.Data.Repos;

public static class SuggestionStrategies {
	public record Candidate(InkedUpSuggestion Suggestion, int PairUses,
		int PenRating = 0, int InkRating = 0, DateTime? PenLastUsed = null,
		double? PairMatchRating = null);

	public static IReadOnlyList<InkedUpSuggestion> Select(IEnumerable<Candidate> candidates, int count) {
		var shuffled = candidates.OrderBy(_ => Random.Shared.NextDouble()).ToList();
		var strategies = new[] {
			(Name: "favorites", Queue: new Queue<Candidate>(shuffled
				.Where(c => c.PairUses > 0).OrderByDescending(c => c.PairUses))),
			(Name: "least-used", Queue: new Queue<Candidate>(shuffled
				.OrderBy(c => c.Suggestion.InkLastInkedAt ?? DateTime.MinValue))),
			(Name: "new-combinations", Queue: new Queue<Candidate>(shuffled
				.Where(c => c.PairUses == 0 && c.PenRating > 0 && c.InkRating > 0)
				.OrderByDescending(c => Math.Min(c.PenRating, c.InkRating))
				.ThenByDescending(c => (long)c.PenRating + c.InkRating))),
			(Name: "neglected-pens", Queue: new Queue<Candidate>(shuffled
				.OrderBy(c => c.PenLastUsed ?? DateTime.MinValue)
				.ThenBy(c => c.Suggestion.FountainPenId)
				.ThenByDescending(c => c.PairMatchRating)
				.ThenBy(c => c.Suggestion.InkLastInkedAt ?? DateTime.MinValue)))
		};
		List<InkedUpSuggestion> results = [];
		HashSet<int> pens = [];
		HashSet<int> inks = [];
		// Take turns so a strategy with many candidates cannot crowd out the others.
		while (results.Count < count) {
			var added = false;
			foreach (var strategy in strategies) {
				while (strategy.Queue.TryDequeue(out var candidate)) {
					var suggestion = candidate.Suggestion;
					if (pens.Contains(suggestion.FountainPenId) || inks.Contains(suggestion.InkId)) continue;
					pens.Add(suggestion.FountainPenId);
					inks.Add(suggestion.InkId);
					results.Add(suggestion with { Strategy = strategy.Name });
					added = true;
					break;
				}
				if (results.Count >= count) break;
			}
			if (!added) break;
		}
		return results;
	}
}
