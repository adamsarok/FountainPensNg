using FountainPensNg.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FountainPensNg.Server.Data {
    public class DataContext(DbContextOptions options) : DbContext(options) {
		public DbSet<Ink> Inks { get; set; }
        public DbSet<FountainPen> FountainPens { get; set; }
        public DbSet<InkedUp> InkedUps { get; set; }
        public DbSet<Paper> Papers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InkedUp>(entity => {
                entity.HasOne(x => x.FountainPen)
                    .WithMany(x => x.InkedUps)
                    .HasForeignKey(e => e.FountainPenId);
                entity.HasOne(x => x.Ink)
                    .WithMany(x => x.InkedUps)
                    .HasForeignKey(e => e.InkId);
            });
            modelBuilder.Entity<Ink>(entity => { 
                entity.HasIndex(p => new { p.Maker, p.InkName })
                    .IsUnique(true);
                entity.HasGeneratedTsVectorColumn(
                      p => p.FullText,
                      "english",
                      p => new { p.Maker, p.InkName, p.Comment, p.Rating })
                  .HasIndex(p => p.FullText)
                  .HasMethod("GIN");
            });
            modelBuilder.Entity<FountainPen>(entity => {
                entity.HasGeneratedTsVectorColumn(
                      p => p.FullText,
                      "english",
                      p => new { p.Maker, p.ModelName, p.Comment, p.Rating })
                  .HasIndex(p => p.FullText)
                  .HasMethod("GIN");
            });
            modelBuilder.Entity<Paper>(entity => {
                entity.HasGeneratedTsVectorColumn(
                      p => p.FullText,
                      "english",
                      p => new { p.Maker, p.PaperName, p.Comment, p.Rating })
                  .HasIndex(p => p.FullText)
                  .HasMethod("GIN");
            });
        }
    }
}
