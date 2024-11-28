using FountainPensNg.Server.Data.DTO;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FountainPensNg.Server.Data.Repos {
    public class FinderRepo {
        private readonly DataContext _context;

        public FinderRepo(DataContext context) {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<SearchResultDTO>>> FindAll(string fulltext) {
            var pens = await _context
                .FountainPens
                .Where(p => string.IsNullOrWhiteSpace(fulltext)
                    || p.FullText.Matches(EF.Functions.ToTsQuery($"{fulltext}:*")))
                .ProjectToType<SearchResultDTO>()
                .ToListAsync();
            var inks = await _context
                .Inks
                .Where(p => string.IsNullOrWhiteSpace(fulltext)
                    || p.FullText.Matches(EF.Functions.ToTsQuery($"{fulltext}:*")))
                .ProjectToType<SearchResultDTO>()
                .ToListAsync();
            var papers = await _context
                .Papers
                .Where(p => string.IsNullOrWhiteSpace(fulltext)
                    || p.FullText.Matches(EF.Functions.ToTsQuery($"{fulltext}:*")))
                .ProjectToType<SearchResultDTO>()
                .ToListAsync();
            return pens
                .Union(inks)
                .Union(papers)
                .ToList();
        }
    }
}
