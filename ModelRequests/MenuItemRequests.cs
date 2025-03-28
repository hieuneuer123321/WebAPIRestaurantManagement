namespace WebAPIRestaurantManagement.ModelRequests
{
    public class MenuItemRequests
    {
        public int MenuItemId {  get; set; }    
        public string MenuName { get; set; }
        public long Price { get; set; }
        public int category_id { get; set; }
    }
}
