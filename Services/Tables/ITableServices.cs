using WebAPIRestaurantManagement.ModelRequests;
using WebAPIRestaurantManagement.ModelResponses;
using WebAPIRestaurantManagement.Models;
using WebAPIRestaurantManagement.Swagger;

namespace WebAPIRestaurantManagement.Services.Tables
{
    public interface ITableServices
    {
        Task<ModelDataPageResponse<List<TableResponse>>> GetTableAsync(string? search, int PageNumber, int PageSize, bool isPaging, bool? status);
        Task<ModelResponse> UpdateTableAsync(TableResponse table);
        Task<ModelResponse> AddTableAsync(TableResquests table);
        Task<ModelResponse> DeleteTableAsync(int tableId);
        Task<TableResponse> GetTableByIDAsync(int tableID);
    }
}
