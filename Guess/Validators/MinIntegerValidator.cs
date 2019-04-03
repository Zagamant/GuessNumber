namespace Guess.Validators
{
    class MinIntegerValidator : IValidator<int>
    {
        private int _minValue;
        public string Errors { get; set; }

        public MinIntegerValidator(int minValue)
        {
            _minValue = minValue;
        }
        public bool Validate(int value)
        {
            if (value < _minValue)
            {
                Errors = $"Entered number is less than {_minValue}";
                return false;
            }

            return true;
        }
    }
}
