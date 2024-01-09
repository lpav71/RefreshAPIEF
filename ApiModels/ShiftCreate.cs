namespace RefreshAPIEF.ApiModels
{
    public class ShiftCreate
    {
        public int AdminId { get; set; } = 0;
        public string AdminName { get; set; } = string.Empty;
        public int Shift { get; set; } = 0;
        public string CashBoxSerial { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
    }
}
