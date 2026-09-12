using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Repos;

namespace FountainPensNg.xTests;

public class SuggestionStrategiesTests {
	private static SuggestionStrategies.Candidate Candidate(int pen, int ink, int uses = 0,
		DateTime? lastUsed = null, int penRating = 0, int inkRating = 0,
        DateTime? penLastUsed = null, double? matchRating = null) => new(
		new InkedUpSuggestion(pen, "", "", ink, "", "", "", "", "", lastUsed), uses, penRating, inkRating, penLastUsed, matchRating);

	[Fact]
	public void MixesStrategiesAndRanksEachByItsOwnCriteria() {
		var results = SuggestionStrategies.Select([
			Candidate(1, 1, uses: 10, lastUsed: new DateTime(2025, 1, 1)),
			Candidate(2, 2, uses: 2, lastUsed: new DateTime(2025, 1, 1)),
			Candidate(3, 3)
		], 3);
		Assert.Equal([1, 3, 2], results.Select(r => r.FountainPenId));
		Assert.Equal(["favorites", "least-used", "neglected-pens"], results.Select(r => r.Strategy));
	}

	[Fact]
	public void LeastUsedFallsBackWithoutHistoryAndPrioritizesNeverUsed() {
		var results = SuggestionStrategies.Select([
			Candidate(1, 1, lastUsed: new DateTime(2025, 1, 1), penLastUsed: new DateTime(2025, 1, 1)),
			Candidate(2, 2),
			Candidate(3, 3, lastUsed: new DateTime(2024, 1, 1), penLastUsed: new DateTime(2024, 1, 1))
		], 10);
		Assert.Equal([2, 3, 1], results.Select(r => r.InkId));
		Assert.Equal(["least-used", "neglected-pens", "least-used"], results.Select(r => r.Strategy));
	}

	[Fact]
	public void DoesNotRepeatPensOrInksAcrossStrategies() {
		var results = SuggestionStrategies.Select([
			Candidate(1, 1, uses: 10), Candidate(1, 2),
			Candidate(2, 1), Candidate(2, 2)
		], 20);
		Assert.Equal(2, results.Count);
		Assert.Equal(results.Count, results.Select(r => r.FountainPenId).Distinct().Count());
		Assert.Equal(results.Count, results.Select(r => r.InkId).Distinct().Count());
	}

	[Theory]
	[InlineData(0, 10)]
	[InlineData(10, 0)]
	[InlineData(-1, 10)]
	public void NewCombinationsExcludeUnratedItems(int penRating, int inkRating) {
		var results = SuggestionStrategies.Select([
			Candidate(1, 1),
			Candidate(2, 2, lastUsed: DateTime.Today, penRating: penRating, inkRating: inkRating)
		], 2);
		Assert.DoesNotContain(results, r => r.Strategy == "new-combinations");
	}

	[Fact]
	public void NewCombinationsPreferBothHighlyRatedAndExcludePreviouslyUsedPairs() {
		var results = SuggestionStrategies.Select([
			Candidate(1, 1, uses: 10),
			Candidate(2, 2),
			Candidate(3, 3, lastUsed: DateTime.Today, penRating: 8, inkRating: 9),
			Candidate(4, 4, lastUsed: DateTime.Today, penRating: 10, inkRating: 7),
			Candidate(5, 5, uses: 1, lastUsed: DateTime.Today, penRating: 10, inkRating: 10),
			Candidate(6, 6, lastUsed: DateTime.Today, penRating: 8, inkRating: 8)
		], 3);
		Assert.Equal("new-combinations", results[2].Strategy);
		Assert.Equal(3, results[2].FountainPenId);
	}

	[Fact]
	public void NeglectedPensPreferNeverUsedThenOldestPenAndBestRatedPairing() {
		var results = SuggestionStrategies.Select([
			Candidate(1, 1, uses: 10),
			Candidate(2, 2),
			Candidate(3, 3, lastUsed: DateTime.Today, penLastUsed: new DateTime(2020, 1, 1), matchRating: 8),
			Candidate(3, 4, lastUsed: DateTime.Today.AddDays(-1), penLastUsed: new DateTime(2020, 1, 1), matchRating: 5),
			Candidate(4, 5, lastUsed: DateTime.Today, penLastUsed: new DateTime(2024, 1, 1), matchRating: 10),
			Candidate(5, 6, lastUsed: DateTime.Today)
		], 3);
		Assert.Equal("neglected-pens", results[2].Strategy);
		Assert.Equal(5, results[2].FountainPenId);

		var withoutNeverUsed = SuggestionStrategies.Select([
			Candidate(1, 1, uses: 10), Candidate(2, 2),
			Candidate(3, 3, lastUsed: DateTime.Today, penLastUsed: new DateTime(2020, 1, 1), matchRating: 8),
			Candidate(3, 4, lastUsed: DateTime.Today.AddDays(-1), penLastUsed: new DateTime(2020, 1, 1), matchRating: 5),
			Candidate(4, 5, lastUsed: DateTime.Today, penLastUsed: new DateTime(2024, 1, 1), matchRating: 10)
		], 3);
		Assert.Equal(3, withoutNeverUsed[2].FountainPenId);
		Assert.Equal(3, withoutNeverUsed[2].InkId);
	}

	[Theory]
	[InlineData(0)]
	[InlineData(-1)]
	public void NonPositiveCountReturnsNoSuggestions(int count) {
		Assert.Empty(SuggestionStrategies.Select([Candidate(1, 1)], count));
	}

	[Fact]
	public void EmptyCollectionReturnsNoSuggestions() {
		Assert.Empty(SuggestionStrategies.Select([], 10));
	}
}
