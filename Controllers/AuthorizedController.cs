using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIRestaurantManagement.Controllers
{
    //[Authorize]  // Bảo vệ tất cả các endpoint trong controller này
    [Route("[controller]")]
    [ApiController]
    public class AuthorizedController : ControllerBase
    {
        [HttpGet("Check Authorized")]
        [Authorize] // Đảm bảo rằng người dùng đã đăng nhập
        public IActionResult GetSecureData()
        {
            return Ok(new { message = "Authorized success!" });
        }
    }
}
