using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace WebAPIRestaurantManagement.Models
{
    [Table("users")]
    public class AuthModel:BaseModel
    {
        [PrimaryKey("id", false)]
        public int auth_id { get; set; }
        [Column("email")]
        public string auth_email { get; set; }
      
    }
}
