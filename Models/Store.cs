#pragma warning disable IDE1006 // Стили именования
using System.ComponentModel.DataAnnotations.Schema;

namespace RefreshAPIEF.Models
{
    [Table("store")]
    public class Store
    {
        public int id { get; set; }
        public int? club_id { get; set; }
        public int? admin_id { get; set; }
        public string? admin_name { get; set; }
        public DateTime? dt_create { get; set; }
        public string? product { get; set; }
        public string? product_param { get; set; }
        public string? barcode { get; set; }
        public double? price { get; set; }
        public bool? shell_show { get; set; }
        public string? icon { get; set; }
        public bool? discount { get; set; }
        public int? num { get; set; }
        public int? types { get; set; }
        public double? price_bonus { get; set; }
    }
}
