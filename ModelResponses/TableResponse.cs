namespace WebAPIRestaurantManagement.ModelResponses
{
    public class TableResponse
    {
        public Guid Table_id { get; set; }
        public string Table_Number { get; set; }
        public int Capacity { get; set; }
        public bool Table_Status { get; set; }
    }
}
