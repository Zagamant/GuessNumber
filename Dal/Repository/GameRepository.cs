using System.Collections.Generic;
using System.Linq;
using Dal.DataBaseHelper;
using Dal.Model;

namespace Dal.Repository
{
    public class GameRepository : BaseRepository<Game>
    {
        public GameRepository(GameContext gameContext) : base(gameContext)
        {

        }

        public List<Game> GetByUsersId(long userId)
        {
            return Entity.Where(x => x.PlayerMakeNumber.Id == userId || x.PlayerGuessingNumber.Id == userId).ToList();
        }

        public override Game Get(long id)
        {
            PlayerRepository playerRepo = new PlayerRepository(GameContext);
            var game = base.Get(id);
            game.PlayerGuessingNumber = playerRepo.Get(game.PlayerGuessingNumber.Id);
            game.PlayerMakeNumber = playerRepo.Get(game.PlayerMakeNumberId);
            return game;
        }

        public override Game Save(Game game)
        {
            return base.Save(game);
        }
    }
}
