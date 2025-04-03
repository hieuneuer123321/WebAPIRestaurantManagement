using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIRestaurantManagement.ModelRequests;
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
       /// <summary>
       /// Lấy ds bàn
       /// </summary>
       /// <param name="search"></param>
       /// <param name="PageNumber"></param>
       /// <param name="PageSize"></param>
       /// <param name="isPaging"></param>
       /// <param name="status"></param>
       /// <returns></returns>
        [HttpGet("GetTable")]
        [Authorize] // Đảm bảo rằng người dùng đã đăng nhập
        public async Task<ActionResult> GetTableAsync(string search = "", int PageNumber = 1, int PageSize = 10, bool isPaging = false, bool? status = null)
        {
            ModelDataPageResponse<List<TableResponse>> result = await _tableServices.GetTableAsync(search, PageNumber, PageSize, isPaging, status);
            return Ok(result);
        }
        /// <summary>
        /// Lấy thông tin 1 bàn
        /// </summary>
        /// <param name="locationID">Địa điểm ID</param>
        /// <returns></returns>
        [HttpGet("GetTableById")]

        public async Task<ActionResult> GetTableById(string TableId)
        {
            TableResponse result = await _tableServices.GetTableByIDAsync(Guid.Parse(TableId));
            return Ok(result);
        }
        [HttpPost("UpdateTable")]
        public async Task<ActionResult> UpdateTable(TableResponse request)
        {
            ModelResponse result = await _tableServices.UpdateTableAsync(request);
            return result.IsValid ? Ok(result) : BadRequest(result);
        }
        [HttpPost("DeleteTable")]
        public async Task<ActionResult> DeleteTable(string requestID)
        {
            ModelResponse result = await _tableServices.DeleteTableAsync(Guid.Parse(requestID));
            return result.IsValid ? Ok(result) : BadRequest(result);
        }
        [HttpPost("AddTable")]
        public async Task<ActionResult> AddTable(TableResquests request)
        {
            ModelResponse result = await _tableServices.AddTableAsync(request);
            return result.IsValid ? Ok(result) : BadRequest(result);
        }
    }
}
