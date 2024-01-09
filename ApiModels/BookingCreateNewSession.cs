namespace RefreshAPIEF.ApiModels
{
    public class BookingCreateNewSession
    {
        public int MapCompId { get; set; }
        public int PriceId { get; set; }
        public string ApiKey { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
