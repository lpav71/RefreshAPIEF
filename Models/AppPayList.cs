using System.ComponentModel.DataAnnotations.Schema;

namespace RefreshAPIEF.Models
{
    [Table("app_pay_list")]
    public class AppPayList
    {
        public int id { get; set; }
        public int? app_pay_id { get; set; }
        public int store_id { get; set; }
        public double? amount { get; set; }
        public int num { get; set; }
    }
}
