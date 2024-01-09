#pragma warning disable IDE1006 // Стили именования
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RefreshAPIEF.Models
{
    [Table("users")]
    public class Users
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public DateTime? email_verified_at { get; set; }
        public string? password { get; set; }
        public string? role { get; set; }
        public string? remember_token { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string? surname { get; set; }
        public string? name_auth { get; set; }
        public string? middle_name { get; set; }
        public DateTime? birthday { get; set; }
        public string? phone { get; set; }
        public string? icon { get; set; }
        public string? sex { get; set; }
        public string? date_of_employment { get; set; }
        public int? club_id { get; set; }
    }
}
