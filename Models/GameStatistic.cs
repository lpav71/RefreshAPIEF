#pragma warning disable IDE1006 // Стили именования

using System.ComponentModel.DataAnnotations.Schema;

namespace RefreshAPIEF.Models
{
    [Table("game_statistic")]
    public class GameStatistic
    {
        public int id { get; set; }
        public int booking_id { get; set; }
        public int games_id { get; set; }
        public int duration_id { get; set; }
        public int clients_id { get; set; }
        public int club_id { get; set; }
    }
}
