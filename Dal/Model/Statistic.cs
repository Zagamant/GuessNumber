using System.ComponentModel.DataAnnotations.Schema;

namespace Dal.Model
{
    public class Statistic : BaseModel
    {
        public long PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        public Player Player { get; set; }
        public int WinCount { get; set; }
        public int LoseCount { get; set; }
        public int Score { get; set; }
    }
}
