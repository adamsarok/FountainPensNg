using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static FountainPensNg.Server.Data.Repos.FountainPensRepo;
using static FountainPensNg.Server.Data.Repos.ResultType;

namespace FountainPensNg.Server.Data.Repos {
    public class InkedUpsRepo(DataContext context) {
        public async Task<IEnumerable<InkedUpDTO>> GetInkedUps() {
            var r = await context.InkedUps
                .Include(x => x.Ink)
                .Include(x => x.FountainPen)
                .ToListAsync();
            return r.Adapt<IEnumerable<InkedUpDTO>>();
        }
        public async Task<InkedUpDTO?> GetInkedUp(int id) {
            var r = await context.InkedUps
                .Include(x => x.Ink)
                .Include(x => x.FountainPen)
                .FirstOrDefaultAsync(x => x.Id == id);
            //TODO: ProjectTo does not work here as well...
            return r.Adapt<InkedUpDTO>();
        }
        public record InkedUpResult(ResultTypes ResultType, InkedUpDTO? InkedUp = null);
        public async Task<InkedUpResult> UpdateInkedUp(int id, InkedUpDTO dto) {
            var inkedUp = dto.Adapt<InkedUpDTO>();
            if (inkedUp == null || id != inkedUp.Id) {
                return new InkedUpResult(ResultTypes.BadRequest);
            }

            var find = await context.InkedUps.FindAsync(inkedUp.Id);
            if (find == null) return new InkedUpResult(ResultTypes.NotFound);

            context.Entry(find).CurrentValues.SetValues(inkedUp);
            find.InkedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();

            return new InkedUpResult(ResultTypes.Ok, find.Adapt<InkedUpDTO>());
        }

        public async Task<InkedUpResult> AddInkedUp(InkedUpDTO dto) {
            var inkedUp = dto.Adapt<InkedUp>();
            if (inkedUp == null) return new InkedUpResult(ResultTypes.BadRequest);
            //it seems mapster does not map the FountainPenId and InkId fields for some reason, also if mapped EF tries to add pen & ink
            inkedUp.FountainPen = await context.FountainPens.FindAsync(dto.FountainPenId);
            inkedUp.Ink = await context.Inks.FindAsync(dto.InkId);
            inkedUp.IsCurrent = true;

            await context
                .InkedUps
                .Where(x => x.FountainPenId == dto.FountainPenId && x.IsCurrent)
                .ExecuteUpdateAsync(s => s.SetProperty(e => e.IsCurrent, false));

            context.InkedUps.Add(inkedUp);
            
            await context.SaveChangesAsync();

            return new InkedUpResult(ResultTypes.Ok, inkedUp.Adapt<InkedUpDTO>());
        }
        public async Task<InkedUpResult> DeleteInkedUp(int id) {
            //TODO: what if I delete the active inkedup?
            var inkedUp = await context.InkedUps.FindAsync(id);
            if (inkedUp == null) return new InkedUpResult(ResultTypes.NotFound);
            context.InkedUps.Remove(inkedUp);
            await context.SaveChangesAsync();
            return new InkedUpResult(ResultTypes.Ok);
        }
    }
}
