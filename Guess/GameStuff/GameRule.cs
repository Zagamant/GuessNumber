namespace Guess.GameStuff
{
    public class GameRule
    {
        public int Min { get; }
        public int Max { get; }
        public int GuessNumber { get; }
        public int MaxAttempt { get; }

        public GameRule(int min, int max, int guessNumber)
        {
            Min = min;
            Max = max;
            GuessNumber = guessNumber;
            MaxAttempt = CalculateAttemptCount();
        }

        private int CalculateAttemptCount()
        {
            var length = Max - Min + 1;
            var sum = 2;
            var power = 1;
            while (sum < length)
            {
                sum *= 2;
                power++;
            }

            return power;
        }
    }
}
