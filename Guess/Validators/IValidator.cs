namespace Guess.Validators
{
    public interface IValidator<in T>
    {
        string Errors { get; set; }
        bool Validate(T value);
    }
}
