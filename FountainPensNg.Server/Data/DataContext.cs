using FountainPensNg.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FountainPensNg.Server.Data {
    public class DataContext : DbContext {
        public DataContext(DbContextOptions options) : base(options) {

        }
        public DbSet<Ink> Inks { get; set; }
        public DbSet<FountainPen> FountainPens { get; set; }
        public DbSet<InkedUp> InkedUps { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<InkedUp>()
                .HasOne(x => x.FountainPen)
                .WithMany(x => x.InkedUps);
            modelBuilder.Entity<InkedUp>()
                .HasOne(x => x.Ink)
                .WithMany(x => x.InkedUps);
            modelBuilder.Entity<FountainPen>()
                .HasOne(x => x.CurrentInk)
                .WithMany(x => x.CurrentPens);
            modelBuilder.Entity<Ink>()
                .HasIndex(p => new { p.Maker, p.InkName })
                .IsUnique(true);

            //seeb
            modelBuilder.Entity<FountainPen>().HasData(
                new FountainPen() {
                    Id = 1,
                    Maker = "Jinhao",
                    ModelName = "X159",
                    Nib = FountainPen.NibTypes.M,
                    Status = FountainPen.Statuses.Favorite,
                    Comment = "Nice writer, dries out quickly?",
                },
                new FountainPen() {
                    Id = 2,
                    Maker = "Jinhao",
                    ModelName = "X159",
                    Nib = FountainPen.NibTypes.M,
                    Status = FountainPen.Statuses.Favorite,
                    Comment = "Nice writer, dries out quickly?",
                }
            );
            modelBuilder.Entity<Ink>().HasData(
                new Ink() {
                    Id = 1,
                    Maker = "Pilot",
                    InkName = "Iroshizuku Kon-Peki",
                    Status = Ink.Statuses.Favorite,
                    Comment = "striking blue"
                }
                );
        }
    }
}
