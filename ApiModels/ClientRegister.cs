using System.ComponentModel.DataAnnotations.Schema;

namespace RefreshAPIEF.ApiModels
{
    public class ClientRegister
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateTime? Bday { get; set; }
        public double? Amount { get; set; }
        public double? Bonus { get; set; }
        public int? TotalTime { get; set; }
        public bool? StatusActive { get; set; }
        public string? TelegramId { get; set; }
        public string? VkId { get; set; }
        public bool? Verify { get; set; }
        public DateTime? VerifyDt { get; set; }
        public string? Name { get; set; }
        public string? SurName { get; set; }
        public string? MiddleName { get; set; }
        public string ApiKey { get; set; } = string.Empty;
        public string MobileApiKey { get; set; } = string.Empty;
    }
}
