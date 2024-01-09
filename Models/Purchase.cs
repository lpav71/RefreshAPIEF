using Microsoft.EntityFrameworkCore;

namespace RefreshAPIEF.Models
{
    [Keyless]
    public class Purchase
    {
        public int id { get; set; }
        public string purchase { get; set; }
    }
}
