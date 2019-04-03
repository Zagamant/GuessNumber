using System;
using System.Collections.Generic;
using System.Linq;
using Dal.Model;
using Guess.Validators;

namespace Guess.GameStuff
{
    public class Game
    {
        public GameRule GameRule { get; private set; }

        private Player _player1;
        private Player _player2;

        private bool _win;
        private int _attempt;
        public List<string> Errors { get; set; } = new List<string>();

        public Game(Player player1, Player player2)
        {
            _player1 = player1;

            _player2 = player2;          
        }

        public void PlayOneRound()
        {
            FirstPlayerSetGameRules();

            SecondPlayerTryingToGuess();

            EndGame();
            
            Console.ReadKey();
        }

        private void EndGame()
        {

            if (_win)
            {
                _player2.Statistic.WinCount++;
                _player2.Statistic.Score += GameRule.MaxAttempt - ++_attempt;
                Console.WriteLine("_win!");
            }
            else
            {
                _player2.Statistic.LoseCount++;
                Console.WriteLine("Looser!");
            }

        }


        private void FirstPlayerSetGameRules()
        {
            Console.WriteLine("Press any key to start...");
            Console.ReadKey();

            Console.WriteLine($"Enter min value");
            var min = ConsoleHelper.ConsoleHelper.ReadNumber();

            Console.WriteLine($"Enter max value");
            var max = ConsoleHelper.ConsoleHelper.ReadNumber(new MinIntegerValidator(min));

            Console.WriteLine($"Enter guessing Number value");
            var guessNumber = ConsoleHelper.ConsoleHelper.ReadNumber(new MinMaxIntegerValidator(minValue: min, maxValue: max));

            GameRule = new GameRule(min, max, guessNumber);
        }


        private void SecondPlayerTryingToGuess()
        {
            Console.Clear();
            Console.Write("Press any key to start: ");
            Console.ReadKey();

            _win = false;
            do
            {
                Console.WriteLine(
                    $"You still have {GameRule.MaxAttempt - ++_attempt} attempts. Guess the number from [{GameRule.Min}, {GameRule.Max}]");
                var player2GuessNumber = ConsoleHelper.ConsoleHelper.ReadNumber(new MinMaxIntegerValidator(GameRule.Min, GameRule.Max));

                if (player2GuessNumber > GameRule.GuessNumber)
                {
                    Console.WriteLine($"{player2GuessNumber} > player1Number");
                }

                if (player2GuessNumber < GameRule.GuessNumber)
                {
                    Console.WriteLine($"{player2GuessNumber} < player1Number");
                }

                if (player2GuessNumber == GameRule.GuessNumber)
                {
                    _win = true;
                }
            } while (!_win && _attempt < GameRule.MaxAttempt);
        }

    }
}
