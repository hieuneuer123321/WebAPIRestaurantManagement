using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;


namespace WebAPIRestaurantManagement.Models
{
    [Table("Menu")]
    public class MenuItemsModel :BaseModel
    {
        [PrimaryKey("menu_id", false)]
        public Guid MenuID { get; set; }
        [Column("name")]
        public string MenuName { get; set; }
        [Column("price")]
        public long Price { get; set; }
        [Column("category_id")]
        public Guid category_id { get; set; }
    }
}
