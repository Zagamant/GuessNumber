using System;
using System.IO;
using System.Text;
using Dal.DataBaseHelper;
using Dal.Model;
using Dal.Repository;
using Guess.ConsoleHelper;
using Guess.GameStuff;
using Guess.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Guess
{
    class Program
    {
        static void Main()
        {
            ExampleWithMSSql();

            Console.ReadKey();
        }

        private static void ExampleWithJson()
        {
            var playerRep = new PlayerRepository();
            var player1 = playerRep.GetJson(1) ?? new Player()
            {
                Id = 1,
                Username = "Zaga"
            };

            var player2 = playerRep.GetJson(2) ?? new Player()
            {
                Id = 2,
                Username = "Bloodlinez"
            };

            var game = new Game(player1, player2);
            game.PlayOneRound();


            playerRep.SaveJson(player1);
            playerRep.SaveJson(player2);
        }

        private static void ExampleWithMSSql()
        {
            using (var db = new GameContext(getDataBaseOptions()))
            {
                using (var dbHelper = new GameDataBaseHelper(db))
                {
                    Console.WriteLine("Player 1 login:");
                    Console.WriteLine(ConsoleHelper.ConsoleHelper.GetInstructions());
                    var key = new ConsoleKeyInfo();
                    AuthorizationEnum? enumchik = null;
                    do
                    {
                        key = Console.ReadKey();
                        switch (key.Key)
                        {
                            case ConsoleKey.D1:
                                enumchik = AuthorizationEnum.LogIn;
                                break;
                            case ConsoleKey.D2:
                                enumchik = AuthorizationEnum.Registrate;
                                break;
                        }
                    } while (enumchik == null);
                    var player1 = Guess.ConsoleHelper.ConsoleHelper.ConsoleAuthenticate(enumchik.Value, dbHelper);

                    Console.Clear();
                    Console.WriteLine("Player 2 login:");
                    Console.WriteLine(ConsoleHelper.ConsoleHelper.GetInstructions());
                    key = new ConsoleKeyInfo();
                    enumchik = null;
                    do
                    {
                        key = Console.ReadKey();
                        switch (key.Key)
                        {
                            case ConsoleKey.D1:
                                enumchik = AuthorizationEnum.LogIn;
                                break;
                            case ConsoleKey.D2:
                                enumchik = AuthorizationEnum.Registrate;
                                break;
                        }
                    } while (enumchik == null);
                    var player2 = Guess.ConsoleHelper.ConsoleHelper.ConsoleAuthenticate(enumchik.Value, dbHelper);

                    

                    var game = new Game(player1, player2);
                    game.PlayOneRound();

                    dbHelper.SaveChanges(player1, player2);

                    Console.WriteLine($"{player1.Username} - {player1.Id} - {player1.Password}");
                    Console.WriteLine($"{player2.Username} - {player2.Id} - {player2.Password}");
                }
            }

        }

        private static DbContextOptions<GameContext> getDataBaseOptions()
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

        
    }
}
