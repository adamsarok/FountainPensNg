using FountainPensNg.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FountainPensNg.Server.Data {
    public class DataContext : DbContext {
        public DataContext(DbContextOptions options) : base(options) {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder.LogTo(Console.WriteLine);
        public DbSet<Ink> Inks { get; set; }
        public DbSet<FountainPen> FountainPens { get; set; }
        public DbSet<InkedUp> InkedUps { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<InkedUp>()
                .HasOne(x => x.FountainPen)
                .WithMany(x => x.InkedUps)
                .HasForeignKey(e => e.FountainPenId);
            modelBuilder.Entity<InkedUp>()
                .HasOne(x => x.Ink)
                .WithMany(x => x.InkedUps)
                .HasForeignKey(e => e.InkId);
            modelBuilder.Entity<Ink>()
                .HasIndex(p => new { p.Maker, p.InkName })
                .IsUnique(true);
        }
    }
}
