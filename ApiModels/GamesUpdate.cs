namespace RefreshAPIEF.ApiModels
{
    public class GamesUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Icon { get; set; }
        public string Param { get; set; }
        public int Type { get; set; }
        public string SteamId { get; set; }
        public bool ClubAccount { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
