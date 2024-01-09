using System.ComponentModel.DataAnnotations.Schema;

namespace RefreshAPIEF.Models
{
    [Table("booking")]
    public class Booking
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("club_id")]
        public int? ClubId { get; set; }

        [Column("price_id")]
        public int? PriceId { get; set; }

        [Column("map_comp_id")]
        public int? MapCompId { get; set; }

        [Column("res")]
        public int? Res { get; set; }

        [Column("duration")]
        public int? Duration { get; set; }

        [Column("tariff_type")]
        public int? TariffType { get; set; }

        [Column("status")]
        public int? Status { get; set; }

        [Column("time_start")]
        public DateTime? TimeStart { get; set; }

        [Column("time_stop")]
        public DateTime? TimeStop { get; set; }

        [Column("time_update")]
        public DateTime? TimeUpdate { get; set; }

        [Column("start_amount")]
        public double? StartAmount { get; set; }

        [Column("amount")]
        public double? Amount { get; set; }

        [Column("start_bonus")]
        public double? StartBonus { get; set; }

        [Column("bonus")]
        public double? Bonus { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [Column("id_zone")]
        public int? IdZone { get; set; }

        [Column("session_pause")]
        public int? SessionPause { get; set; }
    }
}
