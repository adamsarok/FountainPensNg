using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static FountainPensNg.Server.Data.Repos.FountainPensRepo;
using static FountainPensNg.Server.Data.Repos.ResultType;

namespace FountainPensNg.Server.Data.Repos {
    public class InkedUpsRepo {
        private readonly DataContext _context;
        public InkedUpsRepo(DataContext context) {
            _context = context;
        }

        public async Task<IEnumerable<InkedUpDTO>> GetInkedUps() {
            var r = await _context.InkedUps
                .Include(x => x.Ink)
                .Include(x => x.FountainPen)
                .ToListAsync();
            return r.Adapt<IEnumerable<InkedUpDTO>>();
        }
        public async Task<InkedUpDTO?> GetInkedUp(int id) {
            var r = await _context.InkedUps
                .Include(x => x.Ink)
                .Include(x => x.FountainPen)
                .FirstOrDefaultAsync(x => x.Id == id);
            //TODO: ProjectTo does not work here as well...
            return r.Adapt<InkedUpDTO>();
        }
        public record InkedUpResult(ResultTypes ResultType, InkedUp? InkedUp = null);
        public async Task<InkedUpResult> UpdateInkedUp(int id, InkedUpDTO dto) {
            var inkedUp = dto.Adapt<InkedUpDTO>();
            if (inkedUp == null || id != inkedUp.Id) {
                return new InkedUpResult(ResultTypes.BadRequest);
            }

            var find = await _context.InkedUps.FindAsync(inkedUp.Id);
            if (find == null) return new InkedUpResult(ResultTypes.NotFound);

            _context.Entry(find).CurrentValues.SetValues(inkedUp);
            find.InkedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new InkedUpResult(ResultTypes.Ok, find);
        }
        public async Task<InkedUpResult> AddInkedUp(InkedUpDTO dto) {
            var inkedUp = dto.Adapt<InkedUp>();
            if (inkedUp == null) return new InkedUpResult(ResultTypes.BadRequest);
            _context.InkedUps.Add(inkedUp);
            await _context.SaveChangesAsync();

            await _context
                .InkedUps
                .Where(x => x.FountainPenId == dto.FountainPenId && x.IsCurrent)
                .ExecuteUpdateAsync(s => s.SetProperty(e => e.IsCurrent, false));

            return new InkedUpResult(ResultTypes.Ok, inkedUp);
        }
        public async Task<InkedUpResult> DeleteInkedUp(int id) {
            //TODO: what if I delete the active inkedup?
            var inkedUp = await _context.InkedUps.FindAsync(id);
            if (inkedUp == null) return new InkedUpResult(ResultTypes.NotFound);
            _context.InkedUps.Remove(inkedUp);
            await _context.SaveChangesAsync();
            return new InkedUpResult(ResultTypes.Ok);
        }
    }
}
