using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIRestaurantManagement.ModelResponses;
using WebAPIRestaurantManagement.Services.Roles;
using WebAPIRestaurantManagement.Swagger;

namespace WebAPIRestaurantManagement.Controllers
{
    //[Authorize] // Đảm bảo rằng người dùng đã đăng nhập
    [Route("[controller]")]
    [ApiController]
    public class RolesRightsController : ControllerBase
    {
        private readonly IRoleServices _roleServices;
        public RolesRightsController(IRoleServices roleServices)
        {
            _roleServices = roleServices;
        }
        // GET: RolesRightsController

        [HttpGet("GetRoles")]
        
        public async Task<ActionResult> GetRoles(string search = "")
        {
            ModelDataResponse<List<RolesResponse>> result = await _roleServices.GetRolesAsync(search);
            return Ok(result);
        }
        [HttpGet("GetRightByRole")]

        public async Task<ActionResult> GetRightByRole(string roleId)
        {
            ModelDataResponse<List<RightResponse>> result = await _roleServices.GetRightByRoleIdAsync(roleId);
            return Ok(result);
        }
    }
}
