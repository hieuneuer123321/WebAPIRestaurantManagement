using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebAPIRestaurantManagement.ModelRequests;
using WebAPIRestaurantManagement.ModelResponses;
using WebAPIRestaurantManagement.Models;
using WebAPIRestaurantManagement.Services.CategoriesItems;
using WebAPIRestaurantManagement.Services.MenuItems;
using WebAPIRestaurantManagement.Swagger;

namespace WebAPIRestaurantManagement.Controllers
{
    [Authorize]  // Bảo vệ tất cả các endpoint trong controller này
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesItemsServices _categoriesItemsServices;
        private readonly IMenuItemServices _menuItemServices;
        public CategoriesController(ICategoriesItemsServices categoriesItemsServices, IMenuItemServices menuItemServices)
        {
            _categoriesItemsServices = categoriesItemsServices;
            _menuItemServices = menuItemServices;       
        }
        [HttpGet("GetCategories")]
        [Authorize] // Đảm bảo rằng người dùng đã đăng nhập
        public  async Task<ActionResult> GetCategoriesAsync(string search ="", int PageNumber = 1, int PageSize = 10, bool isPaging = false)
        {
            ModelDataPageResponse<List<CategoriesResponse>> result = await _categoriesItemsServices.GetCategoryItemsAsync(search,PageNumber, PageSize,isPaging);
            return Ok(result);
        }
        [HttpPost("UpdateCategories")]
        public async Task<ActionResult> UpdateCategoryItems(CategoriesResponse request)
        {
            ModelResponse result = await _categoriesItemsServices.UpdateCategoryItemsAsync(request);
            return result.IsValid ? Ok(result) : BadRequest(result);
        }
        [HttpPost("AddCategories")]
        public async Task<ActionResult> AddCategoryItems(string categoryName)
        {
            ModelResponse result = await _categoriesItemsServices.AddCategoryItemsAsync(categoryName);
            return result.IsValid ? Ok(result) : BadRequest(result);
        }
        [HttpPost("DeleteCategories")]
        public async Task<ActionResult> DeleteCategoryItems(int categoryId)
        {
            ModelResponse result = await _categoriesItemsServices.DeleteCategoryItemsAsync(categoryId);
            return result.IsValid ? Ok(result) : BadRequest(result);
        }
        [HttpPost("GetItemsMenu")]
        public async Task<ActionResult> GetItemsMenu(string search = "" , int PageNumber = 1, int PageSize = 10, bool isPaging = false, bool isDecPrice = true, List<int> categoryItem = null)
        {
            ModelDataPageResponse<List<MenuItemResponse>> result = await _menuItemServices.GetMenuItemsAsync(search = "", categoryItem, PageNumber, PageSize, isPaging, isDecPrice = true);
            return Ok(result);
        }
        [HttpPost("UpdateMenuItem")]
        public async Task<ActionResult> UpdateMenuItem(MenuItemRequests request)
        {
            ModelResponse result = await _menuItemServices.UpdateMenuItemsAsync(request);
            return result.IsValid ? Ok(result) : BadRequest(result);
        }
        [HttpPost("AddMenuItem")]
        public async Task<ActionResult> AddMenuItem(MenuItemRequests requests)
        {
            ModelResponse result = await _menuItemServices.AddMenuItemsAsync(requests);
            return result.IsValid ? Ok(result) : BadRequest(result);
        }
        [HttpPost("DeleteMenuItem")]
        public async Task<ActionResult> DeleteMenuItem(int MenuId)
        {
            ModelResponse result = await _menuItemServices.DeleteMenuItemsAsync(MenuId);
            return result.IsValid ? Ok(result) : BadRequest(result);
        }
        /// <summary>
        /// Xoá 1 hoặc nhiều Địa điểm cùng lúc
        /// </summary>
        /// <param name="locationID">Địa điểm ID</param>
        /// <returns></returns>
        [HttpPost("GetItemMenuByMenuId")]
      
        public async Task<ActionResult> GetItemMenuByMenuId(int MenuId)
        {
            MenuItemResponse result = await _menuItemServices.GetMenuItemByCategoryIDAsync(MenuId);
            return Ok(result);
        }
    }
}
