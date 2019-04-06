using Dal.DataBaseHelper;
using Dal.Model;

namespace Dal.Repository.DataBase
{
    public class StatisticRepository : BaseRepository<Statistic>
    {
        public StatisticRepository(GameContext gameContext) : base(gameContext)
        {

        }

    }
}
