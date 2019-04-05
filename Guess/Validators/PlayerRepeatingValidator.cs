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
            if (player.Equals(_player))
            {
                Errors = "U entered the same person";
                return false;          
            }

            if (_player == player)
            {
                Errors = "U entered the same person";
                return false;
            }

            if (_player.Username == player.Username)
            {
                Errors = "U entered the same person";
                return false;
            }
            return true;
        }
    }
}
