﻿using System;
using System.Collections.Generic;
using Dal.Model;
using Guess.Validators;
using dbModels = Dal.Model;

namespace Guess.GameStuff
{
    public class Game
    {
        public GameRule GameRule { get; private set; }

        private readonly Player _player1;
        private readonly Player _player2;

        private dbModels.Game game;

        private bool _win;
        private int _attempt;
        public List<string> Errors { get; set; } = new List<string>();

        public Game(Player player1, Player player2)
        {
            _player1 = player1;
            _player2 = player2;
            game = new dbModels.Game
            {
                Date = DateTime.Now,
                PlayerMakeNumber = _player1,
                PlayerGuessingNumber = _player2
            };
            
        }

        public dbModels.Game PlayOneRound()
        {
            FirstPlayerSetGameRules();

            SecondPlayerTryingToGuess();

            EndGame();
            
            Console.ReadKey();

            return game;
        }

        private void EndGame()
        {

            if (_win)
            {
                _player2.Statistic.WinCount++;
                _player2.Statistic.Score += GameRule.MaxAttempt - ++_attempt;
                Console.WriteLine("Win!");
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
