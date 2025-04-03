using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace WebAPIRestaurantManagement.Models
{
    [Table("Roles")]
    public class RolesModel:BaseModel
    {
        [PrimaryKey("role_id", false)]
        public Guid RoleId { get; set; }
        [Column("role_name")]
        public string RoleName { get; set; }
        [Column("description")]
        public string Description { get; set; }
    }
}
