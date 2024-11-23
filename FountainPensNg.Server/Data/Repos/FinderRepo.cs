using AutoMapper;
using AutoMapper.QueryableExtensions;
using FountainPensNg.Server.Data.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FountainPensNg.Server.Data.Repos {
    public class FinderRepo {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FinderRepo(DataContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ActionResult<IEnumerable<SearchResultDTO>>> FindAll(string fulltext) {
            var pens = await _context
                .FountainPens
                .Where(p => string.IsNullOrWhiteSpace(fulltext)
                    || p.FullText.Matches(EF.Functions.ToTsQuery($"{fulltext}:*")))
                .ProjectTo<SearchResultDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
            var inks = await _context
                .Inks
                .Where(p => string.IsNullOrWhiteSpace(fulltext)
                    || p.FullText.Matches(EF.Functions.ToTsQuery($"{fulltext}:*")))
                .ProjectTo<SearchResultDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
            var papers = await _context
                .Papers
                .Where(p => string.IsNullOrWhiteSpace(fulltext)
                    || p.FullText.Matches(EF.Functions.ToTsQuery($"{fulltext}:*")))
                .ProjectTo<SearchResultDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return pens
                .Union(inks)
                .Union(papers)
                .ToList();
        }
    }
}
