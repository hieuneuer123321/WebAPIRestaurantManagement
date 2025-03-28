using WebAPIRestaurantManagement.ModelResponses;
using WebAPIRestaurantManagement.Models;
using WebAPIRestaurantManagement.Swagger;

namespace WebAPIRestaurantManagement.Services.CategoriesItems
{
    public interface ICategoriesItemsServices
    {
        Task<ModelDataPageResponse<List<CategoriesResponse>>> GetCategoryItemsAsync(string search, int PageNumber, int PageSize, bool isPaging);
        Task<ModelResponse> UpdateCategoryItemsAsync(CategoriesResponse category);
        Task<ModelResponse> AddCategoryItemsAsync(string categoryName);
        Task<ModelResponse> DeleteCategoryItemsAsync(int categoryId);
        Task<CategoriesResponse> GetCategoryByCategoryIDAsync(int categoryID);
    }
}
