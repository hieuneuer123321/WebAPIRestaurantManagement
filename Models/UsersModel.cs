using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace WebAPIRestaurantManagement.Models
{
    [Table("accounts")]
    public class UsersModel : BaseModel
    {
        [PrimaryKey("account_id", false)]
        public int account_id { get; set; }
        [Column("username")]
        public string username { get; set; }
        [Column("email")]
        public string email { get; set; }
        [Column("date_create")]
        public string date_create { get; set; }
        [Column("password")]
        public string password { get; set; }
        [Column("account_type")]
        public int account_type { get; set; }
        [Column("status")]
        public bool status { get; set; }
        [Column("student_id")]
        public int? student_id { get; set; }
        [Column("teacher_id")]
        public int? teacher_id { get; set; }
        [Column("userId_auth")]
        public string userId_auth { get; set; }

    }
}
