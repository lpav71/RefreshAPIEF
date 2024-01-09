using RefreshAPIEF.Controllers;

namespace RefreshAPIEF.ApiModels
{
    public class ClientPurchases
    {
        public string? ApiKey { get; set; }
        public string? ApiKeyMobile { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? DataFrom { get; set; }
        public string? DataTo { get; set; }
    }
}
