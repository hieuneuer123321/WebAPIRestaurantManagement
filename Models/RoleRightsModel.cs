using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace WebAPIRestaurantManagement.Models
{
    [Table("Role_Right")]
    public class RoleRightsModel:BaseModel
    {
        [PrimaryKey("role_id", false)]
        public Guid RoleId { get; set; }
        [Column("right_id")]
        public string RightId { get; set; }
        [Column("description")]
        public string Description { get; set; }
    }
}
