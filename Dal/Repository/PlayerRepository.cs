using System.Data.SqlTypes;
using System.Linq;
using Dal.DataBaseHelper;
using Dal.Encryption;
using Dal.Model;

namespace Dal.Repository.DataBase
{
    public class PlayerRepository : BaseRepository<Player>
    {
        public PlayerRepository(GameContext gameContext) : base(gameContext)
        {

        }

        public override Player Get(long id)
        {
            var player = base.Get(id);
            StatisticRepository statRepo = new StatisticRepository(GameContext);
            player.Statistic = statRepo.Get(player.Id);
            return player;
        }

        public Player Get(string username, string password)
        {
            var player = Entity.SingleOrDefault(user => user.Username == username);         

            if (player == null || player.Password != Cryptography.EncryptSHA1(password))
                return null;
            StatisticRepository statRepo = new StatisticRepository(GameContext);
            player.Statistic = statRepo.Get(player.Id);

            return player;
        }

        public Player Get(string username)
        {
            var player = Entity.SingleOrDefault(user => user.Username == username);
            if (player == null)
                return null;

            StatisticRepository statRepo = new StatisticRepository(GameContext);
            player.Statistic = statRepo.Get(player.Id);

            return player;
        }

        public override Player Save(Player model)
        {
            StatisticRepository statRepo = new StatisticRepository(GameContext);
            model.Statistic = new Statistic();
            Entity.Add(model);

            return model;
        }
    }
}
