﻿namespace RefreshAPIEF.ApiModels
{
    public class ClientFindUser
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string SearchText { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
    }
}
