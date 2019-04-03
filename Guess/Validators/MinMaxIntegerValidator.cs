namespace Guess.Validators
{
    class MinMaxIntegerValidator :IValidator<int>
    {
        private int _minValue;
        private int _maxValue;

        public MinMaxIntegerValidator(int minValue, int maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }

        public string Errors { get; set; }

        public bool Validate(int value)
        {
            if (value < _minValue)
            {
                Errors = $"Number must be more than {_minValue}";
                return false;
            }

            if (value > _maxValue)
            {
                Errors = $"Number must be less than {_maxValue}";
                return false;
            }

            return true;
        }
    }
}
