namespace Dal.Model
{
    public class Statistic : BaseModel
    {
        public virtual Player Player { get; set; }
        public int WinCount { get; set; }
        public int LoseCount { get; set; }
        public int Score { get; set; }
    }
}
