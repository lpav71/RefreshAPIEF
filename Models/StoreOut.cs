#pragma warning disable IDE1006 // Стили именования
using System.ComponentModel.DataAnnotations.Schema;

namespace RefreshAPIEF.Models
{
    [Table("store_out")]
    public class StoreOut
    {
        public int id { get; set; }
        public int? club_id { get; set; }
        public int? store_id { get; set; }
        public int? admin { get; set; }
        public int? num { get; set; }
        public DateTime? dateout { get; set; }
        public string? description { get; set; }
        public int? store_operation_type_id { get; set; }
        public int? app_id { get; set; }
    }
}
