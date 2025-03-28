namespace WebAPIRestaurantManagement.ModelResponses
{
    public class MenuItemResponse
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public long Price { get; set; }
        public CategoriesResponse category { get; set; }
    }
}
