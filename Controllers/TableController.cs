using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIRestaurantManagement.ModelResponses;
using WebAPIRestaurantManagement.Services.Tables;
using WebAPIRestaurantManagement.Swagger;

namespace WebAPIRestaurantManagement.Controllers
{
    [Authorize]  // Bảo vệ tất cả các endpoint trong controller này
    [Route("[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly ITableServices _tableServices;
        public TableController(ITableServices tableServices)
        {
            _tableServices = tableServices;
        }
        [HttpGet("GetCategories")]
        [Authorize] // Đảm bảo rằng người dùng đã đăng nhập
        public async Task<ActionResult> GetCategoriesAsync(string search = "", int PageNumber = 1, int PageSize = 10, bool isPaging = false, bool? status = null)
        {
            ModelDataPageResponse<List<TableResponse>> result = await _tableServices.GetTableAsync(search, PageNumber, PageSize, isPaging, status);
            return Ok(result);
        }
    }
}
