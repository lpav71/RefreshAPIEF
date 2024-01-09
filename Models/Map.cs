#pragma warning disable IDE1006 // Стили именования
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RefreshAPIEF.Models
{
    [Table("map")]
    public class Map
    {
        [JsonIgnore]
        [Column("id")]
        public int Id { get; set; }

        [Column("club_id")]
        public int? ClubId { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [Column("id_comp")]
        public int? IdComp { get; set; }

        [Column("zone")]
        public int? Zone { get; set; }

        [Column("level")]
        public int? Level { get; set; }

        [Column("pos_x")]
        public int? PosX { get; set; }

        [Column("pos_y")]
        public int? PosY { get; set; }

        [Column("status_active")]
        public bool? StatusActive { get; set; }

        [Column("ip")]
        public string? Ip { get; set; }

        [Column("mac")]
        public string? Mac { get; set; }

        [Column("ver")]
        public int? Ver { get; set; }

        [NotMapped]
        public string? ApiKey { get; set; }
        public int status_num { get; set; }
        public string? mb { get; set; }
        public string? cpu { get; set; }
        public string? gpu { get; set; }
        public string? ram { get; set; }
        public string? disk { get; set; }
        public string? temp { get; set; }
        public string? qr_to_login { get; set; }
        public int qr_gen { get; set; }
        public string? u_login { get; set; }
        public string? u_pass { get; set; }
    }
}
