using Supabase.Gotrue;
using Supabase.Postgrest.Responses;
using WebAPIRestaurantManagement.Helpers;
using WebAPIRestaurantManagement.ModelResponses;
using WebAPIRestaurantManagement.Models;
using WebAPIRestaurantManagement.Swagger;
using static Supabase.Postgrest.Constants;

namespace WebAPIRestaurantManagement.Services.CategoriesItems
{
    public class CategoriesItemsService : ICategoriesItemsServices
    {
        private readonly Supabase.Client _supabaseClient;
        public CategoriesItemsService(Supabase.Client client)
        {
            _supabaseClient = client;
        }
        public async Task<ModelDataPageResponse<List<CategoriesResponse>>> GetCategoryItemsAsync(string search, int PageNumber, int PageSize, bool isPaging)
        {
            ModeledResponse<CategoriesItemsModel> SupabaseResponseCategory = await _supabaseClient.From<CategoriesItemsModel>().Get();
            List<CategoriesItemsModel> ListcategoriesItems = SupabaseResponseCategory.Models.ToList();
            if (!string.IsNullOrWhiteSpace(search))
            {
                StringConvert conv = new StringConvert();
                search = conv.ConvertToUnSign(search);
                ListcategoriesItems = ListcategoriesItems.Where(tb => conv.ConvertToUnSign(tb.CategoryName).Contains(search)).ToList();
            }
            List<CategoriesResponse> resultCategoriesItemResponse = ListcategoriesItems.Select(u => new CategoriesResponse
            {
                CategoryId = u.CategoryId,
                CategoryName = u.CategoryName,
            }).ToList();
            ModelDataPageResponse<List<CategoriesResponse>> result =
                Helpers.PaginationHelper.createPageDataResponse<List<CategoriesResponse>>(resultCategoriesItemResponse.Count, PageNumber, PageSize, false);
            resultCategoriesItemResponse = isPaging ? resultCategoriesItemResponse.Skip((result.currentPage - 1) * result.pageSize).Take(result.pageSize).ToList() : resultCategoriesItemResponse;
            result.items = resultCategoriesItemResponse;
            return result;
        }
        public async Task<CategoriesResponse> GetCategoryByCategoryIDAsync(int categoryID)
        {
            // Khởi tạo đối tượng response để chứa kết quả
            CategoriesResponse categoriesResponse = new CategoriesResponse();
            // Gọi từ bảng CategoriesItemsModel và sử dụng phương thức Where để lọc theo categoryID
            ModeledResponse<CategoriesItemsModel> supabaseResponseCategory = await _supabaseClient
                .From<CategoriesItemsModel>()
                .Where(c => c.CategoryId == categoryID)  // Sử dụng điều kiện để lọc theo categoryID
                .Get();
            // Kiểm tra xem có dữ liệu không
            if (supabaseResponseCategory.Models != null && supabaseResponseCategory.Models.Any())
            {
                // Chuyển kết quả từ SupabaseResponse thành List và gán cho response
                CategoriesItemsModel CategoriesItems = supabaseResponseCategory.Models.FirstOrDefault();
                CategoriesResponse resultCategoriesItemResponse = new CategoriesResponse
                {
                    CategoryId = CategoriesItems.CategoryId,
                    CategoryName = CategoriesItems.CategoryName,
                };
                // Gán dữ liệu vào categoriesResponse (Giả sử CategoriesResponse có một danh sách CategoriesItems)
                categoriesResponse = resultCategoriesItemResponse;
            }
            // Trả về response
            return categoriesResponse;
        }
        public  async Task<ModelResponse> UpdateCategoryItemsAsync(CategoriesResponse category)
        {
            ModelResponse response = new ModelResponse();
            ModeledResponse<CategoriesItemsModel> updateResponse = await _supabaseClient
                              .From<CategoriesItemsModel>()
                              .Where(x => x.CategoryId == category.CategoryId)
                              .Set(x => x.CategoryName, category.CategoryName)
                              .Update();
            if (updateResponse == null || updateResponse.Models.Count <= 0)
            {
                response.IsValid = false;
                response.ValidationMessages.Add("Update Errors. Category is empty");       
            }
            else {
                response.IsValid = true;
                response.ValidationMessages.Add("Update Success!");
            }
            return response;
        }
        public async Task<ModelResponse> AddCategoryItemsAsync(string categoryName)
        {
            ModelResponse response = new ModelResponse();
            ModeledResponse<CategoriesItemsModel> addResponse = await _supabaseClient.From<CategoriesItemsModel>().Insert(new CategoriesItemsModel
            {
                CategoryName = categoryName
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
        public async Task<ModelResponse> DeleteCategoryItemsAsync(int categoryId)
        {
            ModelResponse response = new ModelResponse();
            CategoriesItemsModel modelToDelete = new CategoriesItemsModel { CategoryId = categoryId }; // Tạo mô hình muốn xóa
            ModeledResponse<CategoriesItemsModel> deleteResponse = await _supabaseClient
                .From<CategoriesItemsModel>()
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
    }
}
