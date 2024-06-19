// using FountainPensNg.Server.Data;
// using FountainPensNg.Server.Data.Models;
// using FountainPensNg.Server.Helpers;
// using Microsoft.EntityFrameworkCore;

// namespace FountainPensNg.Server.Services {
//     public class Repository : IRepository {
//         private readonly IDbContextFactory<DataContext> DbFactory;
//         private bool inksChanged;
//         private bool pensChanged;
//         private List<Ink>? inksCache;
//         private List<FountainPen>? pensCache;

//         public Repository(IDbContextFactory<DataContext> contextFactory) {
//             this.DbFactory = contextFactory;
//         }

//         public async void AddInk(Ink ink) {
//             if (!string.IsNullOrWhiteSpace(ink.Color)) {
//                 var cielab = ColorHelper.ToCIELAB(ink.Color);
//                 ink.Color_CIELAB_L = cielab.L;
//                 ink.Color_CIELAB_a = cielab.A;
//                 ink.Color_CIELAB_b = cielab.B;
//             }
//             using var context = DbFactory.CreateDbContext();
//             if (ink.Id == 0) await context.Inks.AddAsync(ink);
//             else context.Inks.Update(ink);
//             await context.SaveChangesAsync();
//             inksChanged = true;
//         }

//         public async void AddPen(FountainPen pen) {
//             using var context = DbFactory.CreateDbContext();
//             if (pen.Id == 0) {
//                 context.FountainPens.Attach(pen);
//                 if (pen.CurrentInk != null) context.Entry(pen.CurrentInk).State = EntityState.Unchanged;
//             } else context.FountainPens.Update(pen);
//             await context.SaveChangesAsync();
//             pensChanged = true;
//         }

//         public async ValueTask<Ink?> GetInk(int id) {
//             using var context = DbFactory.CreateDbContext();
//             return await context.Inks
//                 .Include(x => x.CurrentPens.Take(1))
//                 .Where(x => x.Id == id)
//                 .FirstOrDefaultAsync();
//         }
//         public async ValueTask<FountainPen?> GetPen(int id) {
//             using var context = DbFactory.CreateDbContext();
//             return await context.FountainPens.FindAsync(id);
//         }

//         public async Task<List<Ink>> GetInks() {
//             if (inksCache == null || inksChanged) {
//                 using var context = DbFactory.CreateDbContext();
//                 inksCache = await context.Inks.ToListAsync();
//             }
//             inksChanged = false;
//             return inksCache;
//         }


//         public async Task<List<FountainPen>> GetPens() {
//             if (pensCache == null || pensChanged) {
//                 using var context = DbFactory.CreateDbContext();
//                 pensCache = await context.FountainPens.ToListAsync();
//             }
//             pensChanged = false;
//             return pensCache;
//         }
//     }
// }
