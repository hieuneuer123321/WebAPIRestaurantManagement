using WebAPIRestaurantManagement.ModelResponses;
using WebAPIRestaurantManagement.Swagger;
using WebAPIRestaurantManagement.Models;
using WebAPIRestaurantManagement.ModelResponses;
using Supabase.Interfaces;
using Supabase.Postgrest.Responses;
using WebAPIRestaurantManagement.Helpers;
using WebAPIRestaurantManagement.ModelRequests;

namespace WebAPIRestaurantManagement.Services.Tables
{
    public class TableServices:ITableServices
    {
        private readonly Supabase.Client _clientSupabase;
        public TableServices(Supabase.Client clientSupabase) {
            _clientSupabase = clientSupabase;
        }

        public async Task<ModelResponse> AddTableAsync(TableResquests table)
        {
            ModelResponse response = new ModelResponse();
            ModeledResponse<TableModel> addResponse = await _clientSupabase.From<TableModel>().Insert(new TableModel
            {
                Table_Number = table.Table_Number,
                Table_Status = table.Table_Status,
                Capacity = table.Capacity,
            });
            if (addResponse == null || addResponse.Models.Count <= 0)
            {
                response.IsValid = false;
                response.ValidationMessages.Add("Add Errors");
            }
            else
            {
                response.IsValid = true;
                response.ValidationMessages.Add("Add Success!");
            }
            return response;
        }

        public async Task<ModelResponse> DeleteTableAsync(Guid tableId)
        {
            ModelResponse response = new ModelResponse();
            TableModel modelToDelete = new TableModel { Table_id = tableId }; // Tạo mô hình muốn xóa
            ModeledResponse<TableModel> deleteResponse = await _clientSupabase
                .From<TableModel>()
                .Delete(modelToDelete);
            // await _supabaseClient.From<CategoriesItemsModel>().Where(x => x.CategoryId == categoryId).Delete();
            if (deleteResponse == null || deleteResponse.Models.Count <= 0)
            {
                response.IsValid = false;
                response.ValidationMessages.Add("Delete Errors");
            }
            else
            {
                response.IsValid = true;
                response.ValidationMessages.Add("Delete Success!");
            }
            return response;
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

        public async Task<TableResponse> GetTableByIDAsync(Guid tableID)
        {
            // Khởi tạo đối tượng response để chứa kết quả
            TableResponse tableResponse = new TableResponse();
            // Gọi từ bảng CategoriesItemsModel và sử dụng phương thức Where để lọc theo categoryID
            ModeledResponse<TableModel> supabaseResponse = await _clientSupabase
                .From<TableModel>()
                .Where(c => c.Table_id == tableID)  // Sử dụng điều kiện để lọc theo categoryID
                .Get();
            // Kiểm tra xem có dữ liệu không
            if (supabaseResponse.Models != null && supabaseResponse.Models.Any())
            {
                // Chuyển kết quả từ SupabaseResponse thành List và gán cho response
                TableModel Items = supabaseResponse.Models.FirstOrDefault();
                TableResponse resultItemResponse = new TableResponse
                {
                    Table_id = Items.Table_id,
                    Table_Number = Items.Table_Number,
                    Table_Status = Items.Table_Status,
                    Capacity = Items.Capacity,
                };
                // Gán dữ liệu vào categoriesResponse (Giả sử CategoriesResponse có một danh sách CategoriesItems)
                tableResponse = resultItemResponse;
            }
            // Trả về response
            return tableResponse;
        }

        public async Task<ModelResponse> UpdateTableAsync(TableResponse table)
        {
            ModelResponse response = new ModelResponse();
            ModeledResponse<TableModel> updateResponse = await _clientSupabase
                              .From<TableModel>()
                              .Where(x => x.Table_id == table.Table_id)
                              .Set(x => x.Table_Number, table.Table_Number)
                              .Set(x => x.Capacity, table.Capacity)
                              .Set(x => x.Table_Status, table.Table_Status)
                              .Update();
            if (updateResponse == null || updateResponse.Models.Count <= 0)
            {
                response.IsValid = false;
                response.ValidationMessages.Add("Update Errors. Category is empty");
            }
            else
            {
                response.IsValid = true;
                response.ValidationMessages.Add("Update Success!");
            }
            return response;
        }
    }
}
