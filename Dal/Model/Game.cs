using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dal.Model
{
    public class Game : BaseModel
    {
        public long PlayerMakeNumberId { get; set; }
        [ForeignKey("PlayerMakeNumberId")]
        public virtual Player PlayerMakeNumber { get; set; }

        public virtual Player PlayerGuessingNumber { get; set; }
        public DateTime Date { get; set; }

    }
}
