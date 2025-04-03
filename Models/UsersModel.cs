using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace WebAPIRestaurantManagement.Models
{
    [Table("Users")]
    public class UsersModel : BaseModel
    {
        [PrimaryKey("user_id", false)]
        public Guid User_id { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("create_date")]
        public DateOnly Date_create { get; set; }
        [Column("role_id")]
        public int Role_id { get; set; }
        [Column("status")]
        public bool Status { get; set; }
        [Column("phone")]
        public string? Phone { get; set; }
        [Column("full_name")]
        public string? FullName { get; set; }
        [Column("user_number")]
        public string? User_Number { get; set; }
        [Column("email_confirmed")]
        public bool? email_confirmed { get; set; }
    }
}
