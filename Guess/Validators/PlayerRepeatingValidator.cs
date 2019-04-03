using Dal.Model;

namespace Guess.Validators
{
    public class PlayerRepeatingValidator : IValidator<Player>
    {
        public string Errors { get; set; }
        private readonly Player _player;

        public PlayerRepeatingValidator(Player player)
        {
            _player = player;
        }

        public bool Validate(Player player)
        {
            if (_player == player)
                return false;
            if (_player.Username == player.Username)
                return false;
            return true;
        }
    }
}
