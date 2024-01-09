using System.ComponentModel.DataAnnotations.Schema;

namespace RefreshAPIEF.Models
{
    [Table("type_operation")]
    public class TypeOperation
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
