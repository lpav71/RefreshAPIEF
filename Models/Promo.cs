using System.ComponentModel.DataAnnotations.Schema;

namespace RefreshAPIEF.Models
{
    [Table("promo")]
    public class Promo
    {           
        public int id { get; set; }
        public int? club_id { get; set; }
        public double? bonus { get; set; }
        public string? promo { get; set; }
        public string? description { get; set; }
        public int? num { get; set; }
        public DateOnly? date_start { get; set; }
        public DateOnly? date_stop { get; set; }
        public bool? status { get; set; }
        public int? max_activation { get; set; }
        public int? activations { get; set; }
        public string? image { get; set; }

    }
}
