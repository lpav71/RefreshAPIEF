using System.ComponentModel.DataAnnotations.Schema;

namespace RefreshAPIEF.Models
{
    [Table("club")]
    public class Club
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("id_group")]
        public int? IdGroup { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("ip")]
        public string? Ip { get; set; }

        [Column("api_key")]
        public string? ApiKey { get; set; }

        [Column("api_key_mobile")]
        public string? ApiKeyMobile { get; set; }

        [Column("local_ip")]
        public string? LocalIp { get; set; }

        [Column("cashbox")]
        public string? CashBox { get; set; }

        [Column("cashbox_port")]
        public string? CashBoxPort { get; set; }

        [Column("max_bonus")]
        public int? MaxBonus { get; set; }

        [Column("time_zone")]
        public int? TimeZone { get; set; }

        [Column("call_token")]
        public string? CallToken { get; set; }
        public string? sound1 { get; set; }
        public string? sound2 { get; set; }
        public string? sound3 { get; set; }
        public string? sound4 { get; set; }
        public string? sound5 { get; set; }
        public string? sound6 { get; set; }
        public string? sound7 { get; set; }
        public string? sound8 { get; set; }
        public string? sound9 { get; set; }
        public string? phone { get; set;}
    }
}
