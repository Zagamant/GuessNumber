using Dal.Model;

namespace Dal.Repository
{
    public class PlayerRepository :BaseRepository<Player>
    {
        public const string FolderName = "Player";
        public PlayerRepository() :base(FolderName)
        {
        }     

    }
}
