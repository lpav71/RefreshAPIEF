namespace RefreshAPIEF.ApiModels
{
    public class PriceAdd
    {
        public int ZoneId { get; set; }
        public double pPrice { get; set; }
        public int Weekday { get; set; }
        public int Duration { get; set; }
        public int TariffType { get; set; }
        public bool StatusActive { get; set; } = true;
        public TimeOnly TimeStart { get; set; }
        public TimeOnly TimeStop { get; set; }
        public TimeOnly TimeFixed { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
    }
}
