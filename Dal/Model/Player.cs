using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dal.Model
{
    public class Player : BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Username { get; set; }
        public string Password { get; set; }
        public virtual Statistic Statistic { get; set; }
        public virtual List<Game> Games { get; set; }

    }
}
