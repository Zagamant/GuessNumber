using System.Linq;
using Dal.Model;
using Microsoft.EntityFrameworkCore;

namespace Dal.DataBaseHelper
{
    public sealed class GameContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Statistic> Stats { get; set; }
        public DbSet<Game> Games { get; set; }

        //public GameContext(DbContextOptions options) : base(options)
        //{
        //    Database.EnsureCreated();
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }


            modelBuilder.Entity<Player>().HasOne(player => player.Statistic).WithOne(stat => stat.Player)
                .HasForeignKey<Statistic>(stat => stat.Id).OnDelete(DeleteBehavior.Cascade);
          

            //modelBuilder.Entity<Game>()
            //    .HasOne(game => game.PlayerGuessingNumber)
            //    .WithMany(player => player.Games)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Player>().HasMany(game => game.Games)
                .WithOne(player => player.PlayerGuessingNumber)
                .OnDelete(DeleteBehavior.Cascade);





            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=GameDB;Trusted_Connection=True;");
        }

    }
}
