using WebAPIRestaurantManagement.ModelRequests;
using WebAPIRestaurantManagement.ModelResponses;
using WebAPIRestaurantManagement.Swagger;

namespace WebAPIRestaurantManagement.Services.MenuItems
{
    public interface IMenuItemServices
    {
        Task<ModelDataPageResponse<List<MenuItemResponse>>> GetMenuItemsAsync(string search,List<int> category, int PageNumber, int PageSize, bool isPaging, bool isDescendPrice);
        Task<ModelResponse> UpdateMenuItemsAsync(MenuItemRequests category);
        Task<ModelResponse> AddMenuItemsAsync(MenuItemRequests categoryRequest);
        Task<ModelResponse> DeleteMenuItemsAsync(int menuId);
        Task<MenuItemResponse> GetMenuItemByCategoryIDAsync(int menuID);
    }
}
