using System.ComponentModel.DataAnnotations.Schema;

namespace RefreshAPIEF.Models
{
    [Table("price")]
    public class Price
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("club_id")]
        public int? ClubId { get; set; }

         [Column("id_zone")]
        public int? IdZone { get; set; }

         [Column("price")]
        public double? Price1 { get; set; }

         [Column("week_day")]
        public int? WeekDay { get; set; }

        [Column("duration")]
        public int? Duration { get; set; }

        [Column("tariff_type")]
        public int? TariffType { get; set; }

        [Column("status_active")]
        public bool? StatusActive { get; set; }

        [Column("time_start")]
        public TimeOnly? TimeStart { get; set; }

        [Column("time_stop")]
        public TimeOnly? TimeStop { get; set; }

        [Column("time_fixed")]
        public TimeOnly? TimeFixed { get; set; }

        [Column("name")]
        public string? Name { get; set; }
        public bool alive { get; set; }
    }
}
