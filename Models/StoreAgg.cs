#pragma warning disable IDE1006 // Стили именования
using Microsoft.EntityFrameworkCore;

namespace RefreshAPIEF.Models
{
    [Keyless]

    public class StoreAgg
    {
        public string product { get; set; }
    }
}
