using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

namespace WebAPIRestaurantManagement.Models
{
    [Table("Users")]
    public class UsersModel : BaseModel
    {

        //"user_id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
        //"username" VARCHAR(255) UNIQUE NOT NULL,
        //"role_id" UUID NOT NULL,
        //"full_name" VARCHAR(255) NOT NULL,
        //"email" VARCHAR(255) UNIQUE NOT NULL,
        //"user_number" VARCHAR(255) UNIQUE NOT NULL,
        //"date_birth" timestamp default NULL,
        //"phone" VARCHAR(20),
        //"status" boolean DEFAULT false,
        //"verify_email" boolean DEFAULT false,
        //"last_login" timestamp default CURRENT_TIMESTAMP,
        //"create_date" timestamp default CURRENT_TIMESTAMP,
        [PrimaryKey("user_id", false)]
        public Guid User_id { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("create_date")]
        public DateOnly Date_create { get; set; }
        [Column("date_birth")]
        public DateOnly? Date_birth { get; set; }
        [Column("last_login")]
        public DateTime? Last_login { get; set; }
        [Column("role_id")]
        public Guid Role_id { get; set; }
        [Column("status")]
        public bool Status { get; set; }
        [Column("phone")]
        public string? Phone { get; set; }
        [Column("full_name")]
        public string? FullName { get; set; }
        [Column("user_number")]
        public string? User_Number { get; set; }
        [Column("verify_email")]
        public bool? Verify_email { get; set; }
    }
}
