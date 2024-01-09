using System.ComponentModel.DataAnnotations.Schema;

namespace RefreshAPIEF.Models
{
    [Table("games")]
    public class Game
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("club_id")]
        public int? ClubId { get; set; }

        [Column("map_comp_id")]
        public int? MapCompId { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("link")]
        public string? Link { get; set; }

        [Column("icon")]
        public string? Icon { get; set; }

        [Column("param")]
        public string? Param { get; set; }

        [Column("type")]
        public int? Type { get; set; }

        [Column("steam_id")]
        public string? SteamId { get; set; }

        [Column("club_account")]
        public bool ClubAccount { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("hwnd")]
        public string? Hwnd { get; set; }

        [Column("status")]
        public bool? Status { get; set; }
    }
}
