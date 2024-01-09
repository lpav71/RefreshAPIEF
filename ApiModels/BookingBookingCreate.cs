namespace RefreshAPIEF.ApiModels
{
    public class BookingBookingCreate
    {
        public int MapCompId { get; set; }
        public int PriceId { get; set; }
        public string? ApiKey { get; set; }
        public string? MobileApiKey { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? DateTimeStart { get; set; }
    }

}
