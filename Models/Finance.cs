using System.ComponentModel.DataAnnotations.Schema;

namespace RefreshAPIEF.Models
{
    [Table("finance")]
    public class Finance
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("club_id")]
        public int? ClubId { get; set; }

        [Column("admin_id")]
        public int? AdminId { get; set; }

        [Column("dt_create")]
        public DateTime? DtCreate { get; set; }

        [Column("shift")]
        public int? Shift { get; set; }

        [Column("open_shift")] 
        public DateTime? OpenShift { get; set; }

        [Column("close_shift")]
        public DateTime? CloseShift { get; set; }

        [Column("cash")]
        public double? Cash { get; set; } = 0;

        [Column("cash_num")]
        public int? CashNum { get; set; } = 0;

        [Column("nocash")]
        public double? Nocash { get; set; } = 0;

        [Column("nocash_num")]
        public int? NocashNum { get; set; } = 0;

        [Column("return_cash")]
        public double? ReturnCash { get; set; } = 0;

        [Column("return_cash_num")]
        public int? ReturnCashNum { get; set; } = 0;

        [Column("return_nocash")]
        public double? ReturnNocash { get; set; } = 0;

        [Column("return_nocash_num")]
        public int? ReturnNocashNum { get; set; } = 0;

        [Column("bonus")]
        public double? Bonus { get; set; } = 0;

        [Column("bonus_num")]
        public int? BonusNum { get; set; } = 0;

        [Column("status")]
        public bool? Status { get; set; }

        [Column("shop_cash")]
        public double ShopCash { get; set; }

        [Column("shop_cash_num")]
        public double ShopCashNum { get; set; }

        [Column("shop_nocash")]
        public double ShopNocash { get; set; }

        [Column("shop_nocash_num")]
        public double ShopNocashNum { get; set; }

        [Column("cash_box_serial")]
        public string? CashBoxSerial { get; set; }
    }
}
