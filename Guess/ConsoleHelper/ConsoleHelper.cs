using System;
using System.Linq;
using System.Text;
using Dal.DataBaseHelper;
using Dal.Encryption;
using Dal.Model;
using Dal.Repository;
using Guess.Validators;

namespace Guess.ConsoleHelper
{
    public static class ConsoleHelper
    {
        public static int ReadNumber(params IValidator<int>[] validators)
        {
            var line = Console.ReadLine();
            int number;

            do
            {
                while (!int.TryParse(line, out number))
                {
                    Console.WriteLine($"'{line}' it is not a number. Please enter number");
                    line = Console.ReadLine();
                }

                var errors = validators?.Where(x => !x.Validate(number)).SelectMany(x => x.Errors).ToList();
                if (errors?.Any() ?? false)
                {
                    Console.WriteLine(string.Join("", errors));
                    line = Console.ReadLine();
                }

            } while (validators?.Any(x => !x.Validate(number)) ?? false);

            return number;
        }

        public static string ReadString(params IValidator<string>[] validators)
        {
            var line = Console.ReadLine();

            do
            {
                var errors = validators?.Where(x => !x.Validate(line)).Select(x => x.Errors).ToList();
                if (errors?.Any() ?? false)
                {
                    Console.WriteLine(string.Join("", errors));
                    line = Console.ReadLine();
                }

            } while (validators?.Any(x => !x.Validate(line)) ?? false);

            return line;
        }

        public static Player ConsoleAuthenticate(TypeOfAuthorization auto, GameContext db, params IValidator<Player>[] validators)
        {
            PlayerRepository playerRepo = new PlayerRepository(db);
            Player player;

            switch (auto)
            {
                case TypeOfAuthorization.LogIn:
                    player = LoginWithConsole(playerRepo);
                    break;
                case TypeOfAuthorization.Registrate:
                    player = SignUpWithConsole(playerRepo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(auto), auto, null);
            }

            do
            {
                var errors = validators?.Where(x => !x.Validate(player)).SelectMany(x => x.Errors).ToList();
                Console.WriteLine(string.Join("", errors));
                if (errors?.Any() ?? false)
                {
                    switch (auto)
                    {
                        case TypeOfAuthorization.LogIn:
                            player = LoginWithConsole(playerRepo);
                            break;
                        case TypeOfAuthorization.Registrate:
                            player = SignUpWithConsole(playerRepo);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(auto), auto, null);
                    }
                }
            } while (validators?.Any(x => !x.Validate(player)) ?? false);

            return player;
        }

        private static Player SignUpWithConsole(PlayerRepository playerRepo)
        {
            Console.WriteLine("Sign up");
            Console.Write("Username: ");
            var username = ReadString();
            while (playerRepo.Get(username) != null)
            {
                Console.WriteLine("This username is taken. Try another");
                Console.Write("Username: ");
                username = ReadString();
            }
            Console.Write("Password: ");
            var password = ReadString();
            var player = new Player()
            {
                Username = username,
                Password = Cryptography.EncryptSha1(password)
            };
            playerRepo.Save(player);

            //playerRepo.DataBase.Stats.Add(new Statistic
            //{
            //    Player = player
            //});
            //playerRepo.DataBase.SaveChanges();
            return player;
        }

        public static string GetInstructions()
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("Press 1 to log-in");
            str.AppendLine("Press 2 to create account");
            str.AppendLine("Choose option: ");

            return str.ToString();
        }

        public static Player LoginWithConsole(PlayerRepository playerRepo)
        {
            Console.WriteLine("Login in system:");
            Console.Write("Username: ");
            var username = ReadString();
            Console.Write("Password: ");
            var password = ReadString();
            var player = playerRepo.Get(username, password);

            while (player == null)
            {
                Console.Clear();
                Console.WriteLine("Wrong username or password. Type 2 - to go back or any key to continue");
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.D2)
                    return null;

                Console.Write("Username: ");
                username = ReadString();
                Console.Write("Password: ");
                password = ReadString();
                player = playerRepo.Get(username, password);
            }

            return player;
        }
    }


}
