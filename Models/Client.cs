using Microsoft.EntityFrameworkCore;
using RefreshAPIEF.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace RefreshAPIEF.Models
{
    [Table("clients")]
    public class Client
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("club_id")]
        public int? ClubId { get; set; }

        [Column("login")]
        public string? Login { get; set; }

        [Column("password")]
        public string? Password { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("icon")]
        public string? Icon { get; set; }

        [Column("amount")]
        public double? Amount { get; set; }

        [Column("bonus")]
        public double? Bonus { get; set; }

        [Column("total_time")]
        public int? TotalTime { get; set; }

        [Column("full_name")]
        public string? FullName { get; set; }

        [Column("status_active")]
        public bool? StatusActive { get; set; }

        [Column("telegram_id")]
        public string? TelegramId { get; set; }

        [Column("vk_id")]
        public string? VkId { get; set; }

        [Column("reg_date")]
        public DateTime? RegDate { get; set; }

        [Column("bday")]
        public DateTime? BDay { get; set; }

        [Column("verify")]
        public bool? Verify { get; set; }

        [Column("verify_dt")]
        public DateTime? VerifyDt { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("surname")]
        public string? SurName { get; set; }

        [Column("middle_name")]
        public string? MiddleName { get; set; }        
    }
}
