using System.ComponentModel.DataAnnotations.Schema;

namespace RefreshAPIEF.Models
{
    [Table("club_steam_account")]
    public class ClubSteamAccount
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("club_id")]
        public int? ClubId { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [Column("game")]
        public string? Game { get; set; }

        [Column("steam_id")]
        public string? SteamId { get; set; }

        [Column("login_steam")]
        public string? LoginSteam { get; set; }

        [Column("pass_steam")]
        public string? PassSteam { get; set; }

        [Column("last_update")]
        public DateTime? LastUpdate { get; set; }

        [Column("status")]
        public bool? Status { get; set; }
    }
}
