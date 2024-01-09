using System.ComponentModel.DataAnnotations.Schema;

namespace RefreshAPIEF.Models
{
    [Table("client_wallets")]
    public class ClientWallets
    {
        public int id { get; set; }
        public int? club_id { get; set; }
        public int? user_id { get; set; }
        public double? money { get; set; }
        public double? bonus { get; set; }
        public DateTime? create_date { get; set; }
        public bool? status { get; set; }
    }
}
