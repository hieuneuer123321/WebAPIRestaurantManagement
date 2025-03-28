using Supabase.Postgrest.Responses;
using WebAPIRestaurantManagement.Helpers;
using WebAPIRestaurantManagement.ModelRequests;
using WebAPIRestaurantManagement.ModelResponses;
using WebAPIRestaurantManagement.Models;
using WebAPIRestaurantManagement.Services.CategoriesItems;
using WebAPIRestaurantManagement.Swagger;

namespace WebAPIRestaurantManagement.Services.MenuItems
{
    public class MenuItemServices : IMenuItemServices
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly ICategoriesItemsServices _categoriesItemsServices;
        public MenuItemServices(Supabase.Client client, ICategoriesItemsServices categoriesItemsServices)
        {
            _supabaseClient = client;
            _categoriesItemsServices = categoriesItemsServices;
        }
        public async Task<ModelResponse> AddMenuItemsAsync(MenuItemRequests categoryRequest)
        {
            ModelResponse response = new ModelResponse();
            CategoriesResponse categoryResponse = await _categoriesItemsServices.GetCategoryByCategoryIDAsync(categoryRequest.category_id);
            if (categoryResponse != null)
            {
                ModeledResponse<MenuItemsModel> addResponse = await _supabaseClient.From<MenuItemsModel>().Insert(new MenuItemsModel
                {
                    MenuName = categoryRequest.MenuName,
                    category_id = categoryRequest.category_id,
                    Price = categoryRequest.Price
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
            }
            else {
                response.IsValid = false;
                response.ValidationMessages.Add("Add Errors. Category Menu Item is empty");
            }
            return response;
        }

        public async Task<ModelResponse> DeleteMenuItemsAsync(int menuId)
        {
            ModelResponse response = new ModelResponse();
            MenuItemsModel modelToDelete = new MenuItemsModel { MenuID = menuId }; // Tạo mô hình muốn xóa
            ModeledResponse<MenuItemsModel> deleteResponse = await _supabaseClient
                .From<MenuItemsModel>()
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

        public async Task<ModelDataPageResponse<List<MenuItemResponse>>> GetMenuItemsAsync(string search, List<int> category, int PageNumber, int PageSize, bool isPaging , bool isDescendPrice)
        {
            // Lấy danh sách MenuItems từ Supabase
            ModeledResponse<MenuItemsModel> SupabaseResponseMenuItems = await _supabaseClient.From<MenuItemsModel>().Get();
            List<MenuItemsModel> SupabaseListMenuItems = SupabaseResponseMenuItems.Models.ToList();

            // Tìm kiếm theo tên món ăn (nếu có)
            if (!string.IsNullOrWhiteSpace(search))
            {
                StringConvert conv = new StringConvert();
                search = conv.ConvertToUnSign(search);
                SupabaseListMenuItems = SupabaseListMenuItems.Where(tb => conv.ConvertToUnSign(tb.MenuName).Contains(search)).ToList();
            }

            // Lọc theo danh mục (nếu có)
            if (category!=null)
            {
                if(category.Count > 0)
                    SupabaseListMenuItems = SupabaseListMenuItems.Where(tb => category.Contains(tb.category_id)).ToList();
            }

            // Sử dụng Task.WhenAll để đợi các tác vụ bất đồng bộ
            var menuItemResponsesTasks = SupabaseListMenuItems.Select(async (MenuItemsModel item) =>
            {
                CategoriesResponse categoryResponse = await _categoriesItemsServices.GetCategoryByCategoryIDAsync(item.category_id);
                return new MenuItemResponse
                {
                    MenuID = item.MenuID,
                    MenuName = item.MenuName,
                    Price = item.Price,
                    category = categoryResponse // Trả về đối tượng Category từ service
                };
            }).ToList();
            // Đợi tất cả các tác vụ hoàn tất và trả về kết quả
            List<MenuItemResponse> menuItemResponse = (await Task.WhenAll(menuItemResponsesTasks)).ToList();
            if (isDescendPrice)
            {
                menuItemResponse = menuItemResponse.OrderByDescending(c => c.Price).ToList();
            }
            else {
                menuItemResponse = menuItemResponse.OrderBy(c => c.Price).ToList();
            }
            ModelDataPageResponse<List<MenuItemResponse>> result =
               Helpers.PaginationHelper.createPageDataResponse<List<MenuItemResponse>>(menuItemResponse.Count, PageNumber, PageSize, false);
            menuItemResponse = isPaging ? menuItemResponse.Skip((result.currentPage - 1) * result.pageSize).Take(result.pageSize).ToList() : menuItemResponse;
            result.items = menuItemResponse;
            return result;
        }

        public async Task<ModelResponse> UpdateMenuItemsAsync(MenuItemRequests item)
        {
            ModelResponse response = new ModelResponse();
            CategoriesResponse categoryResponse = await _categoriesItemsServices.GetCategoryByCategoryIDAsync(item.category_id);
            if (categoryResponse != null)
            {
                ModeledResponse<MenuItemsModel> updateResponse = await _supabaseClient
                              .From<MenuItemsModel>()
                              .Where(x => x.MenuID == item.MenuItemId)
                              .Set(x => x.MenuName, item.MenuName)
                              .Set(x => x.category_id, item.category_id)
                              .Update();
                if (updateResponse == null || updateResponse.Models.Count <= 0)
                {
                    response.IsValid = false;
                    response.ValidationMessages.Add("Update Errors. Menu Item is empty");
                }
                else
                {
                    response.IsValid = true;
                    response.ValidationMessages.Add("Update Success!");
                }
            }
            else {
                response.IsValid = false;
                response.ValidationMessages.Add("Update Errors. Category Menu Item is empty");
            }
            return response;
        }
        public async Task<MenuItemResponse> GetMenuItemByCategoryIDAsync(int menuID)
        {
            // Khởi tạo đối tượng response để chứa kết quả
            MenuItemResponse menuResponse = new MenuItemResponse();
            // Gọi từ bảng CategoriesItemsModel và sử dụng phương thức Where để lọc theo categoryID
            ModeledResponse<MenuItemsModel> supabaseResponseMenus = await _supabaseClient
                .From<MenuItemsModel>()
                .Where(c => c.MenuID == menuID)  // Sử dụng điều kiện để lọc theo categoryID
                .Get();
            // Kiểm tra xem có dữ liệu không
            if (supabaseResponseMenus.Models != null && supabaseResponseMenus.Models.Any())
            {
                MenuItemsModel supabaseMenu = supabaseResponseMenus.Models.FirstOrDefault();
                CategoriesResponse categoryResponse = await _categoriesItemsServices.GetCategoryByCategoryIDAsync(supabaseMenu.category_id);
                // Sử dụng Task.WhenAll để đợi các tác vụ bất đồng bộ
                menuResponse = new MenuItemResponse
                {
                    MenuID = supabaseMenu.category_id,
                    MenuName = supabaseMenu.MenuName,
                    Price = supabaseMenu.Price,
                    category = categoryResponse // Trả về đối tượng Category từ service
                };
                // Đợi tất cả các tác vụ hoàn tất và trả về kết quả
            }
            // Trả về response
            return menuResponse;
        }
    }
}
