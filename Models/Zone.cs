using System.ComponentModel.DataAnnotations.Schema;

namespace RefreshAPIEF.Models
{
    [Table("zone")]
    public class Zone
    {
        public int id { get; set; }
        public int club_id { get; set; }
        public int num { get; set; }
        public string name { get; set; }
    }
}
