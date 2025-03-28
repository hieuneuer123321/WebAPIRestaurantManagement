using WebAPIRestaurantManagement.ModelResponses;
using WebAPIRestaurantManagement.Swagger;
using WebAPIRestaurantManagement.Models;
using WebAPIRestaurantManagement.ModelResponses;
using Supabase.Interfaces;
using Supabase.Postgrest.Responses;
using WebAPIRestaurantManagement.Helpers;

namespace WebAPIRestaurantManagement.Services.Tables
{
    public class TableServices:ITableServices
    {
        private readonly Supabase.Client _clientSupabase;
        public TableServices(Supabase.Client clientSupabase) {
            _clientSupabase = clientSupabase;
        }

        public Task<ModelResponse> AddTableAsync(TableResponse table)
        {
            throw new NotImplementedException();
        }

        public Task<ModelResponse> DeleteTableAsync(int tableId)
        {
            throw new NotImplementedException();
        }

        public async Task<ModelDataPageResponse<List<TableResponse>>> GetTableAsync(string? search, int PageNumber, int PageSize, bool isPaging, bool? status)
        {
            ModeledResponse<TableModel> SupabaseResponse = await _clientSupabase.From<TableModel>().Get();
            List<TableModel> ListItems = SupabaseResponse.Models.ToList();
            if (!string.IsNullOrWhiteSpace(search))
            {
                StringConvert conv = new StringConvert();
                search = conv.ConvertToUnSign(search);
                ListItems = ListItems.Where(tb => conv.ConvertToUnSign(tb.TableName).Contains(search)).ToList();
            }
            if (status != null)
            {
                ListItems = ListItems.Where(tb => tb.Table_Status == status).ToList();
            }
            List<TableResponse> convertItemResponse = ListItems.Select(u => new TableResponse
            {
                Table_id = u.Table_id,
                Table_Number = u.Table_Number,
                Table_Status = u.Table_Status,
                Capacity = u.Capacity,
            }).ToList();
            ModelDataPageResponse<List<TableResponse>> result =
                Helpers.PaginationHelper.createPageDataResponse<List<TableResponse>>(convertItemResponse.Count, PageNumber, PageSize, false);
            convertItemResponse = isPaging ? convertItemResponse.Skip((result.currentPage - 1) * result.pageSize).Take(result.pageSize).ToList() : convertItemResponse;
            result.items = convertItemResponse;
            return result;
        }

        public Task<MenuItemResponse> GetTableByIDAsync(int tableID)
        {
            throw new NotImplementedException();
        }

        public Task<ModelResponse> UpdateTableAsync(TableResponse table)
        {
            throw new NotImplementedException();
        }
    }
}
