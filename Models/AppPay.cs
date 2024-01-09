#pragma warning disable IDE1006 // Стили именования

using System.ComponentModel.DataAnnotations.Schema;

namespace RefreshAPIEF.Models
{
    [Table("app_pay")]
    public class AppPay
    {
        public int id { get; set; }
        public int? clients { get; set; }
        public int? club_id { get; set; }
        public bool? status { get; set; }
        public double? amount { get; set; }
        public DateTime? created_at { get; set; }
        public int? admin_id { get; set; }
        public DateTime? pay_datetime { get; set; }
        public int? pay_type { get; set; }
        public bool? delivery { get; set; }
        public bool? delivery_complete { get; set; }
        public int? product_num { get; set; }
        public int? shift { get; set; }
        public int comp_id { get; set; }
    }
}
