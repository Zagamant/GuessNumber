using Dal.Model;

namespace Dal.Repository
{
    public class StatisticRepository : BaseRepository<Statistic>
    {
        private const string FolderName = "Statistic";

        public StatisticRepository() : base(FolderName)
        {
        }
    }
}
