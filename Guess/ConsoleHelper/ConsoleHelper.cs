using System;
using System.Linq;
using System.Text;
using Dal.DataBaseHelper;
using Dal.Model;
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
                    Console.WriteLine(string.Join("\n", errors));
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
                var errors = validators?.Where(x => !x.Validate(line)).SelectMany(x => x.Errors).ToList();
                if (errors?.Any() ?? false)
                {
                    Console.WriteLine(string.Join("\n", errors));
                    line = Console.ReadLine();
                }

            } while (validators?.Any(x => !x.Validate(line)) ?? false);

            return line;
        }

        public static Player ConsoleAuthenticate(AuthorizationEnum auto, GameDataBaseHelper dbHelper, params IValidator<Player>[] validator)
        {
            switch (auto)
            {
                case AuthorizationEnum.LogIn:
                    return LoginWithConsole(dbHelper, validator);

                case AuthorizationEnum.Registrate:
                    return SignUpWithConsole(dbHelper, validator);
                default:
                    throw new ArgumentOutOfRangeException(nameof(auto), auto, null);
            }
        }

        private static Player SignUpWithConsole(GameDataBaseHelper dbHelper, params IValidator<Player>[] validator)
        {
            Console.WriteLine("Sign up :");
            Console.Write("Username: ");
            var username = ReadString();
            while (dbHelper.GetPlayerFromDB(username) != null)
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
                Password = password,
                
            };
            dbHelper.DataBase.Players.Add(player);
            dbHelper.DataBase.SaveChanges();

            dbHelper.DataBase.Stats.Add(new Statistic()
            {
                PlayerId = dbHelper.DataBase.Players.SingleOrDefault(playerT => playerT.Username == player.Username).Id
            });
            dbHelper.DataBase.SaveChanges();
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

        public static Player LoginWithConsole(GameDataBaseHelper dbHelper, params IValidator<Player>[] validator)
        {
            Console.WriteLine("Login in system:");
            Console.Write("Username: ");
            var username = ReadString();
            Console.Write("Password: ");
            var password = ReadString();
            var player = dbHelper.Authenticate(username, password);

            while (player == null)
            {
                Console.Clear();
                var key = new ConsoleKeyInfo();
                Console.WriteLine("Wrong username or password. Type 2 - to go back");
                switch (key.Key)
                {
                    case ConsoleKey.D2:
                        return null;
                }

                Console.Write("Username: ");
                username = ReadString();
                Console.Write("Password: ");
                password = ReadString();
                player = dbHelper.Authenticate(username, password);
            }

            return player;
        }
    }


}
