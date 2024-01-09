using System.ComponentModel.DataAnnotations.Schema;

namespace RefreshAPIEF.Models
{
    [Table("cashbox")]
    public class Cashbox
    {
        public int id { get; set; }
        public int club_id { get; set; }
        public int? user_id { get; set; }
        public int admin_id { get; set; }
        public string? admin_name { get; set; }
        public int? shift { get; set; }
        public double? amount { get; set; }
        public int? type_operation { get; set; }
        public int? check { get; set; }
        public string? cashbox { get; set; }
        public double? old_amount { get; set; }
        public double? old_bonus { get; set; }
        public bool? status_check { get; set; }
        public DateTime? dt_operation { get; set; }
    }
}