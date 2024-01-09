namespace RefreshAPIEF.ApiModels
{
    public class PriceGetTariffs
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int IdZone { get; set; } = 0;
        public string ApiKey { get; set; }
    }
}
