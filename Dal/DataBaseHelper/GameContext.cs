using Dal.Model;
using Microsoft.EntityFrameworkCore;

namespace Dal.DataBaseHelper
{
    public sealed class GameContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Statistic> Stats { get; set; }

        public GameContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public GameContext() : base()
        {
            Database.EnsureCreated();
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Player>().HasKey(u => u.Id);
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=GameDB;Trusted_Connection=True;");
        }

    }
}
