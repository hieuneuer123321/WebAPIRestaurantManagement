using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Xml.Serialization;

namespace WebAPIRestaurantManagement.Models
{
    [Table("Categories")]
    public class CategoriesItemsModel:BaseModel
    {
        [PrimaryKey("category_id", false)]
        public int CategoryId { get; set; }
        [Column("category_name")]
        public string CategoryName { get; set; }
    }
}
