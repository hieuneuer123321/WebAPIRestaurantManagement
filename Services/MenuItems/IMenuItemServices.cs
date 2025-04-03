using WebAPIRestaurantManagement.ModelRequests;
using WebAPIRestaurantManagement.ModelResponses;
using WebAPIRestaurantManagement.Swagger;

namespace WebAPIRestaurantManagement.Services.MenuItems
{
    public interface IMenuItemServices
    {
        Task<ModelDataPageResponse<List<MenuItemResponse>>> GetMenuItemsAsync(string search,List<string> category, int PageNumber, int PageSize, bool isPaging, bool isDescendPrice);
        Task<ModelResponse> UpdateMenuItemsAsync(MenuItemRequests category);
        Task<ModelResponse> AddMenuItemsAsync(MenuItemRequests categoryRequest);
        Task<ModelResponse> DeleteMenuItemsAsync(Guid menuId);
        Task<MenuItemResponse> GetMenuItemByCategoryIDAsync(Guid menuID);
    }
}
