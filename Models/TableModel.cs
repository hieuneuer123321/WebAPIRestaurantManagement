using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace WebAPIRestaurantManagement.Models
{
    [Table("Tables")]
    public class TableModel : BaseModel
    {
        [PrimaryKey("table_id", false)]
        public int Table_id { get; set; }
        [Column("table_number")]
        public string Table_Number { get; set; }
        [Column("capacity")]
        public string Capacity { get; set; }
        [Column("status")]
        public bool Table_Status { get; set; }
    }
}
