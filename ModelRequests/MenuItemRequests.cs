namespace WebAPIRestaurantManagement.ModelRequests
{
    public class MenuItemRequests
    {
        public Guid MenuItemId {  get; set; }    
        public string MenuName { get; set; }
        public long Price { get; set; }
        public Guid category_id { get; set; }
    }
}
