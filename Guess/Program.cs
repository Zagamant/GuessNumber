using System;
using System.IO;
using Dal.DataBaseHelper;
using Dal.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Guess
{
    class Program
    {
        static void Main()
        {

            Checker();
            //ExampleWithMsSql();

                Console.ReadKey();
        }

        private static DbContextOptions<GameContext> GetDataBaseOptions()
        {
            var builder = new ConfigurationBuilder();
            // установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла appsettings.json
            builder.AddJsonFile("appsettings.json");
            // создаем конфигурацию
            var config = builder.Build();
            // получаем строку подключения
            string connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<GameContext>();
            var options = optionsBuilder
                .UseSqlServer(connectionString)
                .Options;

            return options;
        }

        private static void Checker()
        {
            using (var db = new GameContext( /*GetDataBaseOptions()*/))
            {
                PlayerRepository pl = new PlayerRepository(db);
                var player = pl.Get(2);
                Console.WriteLine(player.Username + " " + player.Password + "\nStats: " + player.Statistic.Score);

                Console.WriteLine();

                GameRepository gm = new GameRepository(db);

                var game = gm.Get(1);

                Console.WriteLine(game.PlayerMakeNumber.Id + " " + game.PlayerGuessingNumber.Id + "\nDate: " + game.Date);
            }
        }
    }
}
