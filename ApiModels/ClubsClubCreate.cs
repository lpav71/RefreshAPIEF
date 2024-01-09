namespace RefreshAPIEF.ApiModels
{
    public class ClubsClubCreate
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Id_group { get; set; } = 0;
        public string Address { get; set; } = "";
        public string Ip { get; set; } = "";
        public string Api_key { get; set; } = "";
        public string Local_ip { get; set; } = "";
        public string Cashbox { get; set; } = "";
        public string Cashbox_port { get; set; } = "";
        public int Max_bonus { get; set; } = 0;
        public int Time_zone { get; set; } = 0;
    }
}
