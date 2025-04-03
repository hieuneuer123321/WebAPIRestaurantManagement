using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace WebAPIRestaurantManagement.Models
{
    [Table("test")]
    public class TestModel:BaseModel
    {
       
        [PrimaryKey("id", false)]
        public Guid id { get; set; }
        [Column("name")]
        public string name { get; set; }
    }
}
