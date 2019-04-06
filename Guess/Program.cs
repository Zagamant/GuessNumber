using System;
using System.IO;
using Dal.DataBaseHelper;
using Dal.Model;
using Dal.Repository;
using Guess.ConsoleHelper;
using Guess.Validators;
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

        private static void ExampleWithMsSql()
        {
            using (var db = new GameContext( /*GetDataBaseOptions()*/))
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
                    player1 = ConsoleHelper.ConsoleHelper.ConsoleAuthenticate(typeOfAuthorization.Value, db);

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
                    player2 = ConsoleHelper.ConsoleHelper.ConsoleAuthenticate(typeOfAuthorization.Value, db,
                        new PlayerRepeatingValidator(player1));
                } while (player2 == null);

                Console.Clear();

                var game = new Guess.GameStuff.Game(player1, player2);
                var gameRepo = new GameRepository(db);
                gameRepo.Save(game.PlayOneRound());

                Console.WriteLine($"{player1.Username} - {player1.Id} - {player1.Password}");
                Console.WriteLine($"{player2.Username} - {player2.Id} - {player2.Password}");
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
