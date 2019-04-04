using System;
using System.IO;
using Dal.DataBaseHelper;
using Dal.Model;
using Dal.Repository;
using Guess.ConsoleHelper;
using Guess.GameStuff;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Guess
{
    class Program
    {
        static void Main()
        {
            ExampleWithMsSql();

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

        private static void ExampleWithMsSql()
        {
            using (var db = new GameContext(GetDataBaseOptions()))
            {
                using (var dbHelper = new GameDataBaseHelper(db))
                {
                    Console.WriteLine("Player 1 login:");
                    Console.WriteLine(ConsoleHelper.ConsoleHelper.GetInstructions());
                    ConsoleKeyInfo key;
                    TypeOfAuthorization? typeOfAuthorization = null;
                    do
                    {
                        key = Console.ReadKey();
                        switch (key.Key)
                        {
                            case ConsoleKey.D1:
                                typeOfAuthorization = TypeOfAuthorization.LogIn;
                                break;
                            case ConsoleKey.D2:
                                typeOfAuthorization = TypeOfAuthorization.Registrate;
                                break;
                        }
                    } while (typeOfAuthorization == null);
                    Console.Clear();
                    Player player1;
                    do
                    {
                        player1 = Guess.ConsoleHelper.ConsoleHelper.ConsoleAuthenticate(typeOfAuthorization.Value, dbHelper);

                    } while (player1 == null);
                     

                    Console.Clear();
                    Console.WriteLine("Player 2 login:");
                    Console.WriteLine(ConsoleHelper.ConsoleHelper.GetInstructions());
                    typeOfAuthorization = null;
                    do
                    {
                        key = Console.ReadKey();
                        switch (key.Key)
                        {
                            case ConsoleKey.D1:
                                typeOfAuthorization = TypeOfAuthorization.LogIn;
                                break;
                            case ConsoleKey.D2:
                                typeOfAuthorization = TypeOfAuthorization.Registrate;
                                break;
                        }
                    } while (typeOfAuthorization == null);
                    Console.Clear();
                    Player player2;
                    do
                    {
                        player2 = Guess.ConsoleHelper.ConsoleHelper.ConsoleAuthenticate(typeOfAuthorization.Value, dbHelper);
                    } while (player2 == null);
                    Console.Clear();

                    var game = new Game(player1, player2);
                    game.PlayOneRound();

                    dbHelper.SaveChanges(player1, player2);

                    Console.WriteLine($"{player1.Username} - {player1.Id} - {player1.Password}");
                    Console.WriteLine($"{player2.Username} - {player2.Id} - {player2.Password}");
                }
            }

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

        
    }
}
