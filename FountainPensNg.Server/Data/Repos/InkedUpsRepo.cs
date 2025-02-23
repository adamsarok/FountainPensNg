using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;
using FountainPensNg.Server.Exceptions;
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
			return r.Adapt<InkedUpDTO>(); //ProjectTo does not work here as well...
		}
		public async Task<InkedUpDTO> UpdateInkedUp(InkedUpUploadDto dto) {
			var inkedUp = dto.Adapt<InkedUpDTO>();
			if (inkedUp == null) throw new NotFoundException();
			var find = await context.InkedUps
				.Include(x => x.FountainPen)
				.Include(x => x.Ink)
				.FirstOrDefaultAsync(x => x.Id == inkedUp.Id);
			if (find == null) throw new NotFoundException();

			context.Entry(find).CurrentValues.SetValues(inkedUp);
			find.InkedAt = DateTime.UtcNow;

			await context.SaveChangesAsync();

			return find.Adapt<InkedUpDTO>();
		}

		public async Task<InkedUpDTO> AddInkedUp(InkedUpUploadDto dto) {
			var inkedUp = dto.Adapt<InkedUp>();
			if (inkedUp == null) throw new MappingException();
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

			return inkedUp.Adapt<InkedUpDTO>();
		}
		public async Task DeleteInkedUp(int id) {
			//TODO: what if I delete the active inkedup?
			var inkedUp = await context.InkedUps.FindAsync(id);
			if (inkedUp == null) throw new NotFoundException();
			context.InkedUps.Remove(inkedUp);
			await context.SaveChangesAsync();
		}
	}
}
